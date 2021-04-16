using System.IO;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameScreen : MonoBehaviour
{
	public MWheelHandler threatWheelHandler;
	public MWheelHandler addtlThreatWheelHandler;
	public Text difficultyText, deploymentText, selectedMissionText, threatCostText, defaultsText;
	public Toggle imperialToggle, mercenaryToggle, adaptiveToggle;
	public TitleController titleController;
	public CanvasGroup cg;
	public CardZoomer cardZoomer;
	public GroupChooserScreen groupChooser;
	public GameObject addAllyButton;
	public Button addHeroButton, startMissionButton, difficultyButton, loadDefaultsButton;
	public Image allyImage;
	public Text[] enemyGroupText;
	public HeroMeta[] heroMetas;
	public MissionTextBox missionTextBox;
	public HeroChooser heroChooser;
	public GridLayoutGroup gridLayoutGroup;

	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();

		//debug only
#if DEBUG
		//DataStore.StartNewSession();
#endif
	}

	public void ActivateScreen()
	{
		gameObject.SetActive( true );
		cg.alpha = 0;
		cg.DOFade( 1, .5f );

		//reset UI
		addHeroButton.interactable = true;
		selectedMissionText.transform.Find( "view Button" ).GetComponent<Button>().interactable = false;
		selectedMissionText.transform.Find( "mission info button" ).GetComponent<Button>().interactable = false;
		selectedMissionText.text = "Choose Mission";
		difficultyText.text = "difficulty";
		deploymentText.text = "no";
		imperialToggle.isOn = true;
		mercenaryToggle.isOn = true;
		adaptiveToggle.isOn = false;
		threatWheelHandler.ResetWheeler();
		addtlThreatWheelHandler.ResetWheeler();
		for ( int i = 0; i < enemyGroupText.Length; i++ )
			enemyGroupText[i].text = "choose";
		enemyGroupText[3].text = "8 selected";
		//button colors to red
		ColorBlock cb = difficultyButton.colors;
		cb.normalColor = new Color( 1, 0.1568628f, 0, 1 );
		difficultyButton.colors = cb;
		cb = addHeroButton.colors;
		cb.normalColor = new Color( 1, 0.1568628f, 0, 1 );
		addHeroButton.colors = cb;
		OnRemoveAlly();

		//check for default state
		string path = Path.Combine( Application.persistentDataPath, "Defaults", "sessiondata.json" );
		loadDefaultsButton.interactable = File.Exists( path );

		//aspect ratio adjustment
		if ( GlowEngine.GetAspectRatio() >= 1.6f )//16:9 or greater
			gridLayoutGroup.cellSize = new Vector2( 413, 400 );
		else if ( GlowEngine.GetAspectRatio() >= 1.33f )//4:3, 5:4
			gridLayoutGroup.cellSize = new Vector2( 313, 400 );
	}

	public void OnDifficulty()
	{
		sound.PlaySound( FX.Click );
		difficultyText.text = DataStore.sessionData.ToggleDifficulty();
		ColorBlock cb = difficultyButton.colors;
		cb.normalColor = new Color( 0, 0.6440244f, 1, 1 );
		difficultyButton.colors = cb;
	}

	public void OnOptionalDeployment()
	{
		sound.PlaySound( FX.Click );
		deploymentText.text = DataStore.sessionData.ToggleDeployment();
	}

	public void OnImperials()
	{
		sound.PlaySound( FX.Click );
		DataStore.sessionData.ToggleImperials( imperialToggle.isOn );
	}

	public void OnMercenaries()
	{
		sound.PlaySound( FX.Click );
		DataStore.sessionData.ToggleMercs( mercenaryToggle.isOn );
	}

	public void OnAdaptiveDifficulty()
	{
		DataStore.sessionData.useAdaptiveDifficulty = adaptiveToggle.isOn;
	}

	public void OnThreatCost()
	{
		sound.PlaySound( FX.Click );
		threatCostText.text = DataStore.sessionData.ToggleThreatCost();
	}

	public void OnViewMissionCard()
	{
		EventSystem.current.SetSelectedGameObject( null );
		sound.PlaySound( FX.Click );

		Sprite sprite = Resources.Load<Sprite>( $"Cards/Missions/{DataStore.sessionData.selectedMissionExpansion}/{DataStore.sessionData.selectedMissionID}" );
		cardZoomer.ZoomIn( sprite );
	}

	public void OnBack()
	{
		sound.PlaySound( FX.Click );

		//clear hero bar
		for ( int i = 0; i < 4; i++ )
			heroMetas[i].gameObject.SetActive( false );


		cg.DOFade( 0, .5f ).OnComplete( () =>
		{
			gameObject.SetActive( false );
			titleController.ReturnTo();
		} );
	}

	public void OnChooseMission()
	{
		sound.PlaySound( FX.Click );
		//ColorBlock cb = chooseMissionButton.colors;
		//cb.normalColor = new Color( 0, 0.6440244f, 1, 1 );
		//chooseMissionButton.colors = cb;
		cg.DOFade( 0, .5f );
		groupChooser.ActivateScreen( ChooserMode.Missions );
	}

	public void OnMisionInfo()
	{
		EventSystem.current.SetSelectedGameObject( null );
		sound.PlaySound( FX.Click );
		var txt = Resources.Load<TextAsset>( $"MissionText/{DataStore.sessionData.selectedMissionID}info" );
		if ( txt != null )
			missionTextBox.Show( txt.text );
	}

	public void OnChooseEnemyGroups( int btnIndex )
	{
		EventSystem.current.SetSelectedGameObject( null );
		//0=starting, 1=reserved, 2=villains, 3=ignored, 4=heroes
		sound.PlaySound( FX.Click );
		cg.DOFade( 0, .5f );
		groupChooser.ActivateScreen( ChooserMode.DeploymentGroups, btnIndex );
	}

	public void OnAddHero()
	{
		EventSystem.current.SetSelectedGameObject( null );
		sound.PlaySound( FX.Click );
		cg.DOFade( 0, .5f );
		heroChooser.Show();
	}

	public void OnRemoveHero( int index )
	{
		EventSystem.current.SetSelectedGameObject( null );
		DataStore.sessionData.ToggleHero( heroMetas[index].id );
		heroMetas[index].gameObject.SetActive( false );
		addHeroButton.interactable = true;
	}

	public void OnAddAlly()
	{
		EventSystem.current.SetSelectedGameObject( null );
		sound.PlaySound( FX.Click );
		cg.DOFade( 0, .5f );
		groupChooser.ActivateScreen( ChooserMode.Ally, 4 );
	}

	public void OnRemoveAlly()
	{
		EventSystem.current.SetSelectedGameObject( null );
		addAllyButton.SetActive( true );
		allyImage.gameObject.SetActive( false );
		DataStore.sessionData.selectedAlly = null;
	}

	public void OnReturnTo()
	{
		EventSystem.current.SetSelectedGameObject( null );
		cg.DOFade( 1, .5f );

		//handle selected mission
		selectedMissionText.text = DataStore.sessionData.selectedMissionName.ToLower();
		selectedMissionText.transform.Find( "view Button" ).GetComponent<Button>().interactable = true;
		selectedMissionText.transform.Find( "mission info button" ).GetComponent<Button>().interactable = true;

		//handle selected enemy groups
		for ( int i = 0; i < 4; i++ )
		{
			//only the first 4 DeploymentCards
			//index 4 contains heroes
			DeploymentCards selectedCards = DataStore.sessionData.selectedDeploymentCards[i];
			if ( selectedCards.cards.Count > 0 )
				enemyGroupText[i].text = selectedCards.cards.Count + " selected";
			else
				enemyGroupText[i].text = "choose";
		}

		//handle selected heroes
		for ( int i = 0; i < 4; i++ )
			heroMetas[i].gameObject.SetActive( false );
		addHeroButton.interactable = DataStore.sessionData.MissionHeroes.Count < 4;

		int idx = 0;
		foreach ( CardDescriptor dc in DataStore.sessionData.MissionHeroes )
		{
			//add thumbnail
			heroMetas[idx].gameObject.SetActive( true );
			heroMetas[idx].allyName = dc.name;
			heroMetas[idx].id = dc.id;
			heroMetas[idx].allySprite.sprite = Resources.Load<Sprite>( $"Cards/Heroes/{dc.id}" );
			idx++;
		}
		ColorBlock cb = addHeroButton.colors;
		if ( DataStore.sessionData.MissionHeroes.Count > 0 )
			cb.normalColor = new Color( 0, 0.6440244f, 1, 1 );
		else
			cb.normalColor = new Color( 1, 0.1568628f, 0, 1 );
		addHeroButton.colors = cb;

		//handle selected ally
		if ( DataStore.sessionData.selectedAlly != null )
		{
			addAllyButton.SetActive( false );
			allyImage.gameObject.SetActive( true );
			allyImage.sprite = Resources.Load<Sprite>( $"Cards/Allies/{DataStore.sessionData.selectedAlly.id.Replace( "A", "M" )}" );
		}
		else
		{
			addAllyButton.SetActive( true );
			allyImage.gameObject.SetActive( false );
		}
	}

	public void OnStartNewGame()
	{
		EventSystem.current.SetSelectedGameObject( null );
		sound.PlaySound( FX.Click );
		startMissionButton.interactable = false;

		//set threat levels into the state
		DataStore.sessionData.threatLevel = threatWheelHandler.wheelValue;
		DataStore.sessionData.addtlThreat = addtlThreatWheelHandler.wheelValue;

		sound.FadeOutMusic();
		titleController.FadeOut( 1 );

		cg.DOFade( 0, 1 ).OnComplete( () =>
		{
			SceneManager.LoadScene( "Main" );
		} );
	}

	public void SaveDefaults()
	{
		DataStore.sessionData.threatLevel = threatWheelHandler.wheelValue;
		DataStore.sessionData.addtlThreat = addtlThreatWheelHandler.wheelValue;

		defaultsText.color = new Color( 0, 1, 0.628047f, 1 );
		if ( DataStore.sessionData.SaveDefaults() )
		{
			defaultsText.text = "saved";
		}
		else
		{
			defaultsText.color = new Color( 1, 0, 0, 1 );
			defaultsText.text = "error";
		}

		string path = Path.Combine( Application.persistentDataPath, "Defaults", "sessiondata.json" );
		loadDefaultsButton.interactable = File.Exists( path );

		defaultsText.DOFade( 0, 2 ).SetDelay( 1 );
	}

	public void LoadDefaults()
	{
		defaultsText.color = new Color( 0, 1, 0.628047f, 1 );

		string basePath = Path.Combine( Application.persistentDataPath, "Defaults", "sessiondata.json" );

		string json = "";
		try
		{
			using ( StreamReader sr = new StreamReader( basePath ) )
			{
				json = sr.ReadToEnd();
			}
			SessionData session = JsonConvert.DeserializeObject<SessionData>( json );

			DataStore.sessionData = session;

			//populate UI
			if ( DataStore.sessionData.difficulty != Difficulty.NotSet )
				difficultyText.text = DataStore.sessionData.difficulty.ToString().ToLower();
			else
				difficultyText.text = "difficulty";
			threatCostText.text = DataStore.sessionData.allyThreatCost.ToString().ToLower();
			deploymentText.text = DataStore.sessionData.optionalDeployment.ToString().ToLower();
			mercenaryToggle.isOn = DataStore.sessionData.includeMercs;
			imperialToggle.isOn = DataStore.sessionData.includeImperials;
			adaptiveToggle.isOn = DataStore.sessionData.useAdaptiveDifficulty;
			threatWheelHandler.ResetWheeler( DataStore.sessionData.threatLevel );
			addtlThreatWheelHandler.ResetWheeler( DataStore.sessionData.addtlThreat );
			//heroes, ally, groups button text, mission
			OnReturnTo();

			if ( DataStore.sessionData.difficulty != Difficulty.NotSet )
			{
				ColorBlock cb = difficultyButton.colors;
				cb.normalColor = new Color( 0, 0.6440244f, 1, 1 );
				difficultyButton.colors = cb;
			}

			defaultsText.text = "loaded";
		}
		catch ( System.Exception e )
		{
			Debug.Log( "***ERROR*** LoadDefaults:: " + e.Message );
			File.WriteAllText( Path.Combine( Application.persistentDataPath, "Defaults", "error_log.txt" ), "TRACE:\r\n" + e.Message );
			defaultsText.color = new Color( 1, 0, 0, 1 );
			defaultsText.text = "error";
		}

		defaultsText.DOFade( 0, 2 ).SetDelay( 1 );
	}

	private void Update()
	{
		//check if mission can be started
		bool heroCheck = DataStore.sessionData.selectedDeploymentCards[4].cards.Count > 0;
		bool difficulty = DataStore.sessionData.difficulty != Difficulty.NotSet;
		bool allyRules = DataStore.sessionData.allyRules != AllyRules.NotSet;
		bool factions = DataStore.sessionData.includeImperials || DataStore.sessionData.includeMercs;

		if ( heroCheck && difficulty && allyRules && factions )
			startMissionButton.interactable = true;
		else
			startMissionButton.interactable = false;
	}
}
