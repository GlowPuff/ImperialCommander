using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Handle toggling MISSIONS ONLY
public class MissionToggleContainer : MonoBehaviour
{
	List<Card> missionCards = new List<Card>();
	Toggle[] buttonToggles;
	Expansion selectedExpansion;
	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();

		buttonToggles = new Toggle[40];
		for ( int i = 0; i < transform.childCount; i++ )
			buttonToggles[i] = transform.GetChild( i ).GetComponent<Toggle>();
	}

	public void ResetUI()
	{
		missionCards = DataStore.missionCards[DataStore.sessionData.selectedMissionExpansion.ToString()];
		//for missions, select expansion of currently selected mission
		transform.parent.parent.Find( "expansion selector container" ).Find( DataStore.sessionData.selectedMissionExpansion.ToString() ).GetComponent<Toggle>().isOn = true;
		//...and show data for that expansion
		OnChangeExpansion( DataStore.sessionData.selectedMissionExpansion.ToString() );
	}

	public void OnChangeExpansion( string expansion )
	{
		Enum.TryParse( expansion, out selectedExpansion );

		if ( DataStore.missionCards.TryGetValue( expansion, out missionCards ) )
		{
			foreach ( Transform c in transform )
			{
				c.gameObject.SetActive( false );
				c.GetComponent<Toggle>().isOn = false;
			}

			for ( int i = 0; i < missionCards.Count(); i++ )
			{
				//switch on if previously selected
				//do it while Toggle is INACTIVE so OnToggle code doesn't run
				if ( DataStore.sessionData.selectedMissionName == missionCards[i].name )
					buttonToggles[i].isOn = true;

				var child = transform.GetChild( i );
				child.gameObject.SetActive( true );
				child.GetComponent<Toggle>().isOn = false;
				var label = child.Find( "Label" );
				label.GetComponent<Text>().text = missionCards[i].name.ToLower();
			}
		}
	}

	public void OnToggle( int index )
	{
		//checking for Active makes sure this code does NOT run when the Toggle is INACTIVE
		if ( !buttonToggles[index].gameObject.activeInHierarchy )
			return;

		sound.PlaySound( FX.Click );

		DataStore.sessionData.selectedMissionID = missionCards[index].id;
		DataStore.sessionData.selectedMissionName = missionCards[index].name;
		DataStore.sessionData.selectedMissionExpansion = selectedExpansion;
	}
}
