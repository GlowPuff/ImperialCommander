using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Handle toggling HEROES and ALLY ONLY
public class HeroAllyToggleContainer : MonoBehaviour
{
	[HideInInspector]
	public CardDescriptor selectedHero;

	List<CardDescriptor> heroCards = new List<CardDescriptor>();
	Toggle[] buttonToggles;
	ChooserMode chooserMode;
	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();

		buttonToggles = new Toggle[transform.childCount];
		for ( int i = 0; i < transform.childCount; i++ )
		{
			buttonToggles[i] = transform.GetChild( i ).GetComponent<Toggle>();
		}
	}

	public void ResetUI( ChooserMode mode )
	{
		chooserMode = mode;
		if ( mode == ChooserMode.Ally )//reset ally to none
			DataStore.sessionData.selectedAlly = null;

		selectedHero = null;

		//reset to show Core expansion
		transform.parent.parent.Find( "expansion selector container" ).Find( "Core" ).GetComponent<Toggle>().isOn = true;
		OnChangeExpansion( "Core" );
	}

	public void OnChangeExpansion( string expansion )
	{
		//disable all toggle buttons
		foreach ( Transform c in transform )
		{
			c.gameObject.SetActive( false );
			c.GetComponent<Toggle>().isOn = false;
		}

		//only get card list of chosen expansion
		if ( chooserMode == ChooserMode.Hero )
			heroCards = DataStore.heroCards.cards.Where( x => x.expansion == expansion ).ToList();
		else if ( chooserMode == ChooserMode.Ally )
			heroCards = DataStore.allyCards.cards.Where( x => x.expansion == expansion ).ToList();

		//activate toggle btns and change label for each card in list
		for ( int i = 0; i < heroCards.Count; i++ )
		{
			var child = transform.GetChild( i );
			child.gameObject.SetActive( true );
			var label = child.Find( "Label" );
			label.GetComponent<Text>().text = heroCards[i].name.ToLower();
		}
	}

	public void OnToggle( int index )
	{
		//checking for Active makes sure this code does NOT run when the Toggle is INACTIVE
		if ( !buttonToggles[index].gameObject.activeInHierarchy )
			return;

		sound.PlaySound( FX.Click );

		if ( buttonToggles[index].isOn )
		{
			selectedHero = heroCards[index];
		}
		else
		{
			if ( selectedHero == heroCards[index] )
				selectedHero = null;
		}
	}

	public void OnBack()
	{
		if ( chooserMode == ChooserMode.Hero )
		{
			if ( selectedHero != null && !DataStore.sessionData.selectedDeploymentCards[4].cards.Contains( selectedHero ) )
				DataStore.sessionData.selectedDeploymentCards[4].cards.Add( selectedHero );
		}
		else if ( chooserMode == ChooserMode.Ally )
		{
			if ( selectedHero != null )
				DataStore.sessionData.selectedAlly = selectedHero;
		}
		FindObjectOfType<GroupChooserScreen>().OnBack();
	}
}
