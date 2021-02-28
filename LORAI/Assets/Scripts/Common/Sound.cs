using UnityEngine;
using DG.Tweening;

public class Sound : MonoBehaviour
{
	public AudioSource source;
	public AudioSource musicSource;
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
	public void CheckMusic()
	{
		musicSource.volume = maxMusicVolume;
		if ( PlayerPrefs.GetInt( "music" ) == 0 )
			musicSource.Stop();
	}

	public void PlayMusic()
	{
		musicSource.Play();
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void FadeOutMusic()
	{
		musicSource.DOFade( 0, 1 );
	}
}
