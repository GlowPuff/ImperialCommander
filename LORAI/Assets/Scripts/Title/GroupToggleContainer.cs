﻿using System;
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
	public ExpansionController expansionController;

	DeploymentCards deploymentCards;
	List<CardDescriptor> enemyCards;
	int groupIndex;//0=starting, 1=reserved, 2=villains, 3=ignored
	Toggle[] buttonToggles;
	Expansion selectedExpansion;
	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();

		buttonToggles = new Toggle[transform.childCount];
		for ( int i = 0; i < transform.childCount; i++ )
			buttonToggles[i] = transform.GetChild( i ).GetComponent<Toggle>();
	}

	public void OnToggle( Toggle toggle )
	{
		int index = int.Parse( toggle.name.Substring( 8 ).TrimEnd( ')' ) );

		EventSystem.current.SetSelectedGameObject( null );
		//checking for Active makes sure this code does NOT run when the Toggle is INACTIVE
		if ( !buttonToggles[index].gameObject.activeInHierarchy )
			return;

		sound.PlaySound( FX.Click );
		previewImage.gameObject.SetActive( true );

		var id = int.Parse( enemyCards[index].id.Substring( 2 ).TrimStart( '0' ) );

		if ( id > 69 )
			previewImage.sprite = Resources.Load<Sprite>( $"Cards/Villains/{enemyCards[index].id}" );
		else
			previewImage.sprite = Resources.Load<Sprite>( $"Cards/Enemies/{selectedExpansion}/{enemyCards[index].id}" );

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

		expansionController.UpdateText( (int)selectedExpansion, buttonToggles.Count( x => x.isOn ) );
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

		//0=starting, 1=reserved, 2=villains, 3=ignored
		if ( groupIndex == 0 || groupIndex == 1 )
		{
			deploymentCards = new DeploymentCards() { cards = DataStore.deploymentCards.cards.Concat( DataStore.villainCards.cards ).ToList() };
		}
		else if ( groupIndex == 2 )
			deploymentCards = DataStore.villainCards;
		else if ( groupIndex == 3 )
			deploymentCards = DataStore.deploymentCards;

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

			var id = int.Parse( enemyCards[i].id.Substring( 2 ).TrimStart( '0' ) );
			if ( id <= 69 )//groupIndex != 2 )//if NOT villains
				thumbNail = Resources.Load<Sprite>( $"Cards/Enemies/{selectedExpansion}/{enemyCards[i].id.Replace( "DG", "M" )}" );
			else//villain thumb directory
				thumbNail = Resources.Load<Sprite>( $"Cards/Villains/{enemyCards[i].id.Replace( "DG", "M" )}" );

			//set the thumbnail texture
			var thumb = child.Find( "Image" );
			thumb.GetComponent<Image>().sprite = thumbNail;
			if ( enemyCards[i].isElite || id > 69 )
				thumb.GetComponent<Image>().color = new Color( 1, .5f, .5f, 1 );
			else
				thumb.GetComponent<Image>().color = new Color( 1, 1, 1, 1 );

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

		expansionController.UpdateText( (int)selectedExpansion, buttonToggles.Count( x => x.isOn ) );
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
