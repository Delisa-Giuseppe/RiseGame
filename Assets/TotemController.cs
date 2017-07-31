using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TotemController : MonoBehaviour {

    public GameObject cutscene;

    private Animator anim;
    private string cutsceneName;

	// Use this for initialization
	void Awake () {
        cutsceneName = gameObject.name.Split('_')[1];
        anim = cutscene.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void OnCutscene () {
        anim.SetBool(cutsceneName, true);
        anim.SetBool("showCutscene", true);
        StartCoroutine(ShowCutscene());
	}

    IEnumerator ShowCutscene()
    {
        yield return new WaitForSeconds(1f);
        float waitTime = 0;
        AnimationClip[] animations = AnimationUtility.GetAnimationClips(anim.gameObject);
        foreach(AnimationClip clip in animations)
        {
            if(clip.name.Contains(cutsceneName))
            {
                waitTime = clip.length;
            }
        }
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("showCutscene", false);
        anim.SetBool(cutsceneName, false);
        GameManager.currentState = GameManager.States.EXPLORATION;
    }
}
