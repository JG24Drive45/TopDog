using UnityEngine;
using System.Collections;

public class BridgeTileScript : MonoBehaviour {

	private MeshRenderer meshRenderer;

	void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.enabled = false;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
