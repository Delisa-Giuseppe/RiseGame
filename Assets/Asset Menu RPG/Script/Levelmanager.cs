using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelmanager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void LoadLevel (string name){
		Debug.Log("Level load request for:"+name);
		SceneManager.LoadScene(name);
	}

	public void LoadByIndex (int sceneIndex){
		Debug.Log("Level load request for:"+sceneIndex);
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
