using System.Collections;
using UnityEngine;

public class TotemController : MonoBehaviour {

    public GameObject cutscene;

    private Animator anim;
    private string cutsceneName;

	// Use this for initialization
	void Awake () {
        cutsceneName = gameObject.name.Split('_')[1];
        anim = cutscene.GetComponent<Animator>();
        GetComponent<Animator>().SetBool("showFlash", true);
	}
	
	// Update is called once per frame
	public void OnCutscene () {
        anim.SetBool(cutsceneName, true);
        anim.SetBool("showCutscene", true);
        StartCoroutine(ShowCutscene());
	}

    IEnumerator ShowCutscene()
    {
        GetComponent<Animator>().SetBool("showFlash", false);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(11);
        anim.SetBool("showCutscene", false);
        anim.SetBool(cutsceneName, false);
        GameManager.currentState = GameManager.States.EXPLORATION;
    }
}
