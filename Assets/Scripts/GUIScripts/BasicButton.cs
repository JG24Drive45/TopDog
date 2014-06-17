using UnityEngine;
using System.Collections;

public class BasicButton : MonoBehaviour {

//	public enum GUIState{ TITLE, MAINMENU, INSTRUCTIONS, HIGHSCORES, CREDITS, OPTIONS };	
	public Texture2D Normal ;
	public Texture2D Hover;
	public string state ;

	void OnMouseEnter(){
		guiTexture.texture = Hover;
		EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
	}
	
	void OnMouseExit(){
		guiTexture.texture = Normal;
	}
	void OnMouseDown(){
		EventAggregatorManager.Publish( new PlaySoundMessage( "goal", false ) );
	}

	void OnMouseUp(){
		if (state == "CREDITS") {
						GUIMenus.menuState = GUIMenus.GUIState.CREDITS;
				}
		else if (state == "INSTRUCTIONS") {
			GUIMenus.menuState = GUIMenus.GUIState.INSTRUCTIONS;
		}
		else if (state == "HIGHSCORES") {
			GUIMenus.menuState = GUIMenus.GUIState.HIGHSCORES;
		}
//		else if (state == "OPTIONS") {
//			GUIMenus.menuState = GUIMenus.GUIState.OPTIONS;
//		}
		else if (state == "MAINMENU") {
			GUIMenus.menuState = GUIMenus.GUIState.MAINMENU;
		}
		else if (state == "LEVELSELECT") {
			GUIMenus.menuState = GUIMenus.GUIState.LEVELSELECT;
		}
		guiTexture.texture = Normal;
	}

}
