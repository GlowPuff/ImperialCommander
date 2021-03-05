using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
	public TextMeshProUGUI tmp;

	public void Show( string t )
	{
		gameObject.SetActive( true );
		tmp.text = t;
	}

	public void Hide()
	{
		gameObject.SetActive( false );
	}
}
