using UnityEngine;
using System.Collections;
using System;

public class ScoreScript : MonoBehaviour {

	//note: these variables are only public so they are visible in the inspector.  
	//proper acesss is through the properties

	public int playerScore;	//stores the player's current score
	public int playerMoveCount;//number of moves the player has made
	public float playerTime;	//stores the player's current time
	public bool timerActive;	//whether or not the timer is currently running

	// Use this for initialization
	void Start () 
	{
		playerScore = 0;
		playerTime = 0.0f;
		timerActive = false;
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
		int iMinutes = (int)fPlayerTime / 60;
		float fSeconds = playerTime - (iMinutes * 60);

		//create label
		GUI.Label( new Rect(0,0,512,128), "Time: " + iMinutes + ":" + fSeconds + "\n" +
		          						  "Score: " + playerScore + "\n" +
		          						  "Moves: " + playerMoveCount);
	}

	//-----------accessors-----------//

	public int iPlayerScore
	{
		get
		{
			return playerScore;
		}

		set
		{
			playerScore = Math.Min(value, 0); //prevent negative score
		}
	}

	public int iPlayerMoveCount
	{
		get
		{
			return playerMoveCount;
		}

		set
		{
			playerMoveCount = value;
		}
	}

	public float fPlayerTime
	{
		get
		{
			return playerTime;
		}

		set
		{
			playerTime = Math.Min(value, 0.0f); //prevent negative time
		}
	}

	public bool bIsTimerActive
	{
		get
		{
			return timerActive;
		}

		set
		{
			timerActive = value;
		}
	}

}
