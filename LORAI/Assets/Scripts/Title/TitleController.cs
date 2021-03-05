﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
	public Fader fader;
	public Animator animator;
	public Sound soundController;
	public NewGameScreen newGameScreen;
	public TitleText titleText;
	public GameObject donateButton;
	public VolumeProfile volume;

	private int m_OpenParameterId;
	private int expID;

	void Start()
	{
		System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
		System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
		System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

		fader.UnFade( 2 );
		DataStore.InitData();
		if ( !PlayerPrefs.HasKey( "music" ) )
			PlayerPrefs.SetInt( "music", 1 );
		if ( !PlayerPrefs.HasKey( "sound" ) )
			PlayerPrefs.SetInt( "sound", 1 );
		if ( !PlayerPrefs.HasKey( "bloom" ) )
			PlayerPrefs.SetInt( "bloom", 1 );
		PlayerPrefs.Save();

		if ( volume.TryGet<Bloom>( out var bloom ) )
			bloom.active = PlayerPrefs.GetInt( "bloom" ) == 1;

		FindObjectOfType<Sound>().CheckMusic();
	}

	private void OnEnable()
	{
		m_OpenParameterId = Animator.StringToHash( "flip in" );
		expID = Animator.StringToHash( "exp flip in" );
		animator.SetBool( m_OpenParameterId, true );

		titleText.Show();
	}

	public void FlipIn( Animator anim )
	{
		anim.SetBool( m_OpenParameterId, true );
	}

	public void ReturnTo()
	{
		EventSystem.current.SetSelectedGameObject( null );
		FlipIn( animator );
		titleText.Show();
		titleText.FlipIn();
		donateButton.SetActive( true );
	}

	public void OnNewGame()
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );
		animator.SetBool( m_OpenParameterId, false );
		animator.SetBool( expID, false );

		titleText.FlipOut();

		donateButton.SetActive( false );

		DataStore.StartNewSession();
		newGameScreen.ActivateScreen();
	}

	//public void OnLoadGame()
	//{
	//	soundController.PlaySound( FX.Click );

	//}

	public void OnExpansions()
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );
		if ( animator.GetBool( expID ) == true )
			animator.SetBool( expID, false );
		else
		{
			animator.SetBool( expID, true );
			FindObjectOfType<ExpansionsPanel>().ActivatePanel();
		}
	}

	public void OnOptions()
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );
		GlowEngine.FindObjectsOfTypeSingle<SettingsScreen>().Show( OnSettingsClose, true );
	}

	void OnSettingsClose( SettingsCommand s )
	{
		Application.Quit();
	}

	public void OnCloseExpansions()
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );
		animator.SetBool( expID, false );
	}

	public void ToggleExpansion( Toggle t )
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );
		if ( t.isOn )
			DataStore.AddExpansion( t.name );
		else
			DataStore.RemoveExpansions( t.name );
	}

	public void FadeOut( float time )
	{
		fader.FadeToBlack( time );
	}

	public void OnDonate()
	{
		Application.OpenURL( "https://paypal.me/glowpuff" );
	}
}
