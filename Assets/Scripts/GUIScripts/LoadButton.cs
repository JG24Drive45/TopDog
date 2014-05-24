using UnityEngine;
using System.Collections;

public class LoadButton : MonoBehaviour {
	
	
	public Texture2D Normal ;
	public Texture2D Hover;
	public string LevelToLoad ;
	
	void OnMouseEnter(){
		guiTexture.texture = Hover;
	}
	
	void OnMouseExit(){
		guiTexture.texture = Normal;
	}
	
	void OnMouseUp(){
		EventAggregatorManager.Publish(new LoadLevelMessage(LevelToLoad));
	}
}