using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
	public Fader fader;
	public Animator animator;
	public Sound soundController;
	public NewGameScreen newGameScreen;
	public TitleText titleText;
	public GameObject donateButton, versionInfo;
	public VolumeProfile volume;
	public Button continueButton;
	public Transform busyIconTF;
	public TextMeshProUGUI versionText;

	private int m_OpenParameterId;
	private int expID;
	private NetworkStatus networkStatus;

	void Start()
	{
		System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
		System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
		System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

		Screen.fullScreen = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		fader.UnFade( 2 );
		DataStore.InitData();
		if ( !PlayerPrefs.HasKey( "music" ) )
			PlayerPrefs.SetInt( "music", 1 );
		if ( !PlayerPrefs.HasKey( "sound" ) )
			PlayerPrefs.SetInt( "sound", 1 );
		if ( !PlayerPrefs.HasKey( "bloom" ) )
			PlayerPrefs.SetInt( "bloom", 1 );
		if ( !PlayerPrefs.HasKey( "vignette" ) )
			PlayerPrefs.SetInt( "vignette", 1 );
		PlayerPrefs.Save();

		if ( volume.TryGet<Bloom>( out var bloom ) )
			bloom.active = PlayerPrefs.GetInt( "bloom" ) == 1;
		if ( volume.TryGet<Vignette>( out var vig ) )
			vig.active = PlayerPrefs.GetInt( "vignette" ) == 1;

		//check if saved state is valid
		continueButton.interactable = IsSessionValid();

		FindObjectOfType<Sound>().CheckAudio();

		networkStatus = NetworkStatus.Busy;
		versionText.text = "Version " + DataStore.appVersion;

		if ( NetworkInterface.GetIsNetworkAvailable() )
			StartCoroutine( StartVersionCheck() );
		else
		{
			networkStatus = NetworkStatus.Error;
			busyIconTF.GetComponent<Image>().color = new Color( 1, 0, 0 );
		}
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
		versionInfo.SetActive( true );
	}

	public void OnNewGame()
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );
		animator.SetBool( m_OpenParameterId, false );
		animator.SetBool( expID, false );

		titleText.FlipOut();

		donateButton.SetActive( false );
		versionInfo.SetActive( false );

		DataStore.StartNewSession();
		newGameScreen.ActivateScreen();
	}

	public void OnContinueSession()
	{
		EventSystem.current.SetSelectedGameObject( null );
		soundController.PlaySound( FX.Click );

		SessionData session = LoadSession();
		if ( session != null )
		{
			DataStore.sessionData = session;

			animator.SetBool( m_OpenParameterId, false );
			animator.SetBool( expID, false );
			titleText.FlipOut();
			donateButton.SetActive( false );
			versionInfo.SetActive( false );
			soundController.FadeOutMusic();
			FadeOut( 1 );

			float foo = 1;
			DOTween.To( () => foo, x => foo = x, 0, 1 ).OnComplete( () =>
			 SceneManager.LoadScene( "Main" ) );
		}
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

	private bool IsSessionValid()
	{
		string basePath = Path.Combine( Application.persistentDataPath, "Session", "sessiondata.json" );

		if ( !File.Exists( basePath ) )
			return false;

		string json = "";
		try
		{
			using ( StreamReader sr = new StreamReader( basePath ) )
			{
				json = sr.ReadToEnd();
			}
			SessionData session = JsonConvert.DeserializeObject<SessionData>( json );

			return session.stateManagementVersion == 1;
		}
		catch ( Exception e )
		{
			Debug.Log( "***ERROR*** IsSessionValid:: " + e.Message );
			File.WriteAllText( Path.Combine( Application.persistentDataPath, "Session", "error_log.txt" ), "TRACE:\r\n" + e.Message );
			return false;
		}
	}

	private SessionData LoadSession()
	{
		string basePath = Path.Combine( Application.persistentDataPath, "Session", "sessiondata.json" );

		string json = "";

		try
		{
			using ( StreamReader sr = new StreamReader( basePath ) )
			{
				json = sr.ReadToEnd();
			}
			SessionData session = JsonConvert.DeserializeObject<SessionData>( json );

			return session;
		}
		catch ( Exception e )
		{
			Debug.Log( "***ERROR*** LoadSession:: " + e.Message );
			File.WriteAllText( Path.Combine( Application.persistentDataPath, "Session", "error_log.txt" ), "TRACE:\r\n" + e.Message );
			return null;
		}
	}

	private void Update()
	{
		if ( networkStatus == NetworkStatus.Busy )
			busyIconTF.Rotate( new Vector3( 0, 0, Time.deltaTime * 175f ) );

		//pulse scale if network error or wrong version
		if ( networkStatus == NetworkStatus.Error || networkStatus == NetworkStatus.WrongVersion )
			busyIconTF.localScale = GlowEngine.SineAnimation( .9f, 1.1f, 15 ).ToVector3();
	}

	private IEnumerator CheckVersion()
	{
		// /repos/{owner}/{repo}/releases
		var web = UnityWebRequest.Get( "https://api.github.com/repos/GlowPuff/ImperialCommander/releases" );
		yield return web.SendWebRequest();
		if ( web.isNetworkError )
		{
			Debug.Log( "network error" );
			networkStatus = NetworkStatus.Error;
			busyIconTF.GetComponent<Image>().color = new Color( 1, 0, 0 );
		}
		else
		{
			//parse JSON response
			var version = JsonConvert.DeserializeObject<List<GitHubResponse>>( web.downloadHandler.text );
			if ( version[0].tag_name == DataStore.appVersion )
			{
				networkStatus = NetworkStatus.UpToDate;
				busyIconTF.GetComponent<Image>().color = new Color( 0, 1, 0 );
			}
			else
			{
				networkStatus = NetworkStatus.WrongVersion;
				busyIconTF.GetComponent<Image>().color = new Color( 1, 0.5586207f, 0 );
			}
		}

		yield return null;
	}

	private IEnumerator StartVersionCheck()
	{
		//first check if internet is available
		var ping = new System.Net.NetworkInformation.Ping();
		var reply = ping.Send( new IPAddress( new byte[] { 8, 8, 8, 8 } ), 5000 );
		if ( reply.Status == IPStatus.Success )
		{
			//internet available, check for latest version
			StartCoroutine( CheckVersion() );
		}
		else
		{
			networkStatus = NetworkStatus.Error;
			busyIconTF.GetComponent<Image>().color = new Color( 1, 0, 0 );
		}

		yield return null;
	}
}
