using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MWheelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public int wheelValue;
	public int maxValue = 10;
	public int minValue = 0;
	public Text numberText;

	bool isHovering = false;
	Sound sound;

	private void Start()
	{
		if ( numberText == null )
			numberText = GetComponent<Text>();
		sound = FindObjectOfType<Sound>();
	}

	void Update()
	{
		if ( Input.mouseScrollDelta.magnitude > 0 && isHovering )
		{
			if ( Input.mouseScrollDelta.y == 1 )
			{
				wheelValue = Mathf.Min( maxValue, wheelValue + 1 );
				sound.PlaySound( FX.Click );
			}
			else if ( Input.mouseScrollDelta.y == -1 )
			{
				wheelValue = Mathf.Max( minValue, wheelValue - 1 );
				sound.PlaySound( FX.Click );
			}
		}

		numberText.text = wheelValue.ToString();
	}
	public void OnPointerEnter( PointerEventData eventData )
	{
		isHovering = true;
	}

	public void OnPointerExit( PointerEventData eventData )
	{
		isHovering = false;
	}

	public void ResetWheeler()
	{
		wheelValue = 0;
		numberText.text = "0";
	}
}
