using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardViewPopup : MonoBehaviour
{
	public CanvasGroup cg;
	public Image fader;
	public DynamicCardPrefab dynamicCard;

	Action callback;

	public void Show( CardDescriptor cd, Action action = null )
	{
		dynamicCard.InitCard( cd );

		callback = action;

		gameObject.SetActive( true );
		fader.color = new Color( 0, 0, 0, 0 );
		fader.DOFade( .95f, .5f );
		cg.DOFade( 1, .5f );
		transform.GetChild( 1 ).localScale = new Vector3( .85f, .85f, .85f );
		transform.GetChild( 1 ).DOScale( 1, .5f ).SetEase( Ease.OutExpo );
	}

	public void OnOK()
	{
		FindObjectOfType<Sound>().PlaySound( FX.Click );
		fader.DOFade( 0, .5f ).OnComplete( () =>
		{
			callback?.Invoke();
			gameObject.SetActive( false );
		} );
		cg.DOFade( 0, .2f );
		transform.GetChild( 1 ).DOScale( .85f, .5f ).SetEase( Ease.OutExpo );
	}

	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
			OnOK();
	}
}
