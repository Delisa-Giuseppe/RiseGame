using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelmanager : MonoBehaviour {

	public bool isNeeded = true;
	public float Timer;
	public int sceneIndex;

	
	public void LoadLevel (string name){
		Debug.Log("Level load request for:"+name);
		SceneManager.LoadScene(name);
	}

	public void LoadByIndex (int sceneIndex){
		Debug.Log("Level load request for:"+sceneIndex);
		SceneManager.LoadScene (sceneIndex);
	}

	public void LoadAfterTimer(){
		if (isNeeded == true) {
			Invoke ("LoadScene", Timer);
			Debug.Log ("The timer has started");
		} else {
			Debug.Log ("You need to active the timer");
		}
	}

	public void LoadScene(){
		Debug.Log("Loading scene " + sceneIndex);
		SceneManager.LoadScene (sceneIndex);
	}

	public void Quit(){
//#if UNITY_EDITOR
	//UnityEditor.EditorApplication.isPlaying = false;
//#else
	//Application.Quit();
//#endif 
		Application.Quit() ; 
		Debug.Log("Quit request");

	}

}
