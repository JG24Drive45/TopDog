using UnityEngine;
using System.Collections;

public class InGameButton : MonoBehaviour 
{
	public delegate void NextLevel();
	public static event NextLevel onNextLevel;
	public static event NextLevel onSpaceDown;

	public delegate void MainMenu();
	public static event MainMenu onMainMenu;

	public delegate void Unpause();
	public static event Unpause onUnpause;

	public Texture2D Normal ;
	public Texture2D Hover;


	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			if( onNextLevel != null )
			{
				onNextLevel();
			}
			else
				Debug.Log("null delegate in InGameButton.cs.");
		}
	}

	void OnMouseEnter(){
		guiTexture.texture = Hover;
	}
	
	void OnMouseExit(){
		guiTexture.texture = Normal;
	}
	
	void OnMouseUp()
	{
		switch( this.gameObject.name )
		{
		case "NextLevel":
			if( onNextLevel != null )
				onNextLevel();
			else
				Debug.Log("null delegate in InGameButton.cs.");
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
	
}
