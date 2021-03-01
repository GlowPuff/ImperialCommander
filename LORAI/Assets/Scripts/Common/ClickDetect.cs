using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetect : MonoBehaviour, IPointerClickHandler
{
	public DGPrefab pfab;

	public void OnPointerClick( PointerEventData eventData )
	{
		if ( eventData.button == PointerEventData.InputButton.Right )
			pfab.OnPointerClick();
	}
}
