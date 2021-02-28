using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsScreen : MonoBehaviour
{
	public CanvasGroup cg;
	public Image fader;
	public Toggle musicToggle, soundToggle;
	public Sound sound;
	public void Show()
	{
		gameObject.SetActive( true );
		fader.color = new Color( 0, 0, 0, 0 );
		fader.DOFade( .95f, .5f );
		cg.DOFade( 1, .5f );
		transform.GetChild( 0 ).localScale = new Vector3( .85f, .85f, .85f );
		transform.GetChild( 0 ).DOScale( 1, .5f ).SetEase( Ease.OutExpo );

		musicToggle.isOn = PlayerPrefs.GetInt( "music" ) == 1;
		soundToggle.isOn = PlayerPrefs.GetInt( "sound" ) == 1;
	}

	public void OnOK()
	{
		PlayerPrefs.SetInt( "music", musicToggle.isOn ? 1 : 0 );
		PlayerPrefs.SetInt( "sound", soundToggle.isOn ? 1 : 0 );
		PlayerPrefs.Save();

		FindObjectOfType<Sound>().PlaySound( FX.Click );

		fader.DOFade( 0, .5f ).OnComplete( () =>
		{
			gameObject.SetActive( false );
		} );
		cg.DOFade( 0, .2f );
		transform.GetChild( 0 ).DOScale( .85f, .5f ).SetEase( Ease.OutExpo );
	}

	public void OnToggle( Toggle t )
	{
		if ( t.name == "music Toggle" )
		{
			if ( t.isOn )
				sound.PlayMusic();
			else
				sound.StopMusic();
		}
	}
}
