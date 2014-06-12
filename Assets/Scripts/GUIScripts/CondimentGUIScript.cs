using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CondimentGUIScript : MonoBehaviour 
{
	public Texture2D satTex;
	public Texture2D redTex;
	public Texture2D yellowTex;
	public Texture2D greenTex;

	public List<GUITexture> children = new List<GUITexture>();

	void Awake()
	{
		//Find any CondimentGUIs and destroy them
		GameObject[] toDestroy = GameObject.FindGameObjectsWithTag( "CondimentGUI" );
		foreach( GameObject go in toDestroy )
		{
			if( go != this.gameObject )
			{
				Destroy( go );
			}
		}

		int screenWid, screenHei;
		screenWid = Screen.width;
		screenHei = Screen.height;

		float texWid, texHei;
		texWid = (1.0f / 6.0f ) * screenWid;
		texHei = (1.0f / 9.5f ) * screenHei;

		guiTexture.pixelInset = new Rect( -texWid, -texHei, texWid, texHei );

		// Get all of the condiment textures and resize them
		GUITexture[] texes = this.GetComponentsInChildren<GUITexture>();
		foreach( GUITexture tex in texes )
		{
			children.Add( tex );
		}

		children.RemoveAt(0);

		// Set all textures to the saturated one
		foreach( GUITexture tex in children )
		{
			tex.guiTexture.texture = satTex;
		}

		Rect pInset = guiTexture.pixelInset;

		// red
		children[1].pixelInset = new Rect( pInset.x * 0.95f, pInset.y, pInset.width * 0.25f, pInset.height * 0.9f );
		// yellow
		children[2].pixelInset = new Rect( pInset.x * 0.625f,  pInset.y, pInset.width * 0.25f, pInset.height * 0.9f );
		// green
		children[0].pixelInset = new Rect( pInset.x * 0.3f,  pInset.y, pInset.width * 0.25f, pInset.height * 0.9f );
	}

	void Start () 
	{
		HotDogScript.onCondimentAcquired += ChangeTexture;
		LevelCompleteScript.onSpace += ResetTextures;
		InGameButton.onNextLevel += ResetTextures;
	}

	void OnDestroy()
	{
		HotDogScript.onCondimentAcquired -= ChangeTexture;
		LevelCompleteScript.onSpace -= ResetTextures;
		InGameButton.onNextLevel -= ResetTextures;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void ChangeTexture( string color )
	{
		switch( color )
		{
		case "Red":
			children[1].texture = redTex;
			break;
		case "Yellow":
			children[2].texture = yellowTex;
			break;
		case "Green":
			children[0].texture = greenTex;
			break;
		}
	}

	void ResetTextures()
	{
		foreach( GUITexture tex in children )
		{
			tex.texture = satTex;
		}
	}
}
