using UnityEngine;
using System.Collections;
using System.IO;
//using UnityEditor;

public class LevelGeneratorScript : MonoBehaviour,
    IHandle<DestroyLevelMessage>
{
	
	public delegate void LevelStart();
	public delegate void SetLevelNum( int num );
	public delegate void SetPlayerOrientationState( int state );

	public static event LevelStart onLevelStart;
	public static event SetLevelNum onSetLevelNum;
	public static event SetPlayerOrientationState onSetOState;


	// GameObject variables
	public GameObject mainTile;
	public GameObject emptyTile;
	public GameObject teleporterTile;
	public GameObject bridgeTile;
	public GameObject fallingTile;
	public GameObject conveyorBelt;
	public GameObject goalTile;
	public GameObject ketchup;
	public GameObject mustard;
	public GameObject relish;
	public GameObject player;
	public GameObject switchObject;
	public GameObject coals;
	public GameObject pauseMenu;
	public GameObject levelCompleteMenu;

    void SetParents()
    {
    }

	public static string sLevel;											// Name of the current level
	private int iLevelNum;											// Current level number
    public static bool CallLoadLevelMessage = false;
	#region Awake()
    void Awake()
    {

    }
	#endregion

	#region Start()

	// INSTEAD OF GENERATING TILES LIKE BELOW, WE CAN READ DATA IN FROM A TEXT FILE TO GENERATE THE LEVELS
	void Start () 
	{
        GameEventAggregator.GameMessenger.Subscribe(this);
		#region Load in Level Data
        //sLevel = System.IO.Path.GetFileNameWithoutExtension(EditorApplication.currentScene);						// Get the name of the current level
        Debug.Log(sLevel);
        iLevelNum = int.Parse(sLevel.Substring(5));				// Level number for the score script

		if( onSetLevelNum != null )
			onSetLevelNum( iLevelNum );

		string[] lines;												// Array that stores text file info
		TextAsset data;												// Text file variable
		data = ( TextAsset )Resources.Load( "LevelData/" + sLevel );
		lines = data.text.Split( "\n"[0] );

		// Read lines 3 - 14 of text file that make up the tiles
        SetParents();
		for( int i = 2; i < 14; i++ )
		{
			int lineLength = lines[i].Length;
			for( int j = 0; j < lineLength; j++ )
			{
				char temp = lines[i][j];
				if( temp == 'E' )
					(Instantiate( emptyTile, new Vector3( j * 50, 7.5f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 0, 0, 0 ) ) ) as GameObject).transform.parent = this.transform;
				else if( temp == 'M' )
					(Instantiate( mainTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == 'G' )
					(Instantiate( goalTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == 'T' )
					(Instantiate( teleporterTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == 'B' )
				{
                    (Instantiate(emptyTile, new Vector3(j * 50, 0.0f, -(i - 2) * 50), Quaternion.Euler(new Vector3(90, 0, 0))) as GameObject).transform.parent = this.transform;
					(Instantiate( bridgeTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
				}
				else if( temp == 'F' )
					(Instantiate( fallingTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == '1' )
					(Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == '2' )
					(Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 90, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == '3' )
					(Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 180, 0 ) ) )as GameObject).transform.parent = this.transform;
				else if( temp == '4' )
					(Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 270, 0 ) ) )as GameObject).transform.parent = this.transform;
			}
		}

		// Read lines 17 & 19 to get the ketchup location
		Vector3 tempPosition = new Vector3( int.Parse( lines[16] ), 12.5f, -int.Parse( lines[17] ) );
		//Debug.Log( "Ketchup is at: " + tempPosition );			// For testing
		(Instantiate( ketchup, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) )as GameObject).transform.parent = this.transform;

		// Read lines 20 & 21 to get the mustard location
		tempPosition = new Vector3( int.Parse( lines[19] ) , 12.5f, -int.Parse( lines[20] ) );
		//Debug.Log( "Mustard is at: " + tempPosition );			// For testing
		(Instantiate( mustard, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) )as GameObject).transform.parent = this.transform;

		// Read lines 23 & 24 to get the relish location
		tempPosition = new Vector3( int.Parse( lines[22] ), 12.5f, -int.Parse( lines[23] ) );
		//Debug.Log( "Relish is at: " + tempPosition );			// For testing
		(Instantiate( relish, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) )as GameObject).transform.parent = this.transform;

		// Read lines 26 & 27 to get the switch location
		if( lines[25] != "none\r" )
		{
			//Debug.Log( "Line 25 is: " + lines[25] );
			tempPosition = new Vector3( int.Parse( lines[25] ), 12.5f, -int.Parse( lines[26] ) );
			//Debug.Log( "Switch is at: " + tempPosition );
			(Instantiate( switchObject, tempPosition, Quaternion.Euler( new Vector3( 270.0f, 0.0f, 0.0f ) ) )as GameObject).transform.parent = this.transform;
		}

		// Read lines 29 - 31 to get the player's (hotdog) location
		tempPosition = new Vector3( int.Parse( lines[28] ), int.Parse( lines[29] ), -int.Parse( lines[30] ) );
		//Debug.Log( "Player is at: " + tempPosition );
		// Read lines 33 - 35 to get the player's (hotdog) rotation
		Vector3 tempRotation = new Vector3( int.Parse( lines[32] ), int.Parse( lines[33] ), int.Parse( lines[34] ) );
		//Debug.Log( "Player rotation is: " + tempRotation );
		(Instantiate( player, tempPosition, Quaternion.Euler( tempRotation ) )as GameObject).transform.parent = this.transform;

		// Read line 37 to get the player's orientation state
		int tempState = int.Parse( lines[36] );
		//Debug.Log( "OState is: " + tempState );
		// Send the message to set the player's orientation state
		//Messenger<int>.Broadcast( "set player original orientation state", tempState );
		if( onSetOState != null )
			onSetOState( tempState );
		#endregion

		// Instantiate the coals
		(Instantiate( coals, new Vector3( 0, -250, 250 ), Quaternion.identity )as GameObject).transform.parent = this.transform;

		// Make more empties across top
		for( int i = 0; i < 19; i++ )
		{
			(Instantiate( emptyTile, new Vector3( i * 50 - 50, 7.5f, 50 ), Quaternion.Euler( new Vector3( 0, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
		}
		// Make more empties along the bottom
		for( int i = 0; i < 19; i++ )
		{
			(Instantiate( emptyTile, new Vector3( i * 50 - 50, 7.5f, -600 ), Quaternion.Euler( new Vector3( 0, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
		}
		// Make more along the left
		for( int i = 0; i < 12; i++ )
		{
			(Instantiate( emptyTile, new Vector3( -50, 7.5f, -(i * 50) ), Quaternion.Euler( new Vector3( 0, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
		}
		// Make more along the right
		for( int i = 0; i < 12; i++ )
		{
			(Instantiate( emptyTile, new Vector3( 850, 7.5f, -(i * 50) ), Quaternion.Euler( new Vector3( 0, 0, 0 ) ) )as GameObject).transform.parent = this.transform;
		}

		(Instantiate( pauseMenu, new Vector3( 0, 0, 0), Quaternion.identity) as GameObject).transform.parent = this.transform;
		(Instantiate( levelCompleteMenu, new Vector3( 0, 0, 0), Quaternion.identity) as GameObject ).transform.parent = this.transform;
        
        //LoadSounds();

		if( onLevelStart != null )
			onLevelStart();

		Time.timeScale = 1.0f;
	}
	#endregion

    void OnDestroy()
    {
        GameEventAggregator.GameMessenger.Unsubscribe(this);
        Debug.Log("Destroyed Level");
        EventAggregatorManager.Publish(new LevelIsDestroyedMessage());
        if(CallLoadLevelMessage)
		{
            EventAggregatorManager.Publish(new LoadLevelMessage("Level" + iLevelNum.ToString() ));
			Debug.Log( "Got to here" );
		}
    }
	

	#region OnEnable()
	void OnEnable()
	{
		InGameButton.onNextLevel += LoadNextLevel;
		InGameButton.onMainMenu += LoadMainMenu;
	}
	#endregion

	#region OnDisable()
	void OnDisable()
	{
		InGameButton.onNextLevel -= LoadNextLevel;
		InGameButton.onMainMenu -= LoadMainMenu;
	}
	#endregion
	
	#region void Update()
	void Update () 
	{
        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            //Debug.Log("Key Pressed");
            EventAggregatorManager.Publish(new PlaySoundMessage("lvlMusic", true));
        }
        if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            //Debug.Log("Key Pressed");
            EventAggregatorManager.Publish(new StopSoundLoopMessage("lvlMusic"));
        }

	}
	#endregion

	#region void LoadNextLevel()
	void LoadNextLevel()
	{
		iLevelNum++;
		if( iLevelNum < Application.levelCount )
		{
			EventAggregatorManager.Publish(new LoadLevelMessage("Level" + iLevelNum.ToString()));
			Destroy(transform.gameObject);
		}
		else
		{
			Destroy(transform.gameObject);
			GUIMenus.menuState = GUIMenus.GUIState.MAINMENU;
			Application.LoadLevel( "MenuScreen" );
		}
	}
	#endregion

	void LoadMainMenu()
	{
        //EventAggregatorManager.Publish(new LoadLevelMessage("Level" + iLevelNum.ToString()));
        Destroy(transform.gameObject);
		GUIMenus.menuState = GUIMenus.GUIState.MAINMENU;
		Application.LoadLevel( "MenuScreen" );
	}

    public void Handle(DestroyLevelMessage message)
    {
        Destroy(transform.gameObject);
    }
}
