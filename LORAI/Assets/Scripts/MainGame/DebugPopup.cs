using TMPro;
using UnityEngine;

public class DebugPopup : MonoBehaviour
{
	public TextMeshProUGUI threat, modifier;
	public Transform container;
	public GameObject dbObjectPrefab;

	public void Show()
	{
		gameObject.SetActive( true );

		threat.text = "Current Threat: " + DataStore.sessionData.gameVars.currentThreat.ToString();
		modifier.text = "Deployment Modifier: " + DataStore.sessionData.gameVars.deploymentModifier.ToString();

		foreach ( var cd in DataStore.deploymentHand )
		{
			var go = Instantiate( dbObjectPrefab, container );
			go.transform.localScale = Vector3.one;
			go.transform.localEulerAngles = Vector3.zero;
			go.GetComponent<DebugObject>().Init( cd );
		}
	}

	public void OnClose()
	{
		foreach ( Transform tf in container )
			Destroy( tf.gameObject );
		gameObject.SetActive( false );
	}

	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
			OnClose();
	}
}
