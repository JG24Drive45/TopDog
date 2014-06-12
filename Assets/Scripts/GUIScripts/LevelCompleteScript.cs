using UnityEngine;
using System.Collections;

public class LevelCompleteScript : MonoBehaviour 
{
	public delegate int NeedScore();
	public static event NeedScore onNeedScore;

	public delegate void NextLevel();
	public static event NextLevel onSpace;


	int score = 0;

	void Awake()
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
			else if( tex.name == "NextLevel" )
			{
				float texWid = sWidth * 0.125f;
				float texHei = sHeight * 0.05f;
				tex.pixelInset = new Rect( sWidth * 0.5f - texWid,
				                          sHeight * 0.42f, 
				                          texWid, texHei );
			}
			else
			{
				float texWid = sWidth * 0.125f;
				float texHei = sHeight * 0.05f;
				tex.pixelInset = new Rect( sWidth * 0.5f,
				                          sHeight * 0.42f, 
				                          texWid, texHei );
			}
		}
		
		GUIText gText = this.GetComponentInChildren<GUIText>();
		gText.pixelOffset = new Vector2( sWidth * 0.5f, sHeight * 0.52f );
		int textSize = sWidth / 30;
		gText.fontSize = textSize;
		gText.text = "Level Complete\nScore: " + score;

		this.gameObject.SetActive( false );

		HotDogScript.onLevelComplete += Activate;
	}
	

	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			LevelGeneratorScript.CallLoadLevelMessage = true;
			if( onSpace != null )
			{
				int curLevel = int.Parse(LevelGeneratorScript.sLevel.Substring(5));
				if( curLevel == Application.levelCount - 1 )
				{
					LevelGeneratorScript.CallLoadLevelMessage = false;
				}
				
				onSpace();
			}

		}
	}

	void OnGUI()
	{
	
	}

	void OnDestroy()
	{
		HotDogScript.onLevelComplete -= Activate;
	}
	
	void Activate(bool bActive)
	{
		this.gameObject.SetActive( true );

		if( onNeedScore != null )
			score = onNeedScore();

		this.GetComponentInChildren<GUIText>().text = "Level Complete\nScore: " + score;
	}
}