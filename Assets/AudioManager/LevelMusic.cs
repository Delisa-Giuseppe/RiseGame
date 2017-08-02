using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelMusic : MonoBehaviour {

	[Header("Muische scene")]

	[Space]

	[Tooltip ("Queste musiche vanno alternate nelle varie scene di gioco")]
	public AudioClip menu;
	public AudioClip forest;
	public AudioClip desert;
	public AudioClip iceLand;
	public AudioClip swamp;
	public AudioClip castel;
	public AudioClip enemyFight;
	public AudioClip nemesi;
	public AudioClip bossFight;
	public AudioClip credits;
	public AudioClip levelUp;
	public AudioClip gameOver;

	static LevelMusic instance=null;
	private AudioSource music;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {

			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
			music = GetComponent<AudioSource> ();
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
}
	   

	// Use this for initialization
	void Start ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
			print ("AudioManager extra distrutto");
		} else {
			instance = this; 
			GameObject.DontDestroyOnLoad (gameObject);
			music = GetComponent<AudioSource> ();
			music.clip = menu;
			music.loop = true;
			music.Play ();
		}
	}
	// Aggiungere il nome o l'index della scena
	void OnSceneLoaded (Scene scene, LoadSceneMode loadscenemode){
    //music.Stop ();
    if (scene.buildIndex == 1) {
        music.clip = menu;
    }
    if (scene.buildIndex == 2) {
        music.clip = credits;
    }
    if (scene.buildIndex == 3) {
        music.clip = forest;
    }
	if (scene.buildIndex == 4) {
			music.clip = desert;
	}
	if (scene.buildIndex == 5) {
			music.clip = iceLand;
	}
	if (scene.buildIndex == 6) {
			music.clip = swamp;
	}
	if (scene.buildIndex == 7) {
			music.clip = castel;
	}
	if (scene.buildIndex == 8) {
			music.clip = levelUp;
	}
	if (scene.buildIndex == 0) {
			music.clip = credits;
	}
    music.loop=true;
    music.Play();
	}
}
