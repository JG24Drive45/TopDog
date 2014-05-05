using UnityEngine;
using System.Collections;

public class LevelGeneratorScript : MonoBehaviour 
{

	public GameObject tempMainTile;
	public GameObject tempKetchup;
	public GameObject tempMustard;
	public GameObject tempRelish;

	// INSTEAD OF GENERATING TILES LIKE BELOW, WE CAN READ DATA IN FROM A TEXT FILE TO GENERATE THE LEVELS
	void Start () 
	{
		int x, z;
		z = 0;

		for(int i = 0; i < 10; i++)
		{
			x = 0;

			for(int j = 0; j < 10; j++)
			{
				Instantiate( tempMainTile, new Vector3( x, 0, z), Quaternion.identity );
				x += 25;
			}

			z += 25;
		}

		// Place the ketchup, mustard, and relish
		Instantiate( tempKetchup, new Vector3( 125.0f, 0.0f, 0.0f ), Quaternion.identity );
		Instantiate( tempMustard, new Vector3( 75.0f, 0.0f, 75.0f ), Quaternion.identity );
		Instantiate( tempRelish, new Vector3( 0.0f, 0.0f, 150.0f ), Quaternion.identity );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
