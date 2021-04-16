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
	public GameObject exhaustedOverlay;

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

		SetHealth( cd.heroHealth );

		Transform tf = transform.GetChild( 0 );
		tf.localScale = Vector3.zero;
		tf.DOScale( 1, 1f ).SetEase( Ease.OutBounce );
	}

	public void OnCount1( Toggle t )
	{
		if ( !woundToggle.gameObject.activeInHierarchy )
			return;

		cardDescriptor.isHealthy = t.isOn;
		if ( cardDescriptor.isHealthy )
		{
			cardDescriptor.heroHealth = HeroHealth.Healthy;
			exhaustedOverlay.SetActive( false );
		}
		else
			cardDescriptor.heroHealth = HeroHealth.Wounded;

		if ( exhaustedOverlay.activeInHierarchy )
			cardDescriptor.heroHealth = HeroHealth.Defeated;

		//if it's an ally, mark it defeated
		if ( cardDescriptor.id[0] == 'A' )
		{
			exhaustedOverlay.SetActive( !cardDescriptor.isHealthy );
			cardDescriptor.heroHealth = HeroHealth.Defeated;
		}

		//Debug.Log( "HEALTHY: " + cardDescriptor.isHealthy );
	}

	/// <summary>
	/// Toggle DEFEATED (dimmed overlay)
	/// </summary>
	public void OnClickSelf()
	{
		exhaustedOverlay.SetActive( !exhaustedOverlay.activeInHierarchy );
		woundToggle.isOn = !exhaustedOverlay.activeInHierarchy;
		if ( exhaustedOverlay.activeInHierarchy )
			cardDescriptor.heroHealth = HeroHealth.Defeated;
	}

	public void OnPointerClick()
	{
		CardZoom cardZoom = GlowEngine.FindObjectsOfTypeSingle<CardZoom>();
		Sprite s = null;
		if(cardDescriptor.id[0]=='A')
			s= Resources.Load<Sprite>($"Cards/Allies/{cardDescriptor.id}");
		if (s != null)
			cardZoom.Show(s, cardDescriptor);
	}

	public void SetHealth( HeroHealth heroHealth )
	{
		cardDescriptor.heroHealth = heroHealth;
		woundToggle.gameObject.SetActive( false );//skip callback

		if ( heroHealth == HeroHealth.Wounded || heroHealth == HeroHealth.Defeated )
		{
			woundToggle.isOn = false;
		}
		if ( cardDescriptor.heroHealth == HeroHealth.Defeated )
			exhaustedOverlay.SetActive( true );

		woundToggle.gameObject.SetActive( true );
	}
}
