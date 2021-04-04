using UnityEngine;
using DG.Tweening;

public class Sound : MonoBehaviour
{
	public AudioSource source;
	public AudioSource musicSource;
	public AudioSource ambientSource;
	public AudioClip[] clips;
	public float maxMusicVolume = .5f;

	public void PlaySound( FX sound )
	{
		if ( PlayerPrefs.GetInt( "sound" ) == 1 )
			source.PlayOneShot( clips[(int)sound] );
	}

	/// <summary>
	/// call on screen Start() to check and play/not play music
	/// </summary>
	public void CheckAudio()
	{
		musicSource.volume = maxMusicVolume;
		if ( PlayerPrefs.GetInt( "music" ) == 0 )
			musicSource.Stop();
		if ( PlayerPrefs.GetInt( "sound" ) == 0 )
			ambientSource.Stop();
	}

	public void PlayMusic()
	{
		musicSource.Play();
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void StartAmbientSound()
	{
		ambientSource.Play();
	}

	public void StopAmbientSound()
	{
		ambientSource.Stop();
	}

	public void FadeOutMusic()
	{
		musicSource.DOFade( 0, 1 );
	}
}
