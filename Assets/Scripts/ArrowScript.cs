using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour 
{
	public float offset = 0.0f;
	public float rate = 0.01f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		offset += rate;
		if( offset > 1.0f )
			offset = 0.0f;
		
		renderer.material.mainTextureOffset = new Vector2( offset, 0.0f );
	}
}
