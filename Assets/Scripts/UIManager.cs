using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

    public Text popupDamage;
    public Text changeTurnText;
    public Image fightImage;
    public List<Image> playersImage;
    public List<TextMeshProUGUI> healthTexts;
    public List<Sprite> playersIcon;
    public List<GameObject> playersTurns;
    public GameObject enemyHealth;

    private List<GameObject> healthEnemies;


    private void Awake()
    {
        StartCoroutine(SetPlayersImage());
    }

    private void Update()
    {
        SetTextHealth();
    }

    public void ShowPopupDamage(int damage, Transform location)
    {
        Text popup = Instantiate(popupDamage);
        popup.transform.SetParent(transform, false);
        popup.transform.position = location.position;
        popup.text = damage.ToString();
        StartCoroutine(DestroyText(popup, 1));
    }

    public void SetChangeTurnText(string text)
    {
        Text turnText = Instantiate(changeTurnText);
        turnText.transform.SetParent(transform, false);
        turnText.text = text;
        StartCoroutine(DestroyText(turnText, 1f));
    }

    public void ShowImageFight()
    {
        Image img = Instantiate(fightImage);
        img.transform.SetParent(transform, false);
        StartCoroutine(DestroyImage(img, 2.5f));
    }

    public void SetTextHealth()
    {
        for(int i=0; i<TileManager.playerInstance.Count;i++)
        {
            if (TileManager.playerInstance[i])
            {
                healthTexts[i].SetText(TileManager.playerInstance[i].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
            }
        }
    }

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
        for(int i=0; i<TileManager.playerInstance.Count; i++)
        {
            if(TileManager.playerInstance[i].GetComponent<PlayerController>().playerNumber == playerNumber)
            {
                healthTexts[i].SetText("0/" + TileManager.playerInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
                playersImage[i].GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                healthTexts.RemoveAt(i);
                playersImage.RemoveAt(i);
            }
        }
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

    IEnumerator SetPlayersImage()
    {
        while(TileManager.playerInstance == null || TileManager.playerInstance.Count == 0)
        {
            yield return null;
        }

        for(int i=0; i<TileManager.playerInstance.Count;i++)
        {
            string[] playerName = TileManager.playerInstance[i].name.Split('(');
            foreach(Sprite image in playersIcon)
            {
                if(image.name == "Icon_"+playerName[0])
                {
                    playersImage[i].sprite = image;
                }
            }
        }
    }

    IEnumerator DestroyText(Text obj, float delay)
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
