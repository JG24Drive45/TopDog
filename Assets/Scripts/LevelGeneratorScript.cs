using UnityEngine;
using System.Collections;
using System.IO;

public class LevelGeneratorScript : MonoBehaviour 
{
	// GameObject variables
	public GameObject mainTile;
	public GameObject emptyTile;
	public GameObject teleporterTile;
	public GameObject bridgeTile;
	public GameObject fallingTile;
	public GameObject goalTile;
	public GameObject ketchup;
	public GameObject mustard;
	public GameObject relish;
	public GameObject player;
	public GameObject switchObject;

	private string sLevel;											// Name of the current level


	// INSTEAD OF GENERATING TILES LIKE BELOW, WE CAN READ DATA IN FROM A TEXT FILE TO GENERATE THE LEVELS
	void Start () 
	{
		sLevel = Application.loadedLevelName;						// Get the name of the current level

		string[] lines;												// Array that stores text file info

		TextAsset data;												// Text file variable

		data = ( TextAsset )Resources.Load( sLevel );
		lines = data.text.Split( "\n"[0] );

		// Read the first 13 lines of text file that make up the tiles
		for( int i = 2; i < 14; i++ )
		{
			int lineLength = lines[i].Length;
			for( int j = 0; j < lineLength; j++ )
			{
				if( lines[i][j] == 'E' )
					Instantiate( emptyTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( lines[i][j] == 'M' )
					Instantiate( mainTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( lines[i][j] == 'G' )
					Instantiate( goalTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( lines[i][j] == 'T' )
					Instantiate( teleporterTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( lines[i][j] == 'B' )
				{
					Instantiate( emptyTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
					Instantiate( bridgeTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				}
				else if( lines[i][j] == 'F' )
					Instantiate( fallingTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
			}
		}

		// Read lines 17 & 18 to get the ketchup location
		Vector3 tempPosition = new Vector3( int.Parse( lines[16] ), 12.5f, -int.Parse( lines[17] ) );
		Debug.Log( "Ketchup is at: " + tempPosition );			// For testing
		Instantiate( ketchup, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) );

		// Read lines 20 & 21 to get the mustard location
		tempPosition = new Vector3( int.Parse( lines[19] ) , 12.5f, -int.Parse( lines[20] ) );
		Debug.Log( "Mustard is at: " + tempPosition );			// For testing
		Instantiate( mustard, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) );

		// Read lines 23 & 24 to get the relish location
		tempPosition = new Vector3( int.Parse( lines[22] ), 12.5f, -int.Parse( lines[23] ) );
		Debug.Log( "Relish is at: " + tempPosition );			// For testing
		Instantiate( relish, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) );

		// Read lines 26 & 27 to get the switch location
		tempPosition = new Vector3( int.Parse( lines[25] ), 12.5f, -int.Parse( lines[26] ) );
		Debug.Log( "Switch is at: " + tempPosition );
		Instantiate( switchObject, tempPosition, Quaternion.Euler( new Vector3( 270.0f, 0.0f, 0.0f ) ) );

		// Read lines 29 - 31 to get the player's (hotdog) location
		tempPosition = new Vector3( int.Parse( lines[28] ), int.Parse( lines[29] ), -int.Parse( lines[30] ) );
		Debug.Log( "Player is at: " + tempPosition );
		// Read lines 33 - 35 to get the player's (hotdog) rotation
		Vector3 tempRotation = new Vector3( int.Parse( lines[32] ), int.Parse( lines[33] ), int.Parse( lines[34] ) );
		Debug.Log( "Player rotation is: " + tempRotation );
		Instantiate( player, tempPosition, Quaternion.Euler( tempRotation ) );

		// Read line 37 to get the player's orientation state
		int tempState = int.Parse( lines[36] );
		Debug.Log( "OState is: " + tempState );
		// Send the message to set the player's orientation state
		Messenger<int>.Broadcast( "set player orientation state", tempState );


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
