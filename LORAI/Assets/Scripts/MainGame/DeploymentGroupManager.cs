using System.Collections.Generic;
using UnityEngine;

public class DeploymentGroupManager : MonoBehaviour
{
	public Transform gridContainer, heroContainer;
	public GameObject dgPrefab, hgPrefab;

	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();
	}

	/// <summary>
	/// deploys hero/ally to hero box and adds it to deployed hero list
	/// </summary>
	public void DeployHeroAlly( CardDescriptor cd )
	{
		if ( DataStore.deployedHeroes.Contains( cd ) )
		{
			Debug.Log( cd.name + " already deployed" );
			return;
		}

		cd.isHealthy = true;
		var go = Instantiate( hgPrefab, heroContainer );
		go.GetComponent<HGPrefab>().Init( cd );
		DataStore.deployedHeroes.Add( cd );
		sound.PlaySound( FX.Computer );
	}

	public void DeployStartingGroups()
	{
		foreach ( var cd in DataStore.sessionData.selectedDeploymentCards[0].cards )
		{
			cd.currentSize = cd.size;
			var go = Instantiate( dgPrefab, gridContainer );
			go.GetComponent<DGPrefab>().Init( cd );
			DataStore.deployedEnemies.Add( cd );
		}
		var rt = gridContainer.GetComponent<RectTransform>();
		rt.localPosition = new Vector3( 20, -3000, 0 );
		sound.PlaySound( FX.Deploy );
	}

	/// <summary>
	/// Takes an enemy or villain, applies difficulty modifier, deploys, removes from dep hand, adds to deployed list
	/// </summary>
	public void DeployGroup( CardDescriptor cardDescriptor, bool skipEliteModify = false )
	{
		// EASY: Any time an Elite group is deployed, it has a 15% chance to be downgraded to a normal group without refunding of threat. ( If the respective normal group is still available.)
		if ( DataStore.sessionData.difficulty == Difficulty.Easy &&
			!skipEliteModify &&
			cardDescriptor.name.Contains( "Elite" ) &&
			GlowEngine.RandomBool( 15 ) )
		{
			//see if normal version exists, include dep hand
			var nonE = DataStore.GetNonEliteVersion( cardDescriptor.name );
			if ( nonE != null )
			{
				Debug.Log( "DeployGroup EASY mode Elite downgrade: " + nonE.name );
				cardDescriptor = nonE;
				GlowEngine.FindObjectsOfTypeSingle<QuickMessage>().Show( "One or more <color=\"red\">Elite</color> Deployments have been downgraded to a <color=\"green\">Regular</color> Group." );
			}
		}

		//Hard: Threat increase x1.3 Any time a normal group is deployed, it has a 15 % chance to be upgraded to an Elite group at no additional threat cost. ( If the respective normal group is still available.) Deployment Modifier starts at 2 instead of 0.
		if ( DataStore.sessionData.difficulty == Difficulty.Hard &&
			!skipEliteModify &&
			!cardDescriptor.name.Contains( "Elite" ) &&
			GlowEngine.RandomBool( 15 ) )
		{
			//see if elite version exists, include dep hand
			var elite = DataStore.GetEliteVersion( cardDescriptor.name );
			if ( elite != null )
			{
				Debug.Log( "DeployGroup HARD mode Elite upgrade: " + elite.name );
				cardDescriptor = elite;
				GlowEngine.FindObjectsOfTypeSingle<QuickMessage>().Show( "One or more <color=\"green\">Regular</color> Deployments have been upgraded to an <color=\"red\">Elite</color> Group." );
			}
			else
				Debug.Log( "SKIPPED: " + cardDescriptor.name );
		}

		if ( DataStore.deployedEnemies.Contains( cardDescriptor ) )
		{
			Debug.Log( cardDescriptor.name + " already deployed" );
			return;
		}

		cardDescriptor.currentSize = cardDescriptor.size;
		var go = Instantiate( dgPrefab, gridContainer );
		go.GetComponent<DGPrefab>().Init( cardDescriptor );

		//add it to deployed enemies
		DataStore.deployedEnemies.Add( cardDescriptor );
		//if it's FROM the dep hand, remove it (should have been already removed in DeploymentPopup)
		DataStore.deploymentHand.Remove( cardDescriptor );

		FX[] sounds = { FX.None, FX.Trouble, FX.Drill, FX.Droid, FX.SetBlasters, FX.Restricted, FX.DropWeapons };
		int[] rnd = GlowEngine.GenerateRandomNumbers( sounds.Length );
		if ( sounds[rnd[0]] != FX.None )
			sound.PlaySound( sounds[rnd[0]] );

		//var rt = gridContainer.GetComponent<RectTransform>();
		//rt.localPosition = new Vector3( 20, -3000, 0 );
	}

	/// <summary>
	/// updates current deploy size
	/// </summary>
	public void UpdateGroups()
	{
		foreach ( Transform enemy in gridContainer )
		{
			enemy.GetComponent<DGPrefab>().UpdateCount();
		}
	}

	public List<CardDescriptor> GetNonExhaustedGroups()
	{
		var cd = new List<CardDescriptor>();
		foreach ( Transform c in gridContainer )
		{
			var pf = c.GetComponent<DGPrefab>();
			if ( !pf.IsExhausted )
				cd.Add( pf.Card );
		}
		return cd;
	}

	public void ExhaustGroup( string id )
	{
		foreach ( Transform c in gridContainer )
		{
			var pf = c.GetComponent<DGPrefab>();
			if ( pf.Card.id == id )
			{
				pf.ToggleExhausted( true );
				return;
			}
		}
	}

	public void ReadyAllGroups()
	{
		foreach ( Transform c in gridContainer )
		{
			var pf = c.GetComponent<DGPrefab>();
			pf.ToggleExhausted( false );
		}
	}
}
