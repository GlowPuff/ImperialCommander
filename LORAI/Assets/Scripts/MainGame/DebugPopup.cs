using TMPro;
using UnityEngine;

public class DebugPopup : MonoBehaviour
{
	public TextMeshProUGUI threat, modifier;
	public Transform container;

	public void Show()
	{
		gameObject.SetActive( true );

		threat.text = "Current Threat: " + DataStore.sessionData.gameVars.currentThreat.ToString();
		modifier.text = "Dep Modifier: " + DataStore.sessionData.gameVars.deploymentModifier.ToString();

		foreach ( var cd in DataStore.deploymentHand )
		{
			var go = new GameObject( "dep hand item" );
			go.transform.SetParent( container );
			go.transform.localScale = Vector3.one;
			go.transform.localEulerAngles = Vector3.zero;
			var tmp = go.AddComponent<TextMeshProUGUI>();
			tmp.text = cd.name + " / cost=" + cd.cost;
			tmp.fontSize = 20;
			tmp.color = Color.white;
		}
	}

	public void OnClose()
	{
		foreach ( Transform tf in container )
			Destroy( tf.gameObject );
		gameObject.SetActive( false );
	}
}
