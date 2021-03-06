﻿using UnityEngine;
using System.Collections;

public class InGameButton : MonoBehaviour 
{
	public delegate void NextLevel();
	public static event NextLevel onNextLevel;

	public delegate void MainMenu();
	public static event MainMenu onMainMenu;

	public delegate void Unpause();
	public static event Unpause onUnpause;

	public Texture2D Normal ;
	public Texture2D Hover;


	void Update()
	{

	}

	void OnMouseEnter(){
		guiTexture.texture = Hover;
		EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
	}
	
	void OnMouseExit(){
		guiTexture.texture = Normal;
	}

	void OnEnable()
	{
		LevelCompleteScript.onSpace += SpacePushed;
	}

	void OnDestroy()
	{
		LevelCompleteScript.onSpace -= SpacePushed;
	}

	
	void OnMouseUp()
	{
		switch( this.gameObject.name )
		{
		case "NextLevel":
			if( onNextLevel != null )
			{
				onNextLevel();
			}
			break;

		case "MainMenu":
			if( onMainMenu != null )
				onMainMenu();
			break;

		case "Continue":
			if( onUnpause != null )
				onUnpause();
			break;
		}
	}

	void SpacePushed()
	{
		if (onNextLevel != null )
		{
			onNextLevel();
		}
	}

}
