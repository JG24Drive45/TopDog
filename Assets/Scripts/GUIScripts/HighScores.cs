using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScores : MonoBehaviour {
	public GUIStyle style ;

	int currentLevel = 1 ;
	int totalLevels = 15 ;
	int sWidth, sHeight;

	string level1HS ;
	string level2HS ;
	string level3HS ;
	string level4HS ;
	string level5HS ;
	string level6HS ;
	string level7HS ;
	string level8HS ;
	string level9HS ;
	string level10HS ;
	string level11HS ;
	string level12HS ;
	string level13HS ;
	string level14HS ;
	string level15HS ;

	private KeyValuePair<int,string>[] scores ;
	// Use this for initialization
	void Start () {

		sWidth = Screen.width;
		sHeight = Screen.height;
		for( int k = 0; k < totalLevels; k++ )
		{
			scores = new KeyValuePair<int,string>[11]; //stores the scores and names
			byte i = 0;	//current entry in the array
			string filePath = "Assets/Resources/Scores/level_" + currentLevel + "_scores.txt"; //location the scores are saved
			
			//read scores
			try
			{
				StreamReader input = new StreamReader(filePath);
				
				while (input.EndOfStream == false)
				{
					int key = Convert.ToInt32( input.ReadLine() );
					string value = input.ReadLine();
					
					scores[i] = new KeyValuePair<int, string>(key,value);
					i++;
				}
				
				input.Close();
			}
			catch (FileNotFoundException e)
			{
				//if the file does not exist, log warning and continue anyway
				Debug.LogWarning("ScoreScript.SaveScore: could not find file " + e.FileName + ".  This message SHOULD be harmless: the script continues regardless.");
			}
			currentLevel++;
			string temp ;
			temp = scores [0].ToString ();
			temp = temp.Substring (1);
			temp = temp.Replace ("]", "");
			// I wanted to do this as an array of strings but for some reason I couldn't
			// but this works
			switch(k){
			case 0:
				level1HS = temp ;
				break;
			case 1:
				level2HS = temp ;
				break;
			case 2:
				level3HS = temp ;
				break;
			case 3:
				level4HS = temp ;
				break;
			case 4:
				level5HS = temp ;
				break;
			case 5:
				level6HS = temp ;
				break;
			case 6:
				level7HS = temp ;
				break;
			case 7:
				level8HS = temp ;
				break;
			case 8:
				level9HS = temp ;
				break;
			case 9:
				level10HS = temp ;
				break;
			case 10:
				level11HS = temp ;
				break;
			case 11:
				level12HS = temp ;
				break;
			case 12:
				level13HS = temp ;
				break;
			case 13:
				level14HS = temp ;
				break;
			case 14:
				level15HS = temp ;
				break;
			}

		}

	
	}

	void OnGUI()
	{
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.13f, 
		                    200, 80 ), "Level 1   " + level1HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.16f, 
		                    200, 80 ), "Level 2   " + level2HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.19f, 
		                    200, 80 ), "Level 3   " + level3HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.22f, 
		                    200, 80 ), "Level 4   " + level4HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.25f, 
		                    200, 80 ), "Level 5   " + level5HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.28f, 
		                    200, 80 ), "Level 6   " + level6HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.31f, 
		                    200, 80 ), "Level 7   " + level7HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.34f, 
		                    200, 80 ), "Level 8   " + level8HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.37f, 
		                    200, 80 ), "Level 9   " + level9HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.4f, 
		                    200, 80 ), "Level 10  " + level10HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.43f, 
		                    200, 80 ), "Level 11  " + level11HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.46f, 
		                    200, 80 ), "Level 12  " + level12HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.49f, 
		                    200, 80 ), "Level 13  " + level13HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.52f, 
		                    200, 80 ), "Level 14  " + level14HS, style );
		GUI.Label( new Rect( sWidth * 0.4f , sHeight * 0.55f, 
		                    200, 80 ), "Level 15  " + level15HS, style );

	}

	// Update is called once per frame
	void Update () {
	
	}
}
