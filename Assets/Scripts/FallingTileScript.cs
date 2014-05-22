using UnityEngine;
using System.Collections;

public class FallingTileScript : MonoBehaviour 
{
	public delegate void FallingTileFell(GameObject tileToAdd);
	public static event FallingTileFell onFallingTile;

	public bool bBeenTouched = false;										// Has the player touched the falling tile yet

	public float fTimeTilFalling = 3.0f;									// Time until the tile falls once touched by the player
	private float fFallingSpeed = 200.0f;
	private const float KILLHEIGHT = -300.0f;
	private bool bGeneratedEmptyTile = false;

	public GameObject emptyTile;

	public void OnEnable()
	{
		Messenger.AddListener( "falling tile touched", ToggleBeenTouched );
	}

	public void OnDisable()
	{
		Messenger.RemoveListener( "falling tile touched", ToggleBeenTouched );
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( bBeenTouched )
		{
			if( fTimeTilFalling <= 0.0f )
			{
				// Generate an empty tile at this position once
				if( !bGeneratedEmptyTile )
				{
					Vector3 tempPosition = this.transform.position;
					tempPosition += new Vector3( 0, 7.5f, 0 );
					GameObject tempO = Instantiate( emptyTile, tempPosition, Quaternion.Euler( new Vector3( 0, 0, 0 ) ) ) as GameObject;
					if( onFallingTile != null )											// If there is a subscriber
						onFallingTile( tempO );
					bGeneratedEmptyTile = true;
				}

				if( transform.position.y <= KILLHEIGHT )
				{
					// destroy the tile
					Destroy( this.gameObject );
				}
				else
				{
					// make the tile fall
					transform.position -= new Vector3( 0, fFallingSpeed * Time.deltaTime, 0 );
				}
			}
			else
			{
				// Decrement time
				fTimeTilFalling -= Time.deltaTime;
			}
		}
	}

	public void ToggleBeenTouched()
	{
		if( !bBeenTouched )
			bBeenTouched = true;

		// Turn off the box collider
		GetComponent<BoxCollider>().enabled = false;
	}
}
