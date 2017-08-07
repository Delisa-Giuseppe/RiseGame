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
	
	public void PlayHitMelee()
    {
        if (!music.isPlaying)
        {
            music.PlayOneShot(hit1, 0.8f);
        }
    }

    public void PlayHitRanged()
    {
        if (!music.isPlaying)
        {
            music.PlayOneShot(hit2, 0.8f);
        }
    }

    public void PlayHealing()
    {
        if (!music.isPlaying)
        {
            music.PlayOneShot(healing, 0.8f);
        }
    }

    public void PlayWarp()
    {
        if (!music.isPlaying)
        {
            music.PlayOneShot(warp, 0.8f);
        }
    }

    public void PlaySpell()
    {
        if (!music.isPlaying)
        {
            music.PlayOneShot(spell, 0.8f);
        }
    }

    public void PlayFootstep()
    {
        if(!music.isPlaying)
        {
            music.PlayOneShot(footstep, 0.8f);
        }
    }

    public void StopMusic()
    {
        music.Stop();
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
