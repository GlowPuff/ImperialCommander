using UnityEngine;

public class WorldSpinner : MonoBehaviour
{
	public Transform world;

	void Update()
	{
		float xScalar = GlowEngine.SineAnimation( .005f, .06f, .4f );
		float yScalar = GlowEngine.SineAnimation( .005f, .06f, .15f );
		float zScalar = GlowEngine.SineAnimation( -.04f, .04f, .6f );
		world.Rotate( xScalar, yScalar, zScalar );
	}
}
