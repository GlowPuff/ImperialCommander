using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardZoomer : MonoBehaviour
{
	public Image image, fader;
	public GameObject button;

	Sound sound;

	private void Awake()
	{
		sound = FindObjectOfType<Sound>();
	}

	public void ZoomIn( Sprite sprite )
	{
		image.sprite = sprite;
		image.transform.DOScale( 0.22f, .5f ).SetEase( Ease.OutExpo ).OnComplete( () => button.SetActive( true ) );
		image.DOFade( 1, 1.5f );

		fader.gameObject.SetActive( true );
		fader.DOFade( .75f, .5f );
	}

	public void ZoomOut()
	{
		button.SetActive( false );
		image.transform.DOScale( 0, .5f ).SetEase( Ease.OutExpo );
		image.DOFade( 0, .25f );

		fader.DOFade( 0, .5f ).OnComplete( () => fader.gameObject.SetActive( false ) );
	}

	public void OnClose()
	{
		sound.PlaySound( FX.Click );
		ZoomOut();
	}
}
