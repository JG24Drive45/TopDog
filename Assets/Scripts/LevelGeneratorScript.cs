using UnityEngine;
using System.Collections;
using System.IO;

public class LevelGeneratorScript : MonoBehaviour 
{

	public GameObject mainTile;
	public GameObject emptyTile;
	public GameObject goalTile;
	public GameObject ketchup;
	public GameObject mustard;
	public GameObject relish;

	string sLevel = "Level1";

	// INSTEAD OF GENERATING TILES LIKE BELOW, WE CAN READ DATA IN FROM A TEXT FILE TO GENERATE THE LEVELS
	void Start () 
	{
		string[] lines;

		TextAsset data;

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

//		int x, z;
//		z = 0;
//
//		for(int i = 0; i < 10; i++)
//		{
//			x = 0;
//
//			for(int j = 0; j < 10; j++)
//			{
//				Instantiate( tempMainTile, new Vector3( x, 0, z), Quaternion.identity );
//				x += 25;
//			}
//
//			z += 25;
//		}
//
//		// Place the ketchup, mustard, and relish
//		Instantiate( tempKetchup, new Vector3( 125.0f, 0.0f, 0.0f ), Quaternion.identity );
//		Instantiate( tempMustard, new Vector3( 75.0f, 0.0f, 75.0f ), Quaternion.identity );
//		Instantiate( tempRelish, new Vector3( 0.0f, 0.0f, 150.0f ), Quaternion.identity );


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
