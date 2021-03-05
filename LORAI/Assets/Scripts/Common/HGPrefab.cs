using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class HGPrefab : MonoBehaviour
{
	public Toggle woundToggle;
	public Image iconImage;
	public Outline outline;
	public Color eliteColor;

	CardDescriptor cardDescriptor;

	private void Awake()
	{
		Transform tf = transform.GetChild( 0 );
		tf.localScale = Vector3.zero;
	}

	public void Init( CardDescriptor cd )
	{
		Debug.Log( "DEPLOYED: " + cd.name );
		cardDescriptor = cd;

		if ( DataStore.heroCards.cards.Any( x => x.id == cd.id ) )
		{
			iconImage.sprite = Resources.Load<Sprite>( $"Cards/Heroes/{cd.id}" );
		}
		else if ( DataStore.allyCards.cards.Any( x => x.id == cd.id ) )
		{
			iconImage.sprite = Resources.Load<Sprite>( $"Cards/Allies/{cd.id.Replace( "A", "M" )}" );
		}

		if ( cd.id[0] == 'A' )
			outline.effectColor = eliteColor;

		Transform tf = transform.GetChild( 0 );
		tf.localScale = Vector3.zero;
		tf.DOScale( 1, 1f ).SetEase( Ease.OutBounce );
	}

	public void OnCount1( Toggle t )
	{
		cardDescriptor.isHealthy = t.isOn;

		//if it's an ally, remove it from game
		if ( DataStore.allyCards.cards.Contains( cardDescriptor ) )
		{
			woundToggle.interactable = false;
			Transform tf = transform.GetChild( 0 );
			tf.DOScale( 0, 1f ).SetEase( Ease.InBounce ).OnComplete( () =>
			{
				//remove it from deployed HERO list
				DataStore.deployedHeroes.Remove( cardDescriptor );
				Object.Destroy( gameObject );
			} );
		}

		//Debug.Log( "HEALTHY: " + cardDescriptor.isHealthy );
	}

	public void OnClickSelf()
	{

	}

	public void OnRemoveSelf()
	{
		woundToggle.interactable = false;
		Transform tf = transform.GetChild( 0 );
		tf.DOScale( 0, 1f ).SetEase( Ease.InBounce ).OnComplete( () =>
		{
			//remove it from deployed HERO list
			DataStore.deployedHeroes.Remove( cardDescriptor );
			Object.Destroy( gameObject );
		} );
	}
}
