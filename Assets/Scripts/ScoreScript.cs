using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	private int iPlayerScore;
	private float fPlayerTime;
	private bool bTimerActive;

	// Use this for initialization
	void Start () {
		iPlayerScore = 0;
		fPlayerTime = 0.0f;
		bTimerActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (bTimerActive)
			fPlayerTime += Time.deltaTime; //update timer
	}

	//start the timper
	public void StartTimer()
	{
		bTimerActive = true;
	}

	//stop the timer

}
