using UnityEngine;
using System.Collections;

public class InGameButton : MonoBehaviour 
{
	public delegate void NextLevel();
	public static event NextLevel onNextLevel;

	public delegate void MainMenu();
	public static event MainMenu onMainMenu;

	public Texture2D Normal ;
	public Texture2D Hover;
	
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
			break;
		case "MainMenu":
			if( onMainMenu != null )
				onMainMenu();
			break;
		}
	}
	
}
