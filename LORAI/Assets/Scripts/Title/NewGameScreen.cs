using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class NewGameScreen : MonoBehaviour
{
	public MWheelHandler threatWheelHandler;
	public MWheelHandler addtlThreatWheelHandler;
	public Text difficultyText, rulesText, deploymentText, selectedMissionText, threatCostText;
	public Toggle imperialToggle, mercenaryToggle;
	public TitleController titleController;
	public CanvasGroup cg;
	public CardZoomer cardZoomer;
	public GroupChooserScreen groupChooser;
	public GameObject addAllyButton;
	public Button addHeroButton, startMissionButton, difficultyButton;
	public Image allyImage;
	public Text[] enemyGroupText;
	public HeroMeta[] heroMetas;
	public MissionTextBox missionTextBox;
	public HeroChooser heroChooser;

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
		rulesText.text = "ally rules";
		deploymentText.text = "no";
		imperialToggle.isOn = true;
		mercenaryToggle.isOn = true;
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
	}

	public void OnDifficulty()
	{
		sound.PlaySound( FX.Click );
		difficultyText.text = DataStore.sessionData.ToggleDifficulty();
		ColorBlock cb = difficultyButton.colors;
		cb.normalColor = new Color( 0, 0.6440244f, 1, 1 );
		difficultyButton.colors = cb;
	}

	public void OnAllyRules()
	{
		sound.PlaySound( FX.Click );
		rulesText.text = DataStore.sessionData.ToggleRules();
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
		DataStore.sessionData.ToggleMercs( imperialToggle.isOn );
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
