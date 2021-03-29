using System.Collections.Generic;

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
	public int colorIndex;
	public HeroHealth heroHealth;
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
			"expansion": "Core",
			"ignored": "Squad Training"
*/