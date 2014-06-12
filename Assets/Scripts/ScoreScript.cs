using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreScript : MonoBehaviour {

	private const float TIME_PENALTY_MULTIPLIER = 0.9f;

	public int 				currentLevel;	//holds the current level number
	private static int 		playerScore;	//stores the player's current score
	private int 			playerMoveCount;//number of moves the player has made
	private static float 	playerTime;		//stores the player's current time
	private bool 			timerActive;	//whether or not the timer is currently running
	private string			playerName;		//what name to attribute any high scores earned to
	private bool			nameKnown;		//whether or not we know the player's name

	private static ScoreScript instance = null;

	// Use this for initialization
	void Awake()
	{
		if( instance != null && instance != this )
		{
			Destroy( this.gameObject );
			//reaching here means the player died.  reset stuff
			playerTime = 0.0f; 
			playerScore = 0;
			return;
		}
		else
		{
			instance = this;

		}

		DontDestroyOnLoad(transform.gameObject); //allow this script to retain data between levels
		currentLevel 	= 0;
		playerScore 	= 0;
		playerMoveCount = 0;
		playerTime 		= 0.0f;
		timerActive 	= false;
		playerName		= "mysterious stranger"; 
		nameKnown 		= false; 
	}

	void Start () 
	{

	}

	public void OnEnable()
	{
		HotDogScript.onCondimentAcquired += AcquiredCondiment;
		LevelGeneratorScript.onLevelStart += StartTimer;
		LevelGeneratorScript.onSetLevelNum += setLevel;
		HotDogScript.onLevelComplete += LevelComplete;
		LevelCompleteScript.onNeedScore += getScore;
	}

	public void OnDisable()
	{
		HotDogScript.onCondimentAcquired -= AcquiredCondiment;
		LevelGeneratorScript.onLevelStart -= StartTimer;
		LevelGeneratorScript.onSetLevelNum -= setLevel;
		HotDogScript.onLevelComplete -= LevelComplete;
		LevelCompleteScript.onNeedScore -= getScore;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timerActive)
			if (nameKnown) //dont update timer while waiting for name
				playerTime += Time.deltaTime; //update timer
	}

	//display the information
	void OnGUI()
	{
		//divide time into minutes and seconds
		int iMinutes = (int)playerTime / 60;
		float fSeconds = playerTime - (iMinutes * 60);

		//create label
		GUI.Label( new Rect(0,0,512,128), "Time: " + iMinutes + ":" + fSeconds + "\n" +
		          						  "Score: " + playerScore + "\n" +
		          						  "Moves: " + playerMoveCount + "\n" +
										  "Level: " + currentLevel);

		//prompt for name
		if (nameKnown == false)
		{
			GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100));

			GUI.Box(   	  new Rect(0,  0, 200, 100), "What is your name?");
			playerName = GUI.TextField(new Rect(0,  25, 200, 30 ), playerName);
			if(GUI.Button(new Rect(75, 65, 50, 30 ), "Done"))
			   nameKnown = true;

			GUI.EndGroup();
		}
	}

	void saveScore(string name)
	{
		KeyValuePair<int,string>[] scores = new KeyValuePair<int,string>[11]; //stores the scores and names
		byte i = 0;	//current entry in the array
		string filePath = Application.dataPath + "/Resources/Scores/level_" + currentLevel + "_scores.txt"; //location the scores are saved
		Debug.Log(Application.dataPath + "Assets/Resources/Scores/");
		//make sure score directory exists
		if (!Directory.Exists(Application.dataPath + "/Resources/Scores/"))
	 	{
			Debug.Log("score directory not found.  creating it...");
			DirectoryInfo test = Directory.CreateDirectory(Application.dataPath + "/Resources/Scores/");
		}
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
			Debug.Log("ScoreScript.SaveScore: could not find file " + e.FileName + ".  This message SHOULD be harmless: the script continues regardless.");
		}
		catch (System.IO.IsolatedStorage.IsolatedStorageException e)
		{
			//if the file does not exist, log warning and continue anyway
			Debug.Log("ScoreScript.SaveScore: could not find file " + e.Data + ".  This message SHOULD be harmless: the script continues regardless.");
		}

		//add new score
		scores[i] = new KeyValuePair<int, string>(playerScore, name);
		i++;

		//sort scores using bubble sort
		bool changed = true;
		while (changed)
		{
			changed = false;
			for (byte j = 1; j < i; j++)
				if ( scores[j].Key > scores[j-1].Key )
				{
					KeyValuePair<int, string> temp = scores[j-1];
					scores[j-1] = scores[j];
					scores[j] = temp;
				changed = true;
				}
		}
		//write scores
		StreamWriter output = new StreamWriter(filePath);

		byte stopAt = Math.Min(i, (byte)10); //toss out the 11th entry, if there is one
		for( i = 0; i < stopAt; i++)
		{
			output.WriteLine(scores[i].Key);
			output.WriteLine(scores[i].Value);
		}

		output.Close();
	}

	public KeyValuePair<int,string>[] readScores()
	{
		KeyValuePair<int,string>[] scores = new KeyValuePair<int,string>[11]; //stores the scores and names
		byte i = 0;	//current entry in the array
		string filePath = Application.dataPath + "/Resources/Scores/level_" + currentLevel + "_scores.txt"; //location the scores are saved
		
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
		catch (System.IO.IsolatedStorage.IsolatedStorageException e)
		{
			//if the file does not exist, log warning and continue anyway
			Debug.Log("ScoreScript.SaveScore: could not find file " + e.Data + ".  This message SHOULD be harmless: the script continues regardless.");
		}

		return scores;
	}

	//-----------messages------------//

	void AcquiredCondiment( string color )
	{
		Debug.Log( "Message received" );
		playerScore += 50;
	}

	void IncrementMoveCount()
	{
		playerMoveCount++;
	}

	void StartTimer()
	{
		playerTime = 0.0f; 
		playerScore = 0;
		timerActive = true;
	}

	void StopTimer()
	{
		timerActive = false;
	}

	int getLevel()
	{
		return currentLevel;
	}

	void setLevel(int levelIndex)
	{
		currentLevel = levelIndex;
	}

	void TimeBonus(float fAmount)
	{
		playerTime -= fAmount;
	}

	void LevelComplete(bool bHasAllCondiments)
	{
		StopTimer();

		playerScore += 100; //100 points for winning level

		if (bHasAllCondiments)
			playerScore += 100; //100 more if all condiments were collected

		playerScore -= (int)( Math.Floor(playerTime) * TIME_PENALTY_MULTIPLIER ); // reduce score based on completion time

		saveScore(playerName);
	}

	public static ScoreScript GetInstance
	{
		get
		{
			return instance;
		}
	}

	int getScore()
	{
		return playerScore;
	}

	void setScore(int score)
	{
		playerScore = score;
	}

	int getPlayerMoveCount()
	{
		return playerMoveCount;
	}

	void setPlayerMoveCount(int count)
	{
		playerMoveCount = count;
	}

	float getPlayerTime()
	{
		return playerTime;
	}

	void setPlayerTime(float time)
	{
		playerTime = time;
	}

	bool isTimerActive()
	{
		return timerActive;
	}

	void setPlayerName (string name)
	{
		playerName = name;
	}
}
