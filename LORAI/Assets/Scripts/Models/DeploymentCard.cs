using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class DeploymentCards
{
	public List<CardDescriptor> cards = new List<CardDescriptor>();
}

public class CardDescriptor : IEquatable<CardDescriptor>
{
	//== data from JSON
	public string name;
	public string id;
	public int tier;
	public string faction;
	public int priority;
	public int cost;
	public int rcost;
	public int size;
	public int fame;
	public int reimb;
	public string expansion;
	public string ignored;
	[DefaultValue( false )]
	[JsonProperty( DefaultValueHandling = DefaultValueHandling.Populate )]
	public bool isElite;
	//==

	//==upkeep properties
	public int currentSize;
	public int colorIndex;
	//start v.1.0.17 additions
	[DefaultValue( false )]
	[JsonProperty( DefaultValueHandling = DefaultValueHandling.Populate )]
	public bool hasActivated;
	public string bonusName, bonusText, rebelName;
	public InstructionOption instructionOption;
	//==end v.1.0.17 additions

	//start v.1.0.20 additions
	public bool isDummy;
	public HeroState heroState;
	//==end v.1.0.20 additions

	public bool Equals( CardDescriptor obj )
	{
		if ( obj == null )
			return false;
		CardDescriptor objAsPart = obj as CardDescriptor;
		if ( objAsPart == null )
			return false;
		else
			return id == objAsPart.id;
	}
}

/*
			"name": "Stormtrooper",
			"id": "DG001",
			"tier": 2,
			"faction": "Imperial",
			"priority": 2,
			"cost": 6,
			"rcost": 2,
			"size": 3,
			"fame": 6,
			"reimb": 3,
			"expansion": "Core",
			"ignored": "Squad Training"
*/
