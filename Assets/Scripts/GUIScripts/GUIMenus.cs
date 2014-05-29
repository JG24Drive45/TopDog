﻿using UnityEngine;
using System.Collections;

public class GUIMenus : MonoBehaviour {
	public enum GUIState{ TITLE, MAINMENU, INSTRUCTIONS, HIGHSCORES, CREDITS, OPTIONS };
	public static GUIState menuState ;

	public Texture2D mainMenu;
	public Texture2D instructions;
	public Texture2D highscores;
	public Texture2D credits;
	public Texture2D options;
	public Texture2D title;

	public GameObject background;
	public GameObject playBtn;
	public GameObject instructionsBtn;
	public GameObject highScoresBtn;
	public GameObject creditsBtn;
	public GameObject optionsBtn;
	public GameObject exitBtn;
	public GameObject backBtn;
	public GameObject menuBtn;
	public GameObject titleExitBtn;

	// Use this for initialization
	void Start () {

		menuState = GUIState.TITLE;
		background.guiTexture.texture = title;
		playBtn.SetActive (false);
		instructionsBtn.SetActive (false);
		highScoresBtn.SetActive (false);
		creditsBtn.SetActive (false);
		optionsBtn.SetActive (false);
		exitBtn.SetActive (false);
		backBtn.SetActive (false);
		menuBtn.SetActive (true);
		titleExitBtn.SetActive (true);
	}


	// Update is called once per frame
	void Update () {
	switch (menuState) {
		case GUIState.CREDITS:
			background.guiTexture.texture = credits;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			creditsBtn.SetActive (false);
			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);
			break;

		case GUIState.HIGHSCORES:
			background.guiTexture.texture = highscores;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			creditsBtn.SetActive (false);
			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);
			break;

		case GUIState.INSTRUCTIONS:
			background.guiTexture.texture = instructions;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			creditsBtn.SetActive (false);
			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);
			break;

		case GUIState.MAINMENU:
			background.guiTexture.texture = mainMenu;
			playBtn.SetActive (true);
			instructionsBtn.SetActive (true);
			highScoresBtn.SetActive (true);
			creditsBtn.SetActive (true);
			optionsBtn.SetActive (true);
			exitBtn.SetActive (true);
			backBtn.SetActive (false);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);
			break;

		case GUIState.OPTIONS:
			background.guiTexture.texture = options;
			playBtn.SetActive (false);
			instructionsBtn.SetActive (false);
			highScoresBtn.SetActive (false);
			creditsBtn.SetActive (false);
			optionsBtn.SetActive (false);
			exitBtn.SetActive (false);
			backBtn.SetActive (true);
			menuBtn.SetActive (false);
			titleExitBtn.SetActive (false);
			break;
		}
	
	}
}