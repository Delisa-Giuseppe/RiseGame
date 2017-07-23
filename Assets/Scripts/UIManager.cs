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
