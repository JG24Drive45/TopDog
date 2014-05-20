	using UnityEngine;
using System.Collections;
using System.IO;

public class LevelGeneratorScript : MonoBehaviour 
{
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

    SoundManager m_SoundManager;

	private string sLevel;											// Name of the current level
	private int iLevelNum;											// Current level number

	#region Awake()
    void Awake()
    {
        EventAggregatorManager.AddEventAggregator(GameEventAggregator.GameMessenger);
    }
	#endregion

	#region Start()

	// INSTEAD OF GENERATING TILES LIKE BELOW, WE CAN READ DATA IN FROM A TEXT FILE TO GENERATE THE LEVELS
	void Start () 
	{
        GameEventAggregator.GameMessenger.Subscribe(this);

		#region Load in Level Data
		sLevel = Application.loadedLevelName;						// Get the name of the current level

		iLevelNum = int.Parse( sLevel.Substring( 5 ) );				// Level number for the score script
		Messenger<int>.Broadcast( "set level", iLevelNum );			// Send the level number to the score script

		string[] lines;												// Array that stores text file info
		TextAsset data;												// Text file variable
		data = ( TextAsset )Resources.Load( "LevelData/" + sLevel );
		lines = data.text.Split( "\n"[0] );

		// Read the first 13 lines of text file that make up the tiles
		for( int i = 2; i < 14; i++ )
		{
			int lineLength = lines[i].Length;
			for( int j = 0; j < lineLength; j++ )
			{
				char temp = lines[i][j];
				if( temp == 'E' )
					Instantiate( emptyTile, new Vector3( j * 50, 7.5f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 0, 0, 0 ) ) );
				else if( temp == 'M' )
					Instantiate( mainTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( temp == 'G' )
					Instantiate( goalTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( temp == 'T' )
					Instantiate( teleporterTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( temp == 'B' )
				{
					Instantiate( emptyTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
					Instantiate( bridgeTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				}
				else if( temp == 'F' )
					Instantiate( fallingTile, new Vector3( j * 50, 0.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
				else if( temp == '1' )
					Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 0, 0 ) ) );
				else if( temp == '2' )
					Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 90, 0 ) ) );
				else if( temp == '3' )
					Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 180, 0 ) ) );
				else if( temp == '4' )
					Instantiate( conveyorBelt, new Vector3( j * 50, 15.0f, -( i - 2 ) * 50 ), Quaternion.Euler( new Vector3( 270, 270, 0 ) ) );
			}
		}

		// Read lines 17 & 18 to get the ketchup location
		Vector3 tempPosition = new Vector3( int.Parse( lines[16] ), 12.5f, -int.Parse( lines[17] ) );
		Debug.Log( "Ketchup is at: " + tempPosition );			// For testing
		Instantiate( ketchup, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) );

		// Read lines 20 & 21 to get the mustard location
		tempPosition = new Vector3( int.Parse( lines[19] ) , 12.5f, -int.Parse( lines[20] ) );
		Debug.Log( "Mustard is at: " + tempPosition );			// For testing
		Instantiate( mustard, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) );

		// Read lines 23 & 24 to get the relish location
		tempPosition = new Vector3( int.Parse( lines[22] ), 12.5f, -int.Parse( lines[23] ) );
		Debug.Log( "Relish is at: " + tempPosition );			// For testing
		Instantiate( relish, tempPosition, Quaternion.Euler( new Vector3( 90.0f, 0.0f, 0.0f ) ) );

		// Read lines 26 & 27 to get the switch location
		if( lines[25] != "none\r" )
		{
			Debug.Log( "Line 25 is: " + lines[25] );
			tempPosition = new Vector3( int.Parse( lines[25] ), 12.5f, -int.Parse( lines[26] ) );
			Debug.Log( "Switch is at: " + tempPosition );
			Instantiate( switchObject, tempPosition, Quaternion.Euler( new Vector3( 270.0f, 0.0f, 0.0f ) ) );
		}

		// Read lines 29 - 31 to get the player's (hotdog) location
		tempPosition = new Vector3( int.Parse( lines[28] ), int.Parse( lines[29] ), -int.Parse( lines[30] ) );
		Debug.Log( "Player is at: " + tempPosition );
		// Read lines 33 - 35 to get the player's (hotdog) rotation
		Vector3 tempRotation = new Vector3( int.Parse( lines[32] ), int.Parse( lines[33] ), int.Parse( lines[34] ) );
		Debug.Log( "Player rotation is: " + tempRotation );
		Instantiate( player, tempPosition, Quaternion.Euler( tempRotation ) );

		// Read line 37 to get the player's orientation state
		int tempState = int.Parse( lines[36] );
		Debug.Log( "OState is: " + tempState );
		// Send the message to set the player's orientation state
		Messenger<int>.Broadcast( "set player original orientation state", tempState );
		#endregion

		// Instantiate the coals
		Instantiate( coals, new Vector3( 0, -250, 250 ), Quaternion.identity );

        LoadSounds();
	}
	#endregion

	#region void LoadSounds()
    void LoadSounds()
    {
        m_SoundManager = gameObject.GetComponent<SoundManager>() as SoundManager;
		m_SoundManager.LoadSound( "hotdogStep", "SFX/HotDogStep", 1 );					// Load player movement sound
        m_SoundManager.LoadSound( "splat", "SFX/splat3",5 );							// Load condiment acquired sound
		m_SoundManager.LoadSound( "switch", "SFX/SwitchActivated", 1 );					// Load the switch sound
		m_SoundManager.LoadSound( "teleport", "SFX/teleport_Sound", 3 );				// Load the teleport sound
		m_SoundManager.LoadSound( "goal", "SFX/goalSound", 1 );							// Load the goal sound
        m_SoundManager.LoadSound( "lvlMusic", "Music/level_music_7", 1 );				// Load some background music
    }
	#endregion

	#region OnEnable()
	void OnEnable()
	{
		Messenger.AddListener( "go to next level", LoadNextLevel );
	}
	#endregion

	#region OnDisable()
	void OnDisable()
	{
		Messenger.RemoveListener( "go to next level", LoadNextLevel );
	}
	#endregion
	
	#region void Update()
	void Update () 
	{
        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            Debug.Log("Key Pressed");
            EventAggregatorManager.Publish(new PlaySoundMessage("lvlMusic", true));
        }
        if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            Debug.Log("Key Pressed");
            EventAggregatorManager.Publish(new StopSoundLoopMessage("lvlMusic"));
        }
        GameEventAggregator.GameMessenger.Update();
	}
	#endregion

	#region void LoadNextLevel()
	void LoadNextLevel()
	{
		iLevelNum++;
		Invoke( "LoadNext", 3.0f );
	}
	#endregion

	void LoadNext()
	{
		Application.LoadLevel( "Level" + iLevelNum.ToString() );
	}
}
