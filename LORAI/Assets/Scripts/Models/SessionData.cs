using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SessionData
{
	public Difficulty difficulty;
	/// <summary>
	/// Current threat in the game
	/// </summary>
	public int threatLevel;
	public int addtlThreat;
	public AllyRules allyRules;
	public YesNo optionalDeployment;
	public YesNo allyThreatCost;
	public bool includeImperials, includeMercs;
	public Expansion selectedMissionExpansion;
	public string selectedMissionID;
	public string selectedMissionName;
	//0=starting, 1=reserved, 2=villains, 3=ignored, 4=heroes
	public DeploymentCards[] selectedDeploymentCards;
	public CardDescriptor selectedAlly;
	public GameVars gameVars;

	public List<CardDescriptor> MissionStarting
	{
		get { return selectedDeploymentCards[0].cards; }
	}
	public List<CardDescriptor> MissionReserved
	{
		get { return selectedDeploymentCards[1].cards; }
	}
	public List<CardDescriptor> EarnedVillains
	{
		get { return selectedDeploymentCards[2].cards; }
	}
	public List<CardDescriptor> MissionIgnored
	{
		get { return selectedDeploymentCards[3].cards; }
	}
	public List<CardDescriptor> MissionHeroes
	{
		get { return selectedDeploymentCards[4].cards; }
	}

	public class GameVars
	{
		public int round;
		public int eventsTriggered;
		public int currentThreat;
		public int deploymentModifier;
		public bool vaderReducedCostBy5;
		public bool pauseDeployment;
		public bool pauseThreatIncrease;
	}

	public SessionData()
	{
		difficulty = Difficulty.NotSet;
		threatLevel = addtlThreat = 0;
		allyRules = AllyRules.Normal;
		optionalDeployment = YesNo.No;
		allyThreatCost = YesNo.No;
		selectedMissionExpansion = Expansion.Core;
		selectedMissionID = "core1";
		selectedMissionName = "A New Threat";
		includeImperials = true;
		includeMercs = true;

		selectedDeploymentCards = new DeploymentCards[5];
		for ( int i = 0; i < 5; i++ )
			selectedDeploymentCards[i] = new DeploymentCards();
		selectedAlly = null;

		//ignore "Other" expansion enemy groups by default
		selectedDeploymentCards[3].cards.AddRange( DataStore.deploymentCards.cards.Where( x => x.expansion == "Other" ) );
		gameVars = new GameVars();
	}

	public void InitGameVars()
	{
		gameVars.round = 1;
		gameVars.eventsTriggered = 0;

		gameVars.currentThreat = 0;
		//if ( optionalDeployment == YesNo.Yes )
		//	gameVars.currentThreat += threatLevel * 2;
		if ( allyThreatCost == YesNo.Yes && selectedAlly != null )
			gameVars.currentThreat += selectedAlly.cost;
		gameVars.currentThreat += addtlThreat;

		gameVars.deploymentModifier = 0;
		if ( difficulty == Difficulty.Hard )
			gameVars.deploymentModifier = 2;

		gameVars.vaderReducedCostBy5 = false;
		gameVars.pauseDeployment = false;
		gameVars.pauseThreatIncrease = false;
	}

	public string ToggleDifficulty()
	{
		if ( difficulty == Difficulty.NotSet )
			difficulty = Difficulty.Medium;
		else if ( difficulty == Difficulty.Easy )
			difficulty = Difficulty.Medium;
		else if ( difficulty == Difficulty.Medium )
			difficulty = Difficulty.Hard;
		else
			difficulty = Difficulty.Easy;

		return difficulty.ToString();
	}

	public string ToggleRules()
	{
		if ( allyRules == AllyRules.NotSet )
			allyRules = AllyRules.Normal;
		else if ( allyRules == AllyRules.Normal )
			allyRules = AllyRules.Lothal;
		else
			allyRules = AllyRules.Normal;

		return allyRules.ToString();
	}

	public string ToggleDeployment()
	{
		if ( optionalDeployment == YesNo.Yes )
			optionalDeployment = YesNo.No;
		else
			optionalDeployment = YesNo.Yes;

		return optionalDeployment.ToString();
	}

	public string ToggleThreatCost()
	{
		if ( allyThreatCost == YesNo.Yes )
			allyThreatCost = YesNo.No;
		else
			allyThreatCost = YesNo.Yes;

		return allyThreatCost.ToString();
	}

	public void ToggleImperials( bool isOn )
	{
		includeImperials = isOn;
	}

	public void ToggleMercs( bool isOn )
	{
		includeMercs = isOn;
	}

	public void ToggleHero( string id )
	{
		int idx = selectedDeploymentCards[4].cards.FindIndex( x => x.id == id );
		selectedDeploymentCards[4].cards.RemoveAt( idx );
	}

	/// <summary>
	/// Positive or negative number to add/decrease
	/// </summary>
	public void UpdateThreat( int amount, bool force = false )
	{
		if ( amount > 0 )
		{
			if ( difficulty == Difficulty.Easy )
				amount = (int)Math.Floor( amount * .7f );
			else if ( difficulty == Difficulty.Hard )
				amount = (int)Math.Floor( amount * 1.3f );
		}

		//Debug.Log( "UpdateThreat() amount: " + amount );
		if ( gameVars.pauseThreatIncrease && !force )
		{
			Debug.Log( "THREAT PAUSED" );
			return;
		}

		gameVars.currentThreat = Math.Max( 0, gameVars.currentThreat + amount );
		Debug.Log( "UpdateThreat(): current=" + gameVars.currentThreat );
	}

	public void UpdateDeploymentModifier( int amount )
	{
		gameVars.deploymentModifier = Math.Max( -2, gameVars.deploymentModifier + amount );
		Debug.Log( "DeploymentModifier: " + gameVars.deploymentModifier );
	}
}