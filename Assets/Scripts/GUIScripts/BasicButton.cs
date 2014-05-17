using UnityEngine;
using System.Collections;

public class BasicButton : MonoBehaviour {

	
	public Texture2D Normal ;
	public Texture2D Hover;
	public string LevelToLoad ;

	void OnMouseEnter(){
		guiTexture.texture = Hover;
	}
	
	void OnMouseExit(){
		guiTexture.texture = Normal;
	}
	
	void OnMouseDown(){
		Application.LoadLevel (LevelToLoad);
	}
}
