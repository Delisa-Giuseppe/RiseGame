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
    public List<Sprite> enemiesIcon;
    public GameObject enemyContainer;
    public GameObject enemyUI;

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
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-5f, 5f), location.position.y + Random.Range(-10f, 10f)));
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
        StartCoroutine(DestroyText(turnText, 1.5f));
    }

    public void ShowImageFight()
    {
        Image img = Instantiate(fightImage);
        img.transform.SetParent(transform, false);
        StartCoroutine(DestroyImage(img, 2.5f));
    }

    public void SetTextHealth()
    {
        healthTexts[0].SetText("HP " + TileManager.playerInstance[0].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[0].GetComponent<ObjectController>().totalHealth.ToString());
        healthTexts[1].SetText("HP " + TileManager.playerInstance[1].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[1].GetComponent<ObjectController>().totalHealth.ToString());
        healthTexts[2].SetText("HP " + TileManager.playerInstance[2].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[2].GetComponent<ObjectController>().totalHealth.ToString());
        healthTexts[3].SetText("HP " + TileManager.playerInstance[3].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[3].GetComponent<ObjectController>().totalHealth.ToString());
    }

    public void CreateEnemyUI(List<GameObject> enemyInstance)
    {
        healthEnemies = new List<GameObject>();
        for (int i=0;i<enemyInstance.Count;i++)
        {
            GameObject enemyUIInstance = Instantiate(enemyUI);
            enemyUIInstance.transform.SetParent(enemyContainer.transform, false);
            Vector3 position = new Vector3(enemyUI.transform.position.x, enemyUI.transform.position.y - (100 * i));
            enemyUIInstance.transform.localPosition = position;

            for (int p = 0; p < enemyUIInstance.transform.childCount; p++)
            {
                if (enemyUIInstance.transform.GetChild(p).name == "HealthText")
                {
                    enemyUIInstance.transform.GetChild(p).GetComponent<TextMeshProUGUI>().SetText("HP " + enemyInstance[i].GetComponent<ObjectController>().currentHealth.ToString() + "/" + enemyInstance[i].GetComponent<ObjectController>().totalHealth.ToString());
                    break;
                }
            }

            healthEnemies.Add(enemyUIInstance);
        }
        enemyContainer.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        enemyContainer.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        enemyContainer.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
    }

    public void DestroyEnemyUI(int enemyPosition)
    {
        Destroy(healthEnemies[enemyPosition]);
        healthEnemies[enemyPosition] = null;
        //healthEnemies.RemoveAt(enemyPosition);
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
                healthEnemies[enemyNumber].transform.GetChild(p).GetComponent<TextMeshProUGUI>().SetText("HP " + target.GetComponent<ObjectController>().currentHealth.ToString() + "/" + target.GetComponent<ObjectController>().totalHealth.ToString());
                break;
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
