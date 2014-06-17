using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {

	
	public Texture2D Normal ;
	public Texture2D Hover;
	
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
		Application.Quit ();
	}
}
