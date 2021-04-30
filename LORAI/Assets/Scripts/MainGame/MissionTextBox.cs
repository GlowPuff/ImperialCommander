using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionTextBox : MonoBehaviour
{
	public TextMeshProUGUI theText;
	public Image fader;
	public CanvasGroup cg;

	public void Show( string text )
	{
		gameObject.SetActive( true );
		theText.text = text;
		fader.color = new Color( 0, 0, 0, 0 );
		fader.DOFade( .95f, 1 );
		cg.DOFade( 1, .5f );

		theText.transform.parent.localPosition = new Vector3( theText.transform.parent.localPosition.x, -3000, 0 );

		transform.GetChild( 0 ).localScale = new Vector3( .85f, .85f, .85f );
		transform.GetChild( 0 ).DOScale( 1, .5f ).SetEase( Ease.OutExpo );
	}

	public void OnClose()
	{
		FindObjectOfType<Sound>().PlaySound( FX.Click );
		fader.DOFade( 0, .5f ).OnComplete( () => gameObject.SetActive( false ) );
		cg.DOFade( 0, .2f );
		transform.GetChild( 0 ).DOScale( .85f, .5f ).SetEase( Ease.OutExpo );
	}

	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
			OnClose();
	}
}
