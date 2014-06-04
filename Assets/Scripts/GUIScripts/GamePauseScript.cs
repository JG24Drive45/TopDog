using UnityEngine;
using System.Collections;

public class GamePauseScript : MonoBehaviour 
{
	public delegate void EscapeToUnpause();
	public static event EscapeToUnpause onEscapeToUnpause;

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
				float texWid = sWidth * 0.25f;
				float texHei = sHeight * 0.2f;
				tex.pixelInset = new Rect( sWidth * 0.5f - texWid * 0.5f,
				                          sHeight * 0.5f - texHei * 0.5f, 
				                          sWidth * 0.25f, sHeight * 0.2f );
			}
			else if( tex.name == "Continue" )
			{
				float texWid = sWidth * 0.125f;
				float texHei = sHeight * 0.05f;
				tex.pixelInset = new Rect( sWidth * 0.5f - texWid,
				                          sHeight * 0.43f, 
				                          texWid, texHei );
			}
			else
			{
				float texWid = sWidth * 0.125f;
				float texHei = sHeight * 0.05f;
				tex.pixelInset = new Rect( sWidth * 0.5f,
				                          sHeight * 0.43f, 
				                          texWid, texHei );
			}
		}
		
		GUIText gText = GetComponentInChildren<GUIText>();
		gText.pixelOffset = new Vector2( sWidth * 0.5f, sHeight * 0.53f );
		int textSize = sWidth / 30;
		gText.fontSize = textSize;
		this.gameObject.SetActive( false );

		HotDogScript.onGamePaused += Activate;
		InGameButton.onUnpause += DeActivate;
	}

	void Start()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			if( onEscapeToUnpause != null )
				onEscapeToUnpause();
			Time.timeScale = 1.0f;
			this.gameObject.SetActive( false );
		}
	}

	void OnDestroy()
	{
		HotDogScript.onGamePaused -= Activate;
		InGameButton.onUnpause -= DeActivate;
	}

	void Activate()
	{
		Time.timeScale = 0.0f;
		this.gameObject.SetActive( true );
	}

	void DeActivate()
	{
		Time.timeScale = 1.0f;
		this.gameObject.SetActive( false );
	}
}