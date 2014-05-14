using UnityEngine;
using System.Collections;
using System;

public class ScoreScript : MonoBehaviour {

	private int currentLevel;	//holds the current level number
	private int playerScore;	//stores the player's current score
	private int playerMoveCount;//number of moves the player has made
	private float playerTime;	//stores the player's current time
	private bool timerActive;	//whether or not the timer is currently running

	// Use this for initialization
	void Start () 
	{
		playerScore = 0;
		playerTime = 0.0f;
		timerActive = false;
	}

	public void OnEnable()
	{
		Messenger.AddListener( "acquired condiment", AcquiredCondiment );
		Messenger.AddListener( "increment move count", IncrementMoveCount );
		Messenger.AddListener( "start timer", StartTimer );
		Messenger.AddListener( "stop timer", StopTimer );
		Messenger<int>.AddListener( "set level", setLevel );
		Messenger<float>.AddListener( "time bonus", TimeBonus );
		Messenger<bool>.AddListener( "level complete", LevelComplete );
		Messenger<int>.AddListener( "set score", setScore );
		Messenger<int>.AddListener( "set move count", setPlayerMoveCount );
		Messenger<float>.AddListener( "set time", setPlayerTime );
	}

	public void OnDisable()
	{
		Messenger.RemoveListener( "acquired condiment", AcquiredCondiment );
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
		          						  "Moves: " + playerMoveCount);
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
}
