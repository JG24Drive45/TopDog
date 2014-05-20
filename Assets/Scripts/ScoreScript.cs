using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreScript : MonoBehaviour {

	private const float TIME_PENALTY_MULTIPLIER = 0.9f;

	private int 	currentLevel;	//holds the current level number
	private int 	playerScore;	//stores the player's current score
	private int 	playerMoveCount;//number of moves the player has made
	private float 	playerTime;		//stores the player's current time
	private bool 	timerActive;	//whether or not the timer is currently running
	private string	playerName;		//what name to attribute any high scores earned to

	// Use this for initialization
	void Start () 
	{
		currentLevel 	= 0;
		playerScore 	= 0;
		playerMoveCount = 0;
		playerTime 		= 0.0f;
		timerActive 	= false;
		playerName		= "mysterious stranger"; 
	}

	public void OnEnable()
	{
		//Messenger.AddListener( "acquired condiment", AcquiredCondiment );
		HotDogScript.onCondimentAcquired += AcquiredCondiment;
		Messenger.AddListener( "increment move count", IncrementMoveCount );
		Messenger.AddListener( "start timer", StartTimer );
		Messenger.AddListener( "stop timer", StopTimer );
		Messenger<int>.AddListener( "set level", setLevel );
		Messenger<float>.AddListener( "time bonus", TimeBonus );
		Messenger<bool>.AddListener( "level complete", LevelComplete );
		Messenger<int>.AddListener( "set score", setScore );
		Messenger<int>.AddListener( "set move count", setPlayerMoveCount );
		Messenger<float>.AddListener( "set time", setPlayerTime );
		Messenger<string>.AddListener( "set player name", setPlayerName );
	}

	public void OnDisable()
	{
		//Messenger.RemoveListener( "acquired condiment", AcquiredCondiment );
		HotDogScript.onCondimentAcquired -= AcquiredCondiment;
		Messenger.RemoveListener( "increment move count", IncrementMoveCount );
		Messenger.RemoveListener( "start timer", StartTimer );
		Messenger.RemoveListener( "stop timer", StopTimer );
		Messenger<int>.RemoveListener( "set level", setLevel );
		Messenger<float>.RemoveListener( "time bonus", TimeBonus );
		Messenger<bool>.RemoveListener( "level complete", LevelComplete );
		Messenger<int>.RemoveListener( "set score", setScore );
		Messenger<int>.RemoveListener( "set move count", setPlayerMoveCount );
		Messenger<float>.RemoveListener( "set time", setPlayerTime );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timerActive)
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
	}

	void saveScore(string name)
	{
		KeyValuePair<int,string>[] scores = new KeyValuePair<int,string>[11]; //stores the scores and names
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

		return scores;
	}

	//-----------messages------------//

	void AcquiredCondiment()
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
		playerScore += 100; //100 points for winning level

		if (bHasAllCondiments)
			playerScore += 100; //100 more if all condiments were collected

		playerScore -= (int)( Math.Floor(playerTime) * TIME_PENALTY_MULTIPLIER ); // reduce score based on completion time

		saveScore(playerName);
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
