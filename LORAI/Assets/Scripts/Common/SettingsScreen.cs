using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SettingsScreen : MonoBehaviour
{
	public CanvasGroup cg;
	public Image fader;
	public Toggle musicToggle, soundToggle;
	public Sound sound;
	public GameObject returnButton;

	Action<SettingsCommand> closeAction;

	public void Show( Action<SettingsCommand> a, bool fromTitle = false )
	{
		closeAction = a;
		//remove return to title button
		returnButton.SetActive( !fromTitle );

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
		sound.PlaySound( FX.Click );
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
		sound.PlaySound( FX.Click );
		if ( t.name == "music Toggle" )
		{
			if ( t.isOn )
				sound.PlayMusic();
			else
				sound.StopMusic();
		}
	}

	public void OnQuit()
	{
		sound.PlaySound( FX.Click );
		closeAction?.Invoke( SettingsCommand.Quit );
	}

	public void OnReturnTitles()
	{
		sound.PlaySound( FX.Click );
		fader.DOFade( 0, .5f ).OnComplete( () =>
		{
			gameObject.SetActive( false );
			closeAction?.Invoke( SettingsCommand.ReturnTitles );
		} );
		cg.DOFade( 0, .2f );
		transform.GetChild( 0 ).DOScale( .85f, .5f ).SetEase( Ease.OutExpo );
	}
}
