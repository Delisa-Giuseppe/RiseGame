using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactersSFX : MonoBehaviour {
	[Header("Effetti Sonori dei personaggi")]
	[Space]
	[Tooltip ("Questi audio possono essere usate nelle battaglie per i personaggi e per i nemici")]
	public AudioClip hit1;
	public AudioClip hit2;
	public AudioClip spell;
	public AudioClip warp;
	public AudioClip healing;
	public AudioClip death;
    public AudioClip footstep;

    private AudioSource music;
    // Use this for initialization
    void Awake () {
        music = GetComponent<AudioSource>();
	}
	
	void Update () {
        music.clip = footstep;
        StartCoroutine(FadeIn(music, 1f));
	}

    public void PlayFootstep()
    {
        music.clip = footstep;
        music.Play();
        //StartCoroutine(FadeIn(music, 1f));
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
