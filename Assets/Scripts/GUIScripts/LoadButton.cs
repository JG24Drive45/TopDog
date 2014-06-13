using UnityEngine;
using System.Collections;

public class LoadButton : MonoBehaviour {

	public int buttonNumber;
	public bool Unlocked ;
	public Texture2D Normal ;
	public Texture2D Hover;
	public Texture2D Gray;
	public string LevelToLoad ;

	void Start () {
	}

	void OnMouseEnter(){
		if (Unlocked) {
			guiTexture.texture = Hover;	
		}
		else {
			guiTexture.texture = Gray;	
		}
	}
		
	void OnMouseExit(){
		if (Unlocked) {
			guiTexture.texture = Normal;
		}
		else {
			guiTexture.texture = Gray;	
		}

	}
		
	void OnMouseUp(){
		if (Unlocked) {
			EventAggregatorManager.Publish (new LoadLevelMessage (LevelToLoad));
		}
	}
	void Update()
	{
		for( int i = 0; i<= 15; i++)
		{
			if( GUIMenus.levelUnlocked >= i)
			{
				if( buttonNumber == i)
				{
					Unlocked = true;
				}
			}
		}
		if(!Unlocked)
		{
			guiTexture.texture = Gray;
		}
	}
}