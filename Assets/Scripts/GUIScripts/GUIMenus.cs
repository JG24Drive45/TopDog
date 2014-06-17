using UnityEngine;
using System.Collections;

public class GUIMenus : MonoBehaviour {
	public enum GUIState{ TITLE, MAINMENU, INSTRUCTIONS, HIGHSCORES, CREDITS, LEVELSELECT };
	public static GUIState menuState ;
	public static int levelUnlocked ;

	public Texture2D mainMenu;
	public Texture2D instructions;
	public Texture2D highscores;
	public Texture2D credits;
//	public Texture2D options;
	public Texture2D title;
	public Texture2D levelSelect;

	public GameObject background;
	public GameObject playBtn;
	public GameObject instructionsBtn;
	public GameObject highScoresBtn;
	public GameObject highScoresObject;
	public GameObject creditsBtn;
//	public GameObject optionsBtn;
	public GameObject exitBtn;
	public GameObject backBtn;
	public GameObject menuBtn;
	public GameObject titleExitBtn;

	// buttons for loading each level
	public GameObject oneBtn;
	public GameObject twoBtn;
	public GameObject threeBtn;
	public GameObject fourBtn;
	public GameObject fiveBtn;
	public GameObject sixBtn;
	public GameObject sevenBtn;
	public GameObject eightBtn;
	public GameObject nineBtn;
	public GameObject tenBtn;
	public GameObject elevenBtn;
	public GameObject twelveBtn;
	public GameObject thirteenBtn;
	public GameObject fourteenBtn;
	public GameObject fifteenBtn;
	public GameObject sixteenBtn;
	public GameObject seventeenBtn;
	public GameObject eightteenBtn;


	// Use this for initialization
	void Start () {
//		menuState = GUIState.TITLE;
		background.guiTexture.texture = title;
		playBtn.SetActive (false);
		instructionsBtn.SetActive (false);
		highScoresBtn.SetActive (false);
		highScoresObject.SetActive (false);
		creditsBtn.SetActive (false);
//		optionsBtn.SetActive (false);
		exitBtn.SetActive (false);
		backBtn.SetActive (false);
		menuBtn.SetActive (true);
		titleExitBtn.SetActive (true);

		oneBtn.SetActive (false);
		twoBtn.SetActive (false);
		threeBtn.SetActive (false);
		fourBtn.SetActive (false);
		fiveBtn.SetActive (false);
		sixBtn.SetActive (false);
		sevenBtn.SetActive (false);
		eightBtn.SetActive (false);
		nineBtn.SetActive (false);
		tenBtn.SetActive (false);
		elevenBtn.SetActive (false);
		twelveBtn.SetActive (false);
		thirteenBtn.SetActive (false);
		fourteenBtn.SetActive (false);
		fifteenBtn.SetActive (false);
		sixteenBtn.SetActive (false);		
		seventeenBtn.SetActive (false);
		eightteenBtn.SetActive (false);

		int sWidth, sHeight;
		sWidth = Screen.width;
		sHeight = Screen.height;


		float texWid = sWidth * 0.5f;
		float texHei = sHeight * 0.5f;
		backBtn.guiTexture.pixelInset = new Rect( sWidth * 0.5f - texWid * 0.93f,
		                   							sHeight * 0.5f - texHei * 1.25f, 
			                                   		sWidth * 0.25f, sHeight * 0.07f );
			//pixelInset = new Rect( sWidth * 0.5f - texWid * 0.5f,
		        //                  sHeight * 0.5f - texHei * 0.5f, 
		        //                  sWidth * 0.25f, sHeight * 0.2f );

	}


	// Update is called once per frame
	void Update () {
	switch (menuState) {
		case GUIState.CREDITS:
			background.guiTexture.texture = credits;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			highScoresObject.SetActive (false);
			creditsBtn.SetActive (false);
//			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);

			oneBtn.SetActive (false);
			twoBtn.SetActive (false);
			threeBtn.SetActive (false);
			fourBtn.SetActive (false);
			fiveBtn.SetActive (false);
			sixBtn.SetActive (false);
			sevenBtn.SetActive (false);
			eightBtn.SetActive (false);
			nineBtn.SetActive (false);
			tenBtn.SetActive (false);
			elevenBtn.SetActive (false);
			twelveBtn.SetActive (false);
			thirteenBtn.SetActive (false);
			fourteenBtn.SetActive (false);
			fifteenBtn.SetActive (false);
			sixteenBtn.SetActive (false);		
			seventeenBtn.SetActive (false);
			eightteenBtn.SetActive (false);
			break;

		case GUIState.HIGHSCORES:
			background.guiTexture.texture = highscores;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			highScoresObject.SetActive (true);
			creditsBtn.SetActive (false);
//			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);

			oneBtn.SetActive (false);
			twoBtn.SetActive (false);
			threeBtn.SetActive (false);
			fourBtn.SetActive (false);
			fiveBtn.SetActive (false);
			sixBtn.SetActive (false);
			sevenBtn.SetActive (false);
			eightBtn.SetActive (false);
			nineBtn.SetActive (false);
			tenBtn.SetActive (false);
			elevenBtn.SetActive (false);
			twelveBtn.SetActive (false);
			thirteenBtn.SetActive (false);
			fourteenBtn.SetActive (false);
			fifteenBtn.SetActive (false);
			sixteenBtn.SetActive (false);		
			seventeenBtn.SetActive (false);
			eightteenBtn.SetActive (false);
			break;

		case GUIState.INSTRUCTIONS:

			background.guiTexture.texture = instructions;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			highScoresObject.SetActive (false);
			creditsBtn.SetActive (false);
//			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);

			oneBtn.SetActive (false);
			twoBtn.SetActive (false);
			threeBtn.SetActive (false);
			fourBtn.SetActive (false);
			fiveBtn.SetActive (false);
			sixBtn.SetActive (false);
			sevenBtn.SetActive (false);
			eightBtn.SetActive (false);
			nineBtn.SetActive (false);
			tenBtn.SetActive (false);
			elevenBtn.SetActive (false);
			twelveBtn.SetActive (false);
			thirteenBtn.SetActive (false);
			fourteenBtn.SetActive (false);
			fifteenBtn.SetActive (false);
			sixteenBtn.SetActive (false);		
			seventeenBtn.SetActive (false);
			eightteenBtn.SetActive (false);
			break;

		case GUIState.MAINMENU:
			background.guiTexture.texture = mainMenu;
			playBtn.SetActive (true);
			instructionsBtn.SetActive (true);
			highScoresBtn.SetActive (true);
			highScoresObject.SetActive (false);
			creditsBtn.SetActive (true);
//			optionsBtn.SetActive (true);
			exitBtn.SetActive (true);
			backBtn.SetActive (false);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);

			oneBtn.SetActive (false);
			twoBtn.SetActive (false);
			threeBtn.SetActive (false);
			fourBtn.SetActive (false);
			fiveBtn.SetActive (false);
			sixBtn.SetActive (false);
			sevenBtn.SetActive (false);
			eightBtn.SetActive (false);
			nineBtn.SetActive (false);
			tenBtn.SetActive (false);
			elevenBtn.SetActive (false);
			twelveBtn.SetActive (false);
			thirteenBtn.SetActive (false);
			fourteenBtn.SetActive (false);
			fifteenBtn.SetActive (false);
			sixteenBtn.SetActive (false);		
			seventeenBtn.SetActive (false);
			eightteenBtn.SetActive (false);
			break;

//		case GUIState.OPTIONS:
//			background.guiTexture.texture = options;
//			playBtn.SetActive (false);
//			instructionsBtn.SetActive (false);
//			highScoresBtn.SetActive (false);
//			highScoresObject.SetActive (false);
//			creditsBtn.SetActive (false);
//			optionsBtn.SetActive (false);
//			exitBtn.SetActive (false);
//			backBtn.SetActive (true);
//			menuBtn.SetActive (false);
//			titleExitBtn.SetActive (false);

//			oneBtn.SetActive (false);
//			twoBtn.SetActive (false);
//			threeBtn.SetActive (false);
//			fourBtn.SetActive (false);
//			fiveBtn.SetActive (false);
//			sixBtn.SetActive (false);
//			sevenBtn.SetActive (false);
//			eightBtn.SetActive (false);
//			nineBtn.SetActive (false);
//			tenBtn.SetActive (false);
//			elevenBtn.SetActive (false);
//			twelveBtn.SetActive (false);
//			thirteenBtn.SetActive (false);
//			fourteenBtn.SetActive (false);
//			fifteenBtn.SetActive (false);
//			break;

		case GUIState.LEVELSELECT:
			background.guiTexture.texture = levelSelect;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			highScoresObject.SetActive (false);
			creditsBtn.SetActive (false);
//			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);


		

			oneBtn.SetActive (true);
			twoBtn.SetActive (true);
			threeBtn.SetActive (true);
			fourBtn.SetActive (true);
			fiveBtn.SetActive (true);
			sixBtn.SetActive (true);
			sevenBtn.SetActive (true);
			eightBtn.SetActive (true);
			nineBtn.SetActive (true);
			tenBtn.SetActive (true);
			elevenBtn.SetActive (true);
			twelveBtn.SetActive (true);
			thirteenBtn.SetActive (true);
			fourteenBtn.SetActive (true);
			fifteenBtn.SetActive (true);
			sixteenBtn.SetActive (true);		
			seventeenBtn.SetActive (true);
			eightteenBtn.SetActive (true);
			break;
			
		}
	
	}
}
