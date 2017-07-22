using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text popupDamage;
    public Text changeTurnText;
    public Image fightImage;

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
