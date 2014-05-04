using UnityEngine;
using System.Collections;

public class LevelGeneratorScript : MonoBehaviour 
{

	public GameObject tempMainTile;

	// INSTEAD OF GENERATING TILES LIKE BELOW, WE CAN READ DATA IN FROM A TEXT FILE TO GENERATE THE LEVELS
	void Start () 
	{
		int x, z;
		z = 0;

		for(int i = 0; i < 5; i++)
		{
			x = 0;

			for(int j = 0; j < 5; j++)
			{
				Instantiate( tempMainTile, new Vector3( x, 0, z), Quaternion.identity );
				x += 25;
			}

			z += 25;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
