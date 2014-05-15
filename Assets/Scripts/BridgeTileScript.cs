using UnityEngine;
using System.Collections;

public class BridgeTileScript : MonoBehaviour {


	void Awake()
	{
		GetComponent<MeshRenderer>().enabled = false;					// Temporarily Disable the mesh renderer
	}

	void OnEnable()
	{
		Messenger.AddListener( "activate bridge", ActivateBridge );
	}

	void OnDisable()
	{
		Messenger.RemoveListener( "activate bridge", ActivateBridge );
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ActivateBridge()
	{
		// Turn off the empty tile that is in the bridge tile's current position
		GameObject[] tiles = GameObject.FindGameObjectsWithTag( "EmptyTile" );

		int length = tiles.Length;										// Cache the array length
		int i;
		for( i = 0; i < length; i++ )									// Find the matching empty tile
		{
			if( tiles[i].transform.position.x == this.transform.position.x &&
			    tiles[i].transform.position.z == this.transform.position.z )
				break;
		}

		if( i < length )												// Error Checking
			Destroy( tiles[i] );										// Kill the empty tile

		GetComponent<MeshRenderer>().enabled = true;					// Enable the mesh renderer
	}
}
