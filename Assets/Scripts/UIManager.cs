﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI popupDamage;
    public GameObject changeTurnText;
    public Image fightImage;
    public List<GameObject> playersTurns;
    public GameObject enemyHealth;
    public GameObject turnUIBar;
    public GameObject turnUI;

    private int minWidthTurnUIBar = 585;
    private int spaceTurnUIBar = 55;
    private List<GameObject> healthEnemies;
    private List<GameObject> turnsBarUI;


    private void Awake()
    {
        StartCoroutine(SetPlayersImage());
        StartCoroutine(SetTurnList());
    }

    private void Update()
    {
        //SetTextHealth();
    }

    public void ShowPopupDamage(int damage, Transform location)
    {
        TextMeshProUGUI popup = Instantiate(popupDamage);
        popup.transform.SetParent(transform, false);
        popup.transform.position = location.position + Vector3.up;
        popup.text = damage.ToString();
        StartCoroutine(DestroyText(popup, 1));
    }

    /**
     * 
     * Metodo che fa apparire la UI del personaggio selezionato
     * 
     **/
    public void ShowPlayerUI(GameObject player)
    {
        player.GetComponent<PlayerController>().playerUI.SetActive(true);
    }

    /**
     * 
     * Metodo che nasconde la UI del personaggio selezionato
     * 
     **/
    public void HidePlayerUI(GameObject player)
    {
        player.GetComponent<PlayerController>().playerUI.SetActive(false);
    }

    public void SetChangeTurnText(string text)
    {
        GameObject turnText = Instantiate(changeTurnText);
        turnText.transform.SetParent(transform, false);
        turnText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(text);
        StartCoroutine(DestroyObject(turnText, 1f));
    }

    public void ShowImageFight()
    {
        Image img = Instantiate(fightImage);
        img.transform.SetParent(transform, false);
        StartCoroutine(DestroyImage(img, 2.5f));
    }

    public void SetPlayerHealthBar(GameObject player)
    {
        int totalHealth = player.GetComponent<ObjectController>().totalHealth;
        int currentHealth = player.GetComponent<ObjectController>().currentHealth;
        float barHealth = (float) currentHealth / totalHealth;
        int playerNumber = player.GetComponent<PlayerController>().playerNumber;

        GameObject healthBar = playersTurns[playerNumber].transform.GetChild(1).transform.GetChild(1).gameObject;
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(barHealth, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    //public void SetTextHealth()
    //{
    //    for(int i=0; i<TileManager.playerInstance.Count;i++)
    //    {
    //        if (TileManager.playerInstance[i])
    //        {
    //            healthTexts[i].SetText(TileManager.playerInstance[i].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
    //        }
    //    }
    //}

    public void CreateEnemyUI(List<GameObject> enemyInstance)
    {
        healthEnemies = new List<GameObject>();
        for (int i=0;i<enemyInstance.Count;i++)
        {
            GameObject enemyUIInstance = Instantiate(enemyHealth);
            enemyUIInstance.transform.SetParent(transform, false);

            enemyUIInstance.GetComponent<EnemyHealth>().target = enemyInstance[i].transform;

            for (int p = 0; p < enemyUIInstance.transform.childCount; p++)
            {
                if (enemyUIInstance.transform.GetChild(p).name == "HealthText")
                {
                    enemyUIInstance.transform.GetChild(p).GetComponent<TextMeshProUGUI>().SetText(enemyInstance[i].GetComponent<ObjectController>().currentHealth.ToString() + "/" + enemyInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
                }
            }

            healthEnemies.Add(enemyUIInstance);
        }
    }

    public void DestroyEnemyUI(int enemyPosition)
    {
        Destroy(healthEnemies[enemyPosition]);
        healthEnemies[enemyPosition] = null;
    }

    public void ClearEnemyList()
    {
        healthEnemies.Clear();
    }

    public void SetEnemyHealth(int enemyNumber, GameObject target)
    {
        for (int p = 0; p < healthEnemies[enemyNumber].transform.childCount; p++)
        {
            if (healthEnemies[enemyNumber] && healthEnemies[enemyNumber].transform.GetChild(p).name == "HealthText")
            {
                healthEnemies[enemyNumber].transform.GetChild(p).GetComponent<TextMeshProUGUI>().SetText(target.GetComponent<ObjectController>().currentHealth.ToString() + "/" + target.GetComponent<ObjectController>().totalHealth.ToString() + " HP");
                break;
            }
        }
    }

    public void DisablePlayerHUD(int playerNumber)
    {
        //for(int i=0; i<TileManager.playerInstance.Count; i++)
        //{
        //    if(TileManager.playerInstance[i].GetComponent<PlayerController>().playerNumber == playerNumber)
        //    {
                //healthTexts[i].SetText("0/" + TileManager.playerInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
                //playersImage[i].GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                //healthTexts.RemoveAt(i);
                //playersImage.RemoveAt(i);
                playersTurns[playerNumber].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                playersTurns.RemoveAt(playerNumber);
        //    }
        //}
    }

    public void SetPlayerTurnColor(GameObject player)
    {
        int playerNumber = player.GetComponent<PlayerController>().playerNumber;
        Color tileColor = player.GetComponent<PlayerController>().colorTile;
        playersTurns[playerNumber].GetComponent<Image>().color = new Color(tileColor.r, tileColor.g, tileColor.b, 0.5f);
    }

    public void ResetColor()
    {
        foreach(GameObject ui in playersTurns)
        {
            if(ui)
            {
                ui.GetComponent<Image>().color = new Color(255, 255, 255, 0.8f);
            }
        }
    }

    public void SetTurnBarUI(List<GameObject> turns, int currentTurn)
    {
        turnsBarUI[0].GetComponent<Image>().color = Color.red;
        turnsBarUI[0].GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f);
        turnsBarUI[0].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = turns[currentTurn].GetComponent<ObjectController>().iconObject;

        int barPosition = 1;
        for(int i=0; i<turns.Count; i++)
        {
            if(i != currentTurn)
            {
                turnsBarUI[barPosition].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = turns[i].GetComponent<ObjectController>().iconObject;
                turnsBarUI[barPosition].GetComponent<Image>().color = Color.white;
                barPosition++;
            }
        }
    }

    public void DisableTurnUI(int turnPosition)
    {
        turnsBarUI[turnPosition].GetComponent<Image>().color = Color.white;
        turnsBarUI[turnPosition].GetComponent<RectTransform>().localScale = Vector3.one;
    }

    IEnumerator SetTurnList()
    {
        while (TurnManager.turns == null || TurnManager.turns.Count == 0)
        {
            yield return null;
        }

        turnsBarUI = new List<GameObject>();
        turnUIBar.SetActive(true);
        turnUIBar.GetComponent<RectTransform>().sizeDelta = new Vector2(150 * TurnManager.turns.Count, turnUIBar.GetComponent<RectTransform>().sizeDelta.y);
        foreach (GameObject turn in TurnManager.turns)
        {
            GameObject turnUIInstance = Instantiate(turnUI);
            turnUIInstance.transform.SetParent(turnUIBar.transform.GetChild(0).transform, false);
            turnsBarUI.Add(turnUIInstance);
        }
    }

    IEnumerator SetPlayersImage()
    {
        while(TileManager.playerInstance == null || TileManager.playerInstance.Count == 0)
        {
            yield return null;
        }

        for(int i=0; i<TileManager.playerInstance.Count;i++)
        {
            playersTurns[i].transform.GetChild(0).GetComponent<Image>().sprite = TileManager.playerInstance[i].GetComponent<ObjectController>().iconObject;
        }
    }

    IEnumerator DestroyText(TextMeshProUGUI obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }

    IEnumerator DestroyObject(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }

    IEnumerator DestroyImage(Image obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }
}
