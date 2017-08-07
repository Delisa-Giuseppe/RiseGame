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
    private AudioSource currentMusic;

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

        music.loop=true;
        StartCoroutine(FadeIn(music, 1f));
    }

    private void Update()
    {
        if(GameManager.currentState == GameManager.States.ENGAGE_ENEMY)
        {
            currentMusic = music;
            StartCoroutine(FadeOutIn(music, 0.5f, enemyFight));
        }
        else
        {
            music = currentMusic;
            StartCoroutine(FadeOutIn(music, 0.5f, enemyFight));
        }
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {

        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }

    public IEnumerator FadeOutIn(AudioSource audioSource, float FadeTime, AudioClip newClip)
    {
        float startVolume = 1f;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 0f;

        music.clip = newClip;

        StartCoroutine(FadeIn(music, 0.5f));
    }

}


