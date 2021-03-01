using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
	public Image fader, image;
	public CanvasGroup cg;
	public TextMeshProUGUI ignoreText;

	public void Show( Sprite s, CardDescriptor cd )
	{
		gameObject.SetActive( true );
		cg.DOFade( 1, .5f );
		fader.color = new Color( 0, 0, 0, 0 );
		fader.DOFade( .95f, .5f );

		image.sprite = s;
		image.transform.localScale = new Vector3( .85f, .85f, .85f );
		image.transform.DOScale( 1, .5f ).SetEase( Ease.OutExpo );

		if ( !string.IsNullOrEmpty( cd.ignored ) )
		{
			ignoreText.text = "<color=\"red\"><font=\"ImperialAssaultSymbols SDF\">F</font></color>" + cd.ignored;
		}
		else
			ignoreText.text = "";
	}

	public void OnOK()
	{
		cg.DOFade( 0, .2f );
		fader.DOFade( 0, .5f ).OnComplete( () => gameObject.SetActive( false ) );
		image.transform.DOScale( .85f, .5f ).SetEase( Ease.OutExpo );
	}
}
