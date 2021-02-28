using System.Collections.Generic;
using UnityEngine;

public class DeploymentCards
{
	public List<CardDescriptor> cards = new List<CardDescriptor>();
}

public class CardDescriptor
{
	public string name;
	public string id;
	public int tier;
	public string faction;
	public int priority;
	public int cost;
	public int rcost;
	public int size;
	public string expansion;
	public string ignored;
	public int currentSize;
	public bool isHealthy;
	public Color idColor = new Color( 0, 0.3294118f, 1, 1 );
}

//public class DeployedCard : CardDescriptor
//{
//	public int currentSize;
//}

/*
			"name": "Stormtrooper",
			"id": "DG001",
			"tier": 2,
			"faction": "Imperial",
			"priority": 2,
			"cost": 6,
			"rcost": 2,
			"size": 3,
			"expansion": "Core",
			"ignored": "Squad Training"
*/