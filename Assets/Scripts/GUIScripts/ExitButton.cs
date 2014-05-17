using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {

	
	public Texture2D Normal ;
	public Texture2D Hover;
	
	void OnMouseEnter(){
		guiTexture.texture = Hover;
	}
	
	void OnMouseExit(){
		guiTexture.texture = Normal;
	}
	
	void OnMouseDown(){
		Application.Quit();
	}
}
