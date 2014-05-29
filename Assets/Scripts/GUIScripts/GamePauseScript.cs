using UnityEngine;
using System.Collections;

public class GamePauseScript : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		int sWidth, sHeight;
		sWidth = Screen.width;
		sHeight = Screen.height;
		
		GUITexture[] guiTextures = this.GetComponentsInChildren<GUITexture>();
		
		foreach( GUITexture tex in guiTextures )
		{
			if( tex.name == "Background" )
			{
				tex.pixelInset = new Rect( sWidth * 0.5f - tex.texture.width * 0.5f, 
				                          sHeight * 0.5f - tex.texture.height * 0.5f, 
				                          tex.texture.width, tex.texture.height );
			}
			else if( tex.name == "Continue" )
			{
				tex.pixelInset = new Rect( sWidth * 0.5f - tex.texture.width * 0.5f - 5.0f,
				                          sHeight * 0.43f, tex.texture.width * 0.5f, tex.texture.height * 0.5f );
			}
			else
			{
				tex.pixelInset = new Rect( sWidth * 0.5f - 5.0f,
				                          sHeight * 0.43f, tex.texture.width * 0.5f, tex.texture.height * 0.5f );
			}
		}
		
		this.GetComponentInChildren<GUIText>().pixelOffset = new Vector2( sWidth * 0.5f, sHeight * 0.5f + 20.0f );
		this.gameObject.SetActive( false );
	}

	void Start()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}