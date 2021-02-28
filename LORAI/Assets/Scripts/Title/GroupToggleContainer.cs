using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Handle toggling ENEMY GROUPS and VILLAINS ONLY
public class GroupToggleContainer : MonoBehaviour
{
	public Image previewImage;
	public GameObject previewButton;

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
		//checking for Active makes sure this code does NOT run when the Toggle is INACTIVE
		if ( !buttonToggles[index].gameObject.activeInHierarchy )
			return;

		sound.PlaySound( FX.Click );
		previewImage.gameObject.SetActive( true );
		previewButton.gameObject.SetActive( true );

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
				//case 4:
				//	texture = Resources.Load<Sprite>( $"Cards/Allies/{enemyCards[index].id}" );
				//	break;
		}

		previewImage.sprite = texture;

		if ( buttonToggles[index].isOn )
		{
			DataStore.sessionData.selectedDeploymentCards[groupIndex].cards.Add( enemyCards[index] );
		}
		else
		{
			DataStore.sessionData.selectedDeploymentCards[groupIndex].cards.Remove( enemyCards[index] );
		}
	}

	public void OnChangeExpansion( string expansion )
	{
		Enum.TryParse( expansion, out selectedExpansion );

		previewImage.gameObject.SetActive( false );
		foreach ( Transform c in transform )
		{
			c.gameObject.SetActive( false );
			c.GetComponent<Toggle>().isOn = false;
		}

		if ( groupIndex == 0 || groupIndex == 1 || groupIndex == 3 )
			deploymentCards = DataStore.deploymentCards;
		else if ( groupIndex == 2 )
			deploymentCards = DataStore.villainCards;

		enemyCards = deploymentCards.cards.Where( x => x.expansion == expansion ).ToList();
		DeploymentCards prevSelected = DataStore.sessionData.selectedDeploymentCards[groupIndex];

		for ( int i = 0; i < enemyCards.Count; i++ )
		{
			//switch on if previously selected
			//do it while Toggle is INACTIVE so OnToggle code doesn't run
			if ( prevSelected.cards.Contains( enemyCards[i] ) )
				buttonToggles[i].isOn = true;

			var child = transform.GetChild( i );
			child.gameObject.SetActive( true );
			var label = child.Find( "Label" );
			label.GetComponent<Text>().text = enemyCards[i].name.ToLower();
		}
	}

	public void ResetUI( int dataGroupIndex )
	{
		previewImage.gameObject.SetActive( false );
		//0=starting, 1=reserved, 2=villains, 3=ignored, 4=heroes
		groupIndex = dataGroupIndex;
		transform.parent.parent.Find( "expansion selector container" ).Find( "Core" ).GetComponent<Toggle>().isOn = true;
		OnChangeExpansion( "Core" );
	}
}
