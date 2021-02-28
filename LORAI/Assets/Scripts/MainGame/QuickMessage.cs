using TMPro;
using UnityEngine;
using DG.Tweening;

public class QuickMessage : MonoBehaviour
{
	public TextMeshProUGUI message;
	public CanvasGroup cg;

	public void Show( string m )
	{
		gameObject.SetActive( true );
		cg.alpha = 0;
		cg.DOFade( 1, .25f );
		message.text = m;
		cg.DOFade( 0, .25f ).SetDelay( 3 ).OnComplete( () => { gameObject.SetActive( false ); } );
	}
}
