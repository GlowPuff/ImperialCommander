using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Handle toggling ENEMY GROUPS and VILLAINS ONLY
public class GroupToggleContainer : MonoBehaviour
{
	public Image previewImage;
	public TextMeshProUGUI previewNameText;

	DeploymentCards deploymentCards;
	List<CardDescriptor> enemyCards;
	int groupIndex;//0=starting, 1=reserved, 2=villains, 3=ignored
	Toggle[] buttonToggles;
	Expansion selectedExpansion;
	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();

		buttonToggles = new Toggle[transform.childCount];//18
		for ( int i = 0; i < transform.childCount; i++ )
			buttonToggles[i] = transform.GetChild( i ).GetComponent<Toggle>();
	}

	public void OnToggle( int index )
	{
		EventSystem.current.SetSelectedGameObject( null );
		//checking for Active makes sure this code does NOT run when the Toggle is INACTIVE
		if ( !buttonToggles[index].gameObject.activeInHierarchy )
			return;

		sound.PlaySound( FX.Click );
		previewImage.gameObject.SetActive( true );
		//previewButton.gameObject.SetActive( true );

		Sprite texture = null;
		//0=starting, 1=reserved, 2=villains, 3=ignored, 4=allies
		switch ( groupIndex )
		{
			case 0:
			case 1:
			case 3:
				texture = Resources.Load<Sprite>( $"Cards/Enemies/{selectedExpansion}/{enemyCards[index].id}" );
				break;
			case 2:
				texture = Resources.Load<Sprite>( $"Cards/Villains/{enemyCards[index].id}" );
				break;
		}

		previewImage.sprite = texture;
		previewNameText.text = enemyCards[index].name;

		if ( buttonToggles[index].isOn )
		{
			DataStore.sessionData.selectedDeploymentCards[groupIndex].cards.Add( enemyCards[index] );
		}
		else
		{
			DataStore.sessionData.selectedDeploymentCards[groupIndex].cards.Remove( enemyCards[index] );
			previewImage.gameObject.SetActive( false );
			previewNameText.text = "";
		}
	}

	public void OnChangeExpansion( string expansion )
	{
		EventSystem.current.SetSelectedGameObject( null );
		Enum.TryParse( expansion, out selectedExpansion );

		previewImage.gameObject.SetActive( false );
		foreach ( Transform c in transform )
		{
			c.gameObject.SetActive( false );
			c.GetComponent<Toggle>().isOn = false;
			c.GetComponent<Toggle>().interactable = true;
		}

		if ( groupIndex == 0 || groupIndex == 1 || groupIndex == 3 )
			deploymentCards = DataStore.deploymentCards;
		else if ( groupIndex == 2 )
			deploymentCards = DataStore.villainCards;

		enemyCards = deploymentCards.cards.Where( x => x.expansion == expansion ).ToList();
		DeploymentCards prevSelected = DataStore.sessionData.selectedDeploymentCards[groupIndex];

		Sprite thumbNail = null;

		for ( int i = 0; i < enemyCards.Count; i++ )
		{
			var child = transform.GetChild( i );
			//switch on if previously selected
			//do it while Toggle is INACTIVE so OnToggle code doesn't run
			if ( prevSelected.cards.Contains( enemyCards[i] ) )
				buttonToggles[i].isOn = true;
			child.gameObject.SetActive( true );//re-enable the Toggle

			if ( groupIndex != 2 )//if NOT villains
				thumbNail = Resources.Load<Sprite>( $"Cards/Enemies/{selectedExpansion}/{enemyCards[i].id.Replace( "DG", "M" )}" );
			else//villain thumb directory
				thumbNail = Resources.Load<Sprite>( $"Cards/Villains/{enemyCards[i].id.Replace( "DG", "M" )}" );

			//set the thumbnail texture
			var thumb = child.Find( "Image" );
			thumb.GetComponent<Image>().sprite = thumbNail;
			if ( !enemyCards[i].isElite )
				thumb.GetComponent<Image>().color = new Color( 1, 1, 1, 1 );
			else
				thumb.GetComponent<Image>().color = new Color( 1, .5f, .5f, 1 );

			//if an enemy is already in another group index (Initial, Reserved, etc), disable the toggle so the enemy can't be added to 2 different groups
			//ie: can't put same enemy into both Initial and Reserved
			if ( IsInGroup( enemyCards[i] ) )
			{
				buttonToggles[i].interactable = false;
				if ( !enemyCards[i].isElite )
					thumb.GetComponent<Image>().color = new Color( 1, 1, 1, .35f );
				else
					thumb.GetComponent<Image>().color = new Color( 1, .5f, .5f, .35f );
			}
		}
	}

	public void ResetUI( int dataGroupIndex )
	{
		previewImage.gameObject.SetActive( false );
		//0=starting, 1=reserved, 2=villains, 3=ignored, 4=heroes
		groupIndex = dataGroupIndex;
		previewNameText.text = "";
		transform.parent.parent.parent.parent.Find( "expansion selector container" ).Find( "Core" ).GetComponent<Toggle>().isOn = true;
		OnChangeExpansion( "Core" );
	}

	public bool IsInGroup( CardDescriptor cd )
	{
		bool found = false;

		for ( int i = 0; i < 5; i++ )
		{
			if ( groupIndex != i )
			{
				if ( DataStore.sessionData.selectedDeploymentCards[i].cards.Contains( cd ) )
					found = true;
			}
		}

		return found;
	}
}
