using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeroChooser : MonoBehaviour
{
	public Image fader;
	public CanvasGroup cg;
	public Transform container;

	Sound sound;
	List<CardDescriptor> selectedHeroes;
	List<CardDescriptor> ownedHeroes;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();
	}

	public void Show()
	{
		gameObject.SetActive( true );
		fader.DOFade( .95f, .5f );
		cg.DOFade( 1, .5f );


		foreach ( Transform tf in container )
		{
			tf.gameObject.SetActive( false );
			tf.GetComponent<Toggle>().isOn = false;
		}

		selectedHeroes = DataStore.sessionData.MissionHeroes;
		//filter owned heroes
		ownedHeroes = DataStore.heroCards.cards.Owned();

		//only show owned heroes, toggle if previously selected
		for ( int i = 0; i < ownedHeroes.Count; i++ )
		{
			var child = container.GetChild( i );
			var label = child.Find( "Label" );
			label.GetComponent<Text>().text = ownedHeroes[i].name.ToLower();
			//toggle if already selected
			if ( selectedHeroes.Contains( ownedHeroes[i] ) )
				child.GetComponent<Toggle>().isOn = true;
			child.gameObject.SetActive( true );
		}

		UpdateInteractable();
	}

	public void OnBack()
	{
		sound.PlaySound( FX.Click );
		fader.DOFade( 0, .5f );
		cg.DOFade( 0, .5f ).OnComplete( () => gameObject.SetActive( false ) );
		FindObjectOfType<NewGameScreen>().OnReturnTo();
	}

	public void OnToggle( Toggle t )
	{
		if ( !t.gameObject.activeInHierarchy )
			return;

		sound.PlaySound( FX.Click );

		//get index Toggle (1)
		Regex rx = new Regex( @"[\d+]" );
		var m = rx.Match( t.name );
		int idx = int.Parse( m.Value ) - 1;
		CardDescriptor clicked = ownedHeroes[idx];
		if ( t.isOn && !selectedHeroes.Contains( clicked ) )
			selectedHeroes.Add( clicked );
		else if ( !t.isOn && selectedHeroes.Contains( clicked ) )
			selectedHeroes.Remove( clicked );

		UpdateInteractable();
		//foreach ( CardDescriptor cd in selectedHeroes )
		//	Debug.Log( cd.name );
	}

	void UpdateInteractable()
	{
		//only 4 allowed
		if ( DataStore.sessionData.MissionHeroes.Count == 4 )
		{
			foreach ( Transform tf in container )
			{
				if ( !tf.GetComponent<Toggle>().isOn )
					tf.GetComponent<Toggle>().interactable = false;
			}
		}
		else
		{
			foreach ( Transform tf in container )
				tf.GetComponent<Toggle>().interactable = true;
		}
	}
}
