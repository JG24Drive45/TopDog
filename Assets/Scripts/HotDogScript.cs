using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HotDogScript : MonoBehaviour 
{
	#region Delegates
	public delegate void CondimentHandler();
	public delegate void LevelComplete(bool gotAllCondiments);
	#endregion

	#region Events
	public static event CondimentHandler onCondimentAcquired;
	public static event LevelComplete onLevelComplete;
	#endregion

	#region Hotdog Movement Offsets

	// Since our object is oblong we cannot rotate around the center.
	// These offsets provide for a pivot point relative to the center of the object.

	// Pivots for vertical orientation
	private Vector3 RIGHTVERTPIVOTOFFSET 	= new Vector3( 25.0f, -50.0f, 0.0f );
	private Vector3 LEFTVERTPIVOTOFFSET		= new Vector3( -25.0f, -50.0f, 0.0f );
	private Vector3 UPVERTPIVOTOFFSET		= new Vector3( 0.0f, -50.0f, 25.0f );
	private Vector3 DOWNVERTPIVOTOFFSET		= new Vector3( 0.0f, -50.0f, -25.0f );

	// Pivots for horizontal orientation
	private Vector3 RIGHTHORZPIVOTOFFSET	= new Vector3( 50.0f, -25.0f, 0.0f );
	private Vector3 LEFTHORZPIVOTOFFSET		= new Vector3( -50.0f, -25.0f, 0.0f );
	private Vector3 UPHORZPIVOTOFFSET		= new Vector3( 0.0f, -25.0f, 25.0f );
	private Vector3 DOWNHORZPIVOTOFFSET		= new Vector3( 0.0f, -25.0f, -25.0f );

	// Pivots for vertical and horizontal orientation
	private Vector3 RIGHTVHPIVOTOFFSET		= new Vector3( 25.0f, -25.0f, 0.0f );
	private Vector3 LEFTVHPIVOTOFFSET		= new Vector3( -25.0f, -25.0f, 0.0f );
	private Vector3 UPVHPIVOTOFFSET			= new Vector3( 0.0f, -25.0f, 50.0f );
	private Vector3 DOWNVHPIVOTOFFSET		= new Vector3( 0.0f, -25.0f, -50.0f );

	#endregion

	#region Data Members
	// Hotdog Condiment Traits
	private bool bHasKetchup = false;
	private bool bHasMustard = false;
	private bool bHasRelish	 = false;
	private bool bFullDog    = false;

	// Materials for Hotdog
	public Material[] hotdogMaterials;
		// 0 - Empty Dog
		// 1 - Ketchup
		// 2 - Mustard
		// 3 - Relish
		// 4 - Ketchup & Mustard
		// 5 - Ketchup & Relish
		// 6 - Mustard & Relish
		// 7 - Ketchup & Mustard & Relish

	// Describes how the hotdog is currently oriented.
	// The pivot point to use depends upon this state.
	private enum OrientationState { NONE, VERTICAL, HORIZONTAL, VERTANDHORZ };
	private OrientationState orientationState;

	private bool bCanMove = true;												// Can the player currently move
	private string sLastKeyUsed;													// Last arrow button the player used
	private bool bIsTeleporting = false;										// Is the player currently teleporting?
	private Vector3 v3LastTeleportedLocation;
	private bool bTouchingATile = true;											// Is the player currently touching any tiles?
	private bool bOnConveyor = false;

	private Vector3 v3OriginalPosition;											// Starting position for the level
	private Vector3 v3OriginalRotation;											// Starting rotation for the level
	private OrientationState oStateOriginal;									// Starting orientation state

	private float fKillHeight = -250.0f;										// Terminating Y-Coordinate value
	private float fGoalSpeed = 100.0f;											// Speed at which the dog falls
	private int iConveyorSpeed = 2;												// Speed the dog moves while on the conveyor belt
	private float fDeadSpeed = 150.0f;
	private float fSizzleHeight = -150.0f;
	private bool bFalling = false;
	private GameObject[] empties;
	public List<GameObject> allEmptyTiles;
	private int numTouching = 0;
	private int counter = 0;
	#endregion

	#region void Start()
	void Start () 
	{
		v3OriginalPosition = transform.position;								// Cache original position
		v3OriginalRotation = transform.rotation.eulerAngles;					// Cache original rotation
		Physics.gravity *= 5.0f;												// Testing - Increasing the rate of gravity
		Debug.Log( Physics.gravity );											// Testing	

		renderer.material = hotdogMaterials[0];									// Give the hotdog the empty material

		empties = GameObject.FindGameObjectsWithTag( "zEmptyTile" );
		int length = empties.Length;
		for( int i = 0; i < length; i++ )
		{
			allEmptyTiles.Add( empties[i] );
			empties[i] = null;
		}
	}
	#endregion

	#region void OnEnable()
	public void OnEnable()
	{
		FallingTileScript.onFallingTile += AddEmptyTileToList;
		// Add listeners here
		Messenger<int>.AddListener( "set player original orientation state", SetPlayerOriginalOrientationState );
		Messenger<int,int,int>.AddListener( "set player original rotation", SetPlayerOriginalRotation );
	}
	#endregion

	#region void OnDisable()
	public void OnDisable()
	{
		FallingTileScript.onFallingTile -= AddEmptyTileToList;
		// Remove listeners here
		Messenger<int>.RemoveListener( "set player original orientation state", SetPlayerOriginalOrientationState );
		Messenger<int,int,int>.RemoveListener( "set player original rotation", SetPlayerOriginalRotation );
	}
	#endregion
	
	#region void Update()
	void Update () 
	{
		// Stop player from moving if they are currently falling
		if( bCanMove )
		{
			// MOVEMENT IF THE PLAYER PRESSES <LEFT>, <RIGHT>, <UP>, OR <DOWN>
			if( IsMovementKeyDown() )
			{
				switch( orientationState )
				{
					#region CASE: HORIZONTAL
				case OrientationState.HORIZONTAL:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTHORZPIVOTOFFSET, Vector3.back, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( "R" );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTHORZPIVOTOFFSET, Vector3.forward, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( "L" );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPHORZPIVOTOFFSET, Vector3.right, 90.0f );
						SetLastKeyUsed( "U" );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNHORZPIVOTOFFSET, Vector3.left, 90.0f );
						SetLastKeyUsed( "D" );
					}

					bTouchingATile = false;
					EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
					break;
					#endregion

					#region CASE: VERTICAL
				case OrientationState.VERTICAL:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTVERTPIVOTOFFSET, Vector3.back, 90.0f );
						orientationState = OrientationState.HORIZONTAL;
						SetLastKeyUsed( "R" );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTVERTPIVOTOFFSET, Vector3.forward, 90.0f );
						orientationState = OrientationState.HORIZONTAL;
						SetLastKeyUsed( "L" );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPVERTPIVOTOFFSET, Vector3.right, 90.0f );
						orientationState = OrientationState.VERTANDHORZ;
						SetLastKeyUsed( "U" );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNVERTPIVOTOFFSET, Vector3.left, 90.0f );
						orientationState = OrientationState.VERTANDHORZ;
						SetLastKeyUsed( "D" );
					}

					bTouchingATile = false;
					EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
					break;

					#endregion

					#region CASE: VERTICAL & HORIZONTAL

				case OrientationState.VERTANDHORZ:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTVHPIVOTOFFSET, Vector3.back, 90.0f );
						SetLastKeyUsed( "R" );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTVHPIVOTOFFSET, Vector3.forward, 90.0f );
						SetLastKeyUsed( "L" );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPVHPIVOTOFFSET, Vector3.right, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( "U" );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNVHPIVOTOFFSET, Vector3.left, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( "D" );
					}

					bTouchingATile = false;
					EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
					break;

					#endregion
				}
			}
		}

	}
	#endregion
	
	#region void SetLastKeyUsed( string key )
	void SetLastKeyUsed( string key )
	{
		sLastKeyUsed = key;
	}
	#endregion

	#region bool IsMovementKeyDown()
	// Tells us if the player has pressed, up, down, left, right, W, A, S, or D
	bool IsMovementKeyDown()
	{
		if( Input.GetKeyDown( KeyCode.LeftArrow ) ||
		    Input.GetKeyDown( KeyCode.RightArrow ) ||
		    Input.GetKeyDown( KeyCode.UpArrow ) ||
		    Input.GetKeyDown( KeyCode.DownArrow ) ||
		    Input.GetKeyDown( KeyCode.A ) ||
		    Input.GetKeyDown( KeyCode.D ) ||
		    Input.GetKeyDown( KeyCode.W ) ||
		    Input.GetKeyDown( KeyCode.S ) )
			return true;
		else
			return false;
	}
	#endregion

	#region void SetPlayerOriginalOrientationState( int orientation )
	public void SetPlayerOriginalOrientationState( int orientation )
	{
		oStateOriginal = orientationState = (OrientationState)orientation;
	}
	#endregion

	#region void SetPlayerOriginalRotation( int x, int y, int z )
	public void SetPlayerOriginalRotation( int x, int y, int z )
	{
		v3OriginalRotation.x = x;
		v3OriginalRotation.y = y;
		v3OriginalRotation.z = z;
	}
	#endregion

	#region void TeleportPlayer( Vector3 tilePosition )
	void TeleportPlayer( Vector3 tilePosition )
	{
		GameObject[] teleporters = GameObject.FindGameObjectsWithTag( "TeleporterTile" );
		int numTeleporters = teleporters.Length;

		int randomNum = Random.Range(0, numTeleporters);

		while( teleporters[randomNum].transform.position == tilePosition || teleporters[randomNum].transform.position == v3LastTeleportedLocation )
		{
			randomNum = Random.Range(0, numTeleporters);
		}

		// Play the teleport sound
		EventAggregatorManager.Publish(new PlaySoundMessage("teleport", false));
		// Set the player's x and z position values to that of the teleporter tile
		transform.position = new Vector3( teleporters[randomNum].transform.position.x, transform.position.y, teleporters[randomNum].transform.position.z );
		// Set teleporting to true
		bIsTeleporting = true;

		v3LastTeleportedLocation = tilePosition;
	}
	#endregion

	#region void OnTriggerEnter( Collider other )
	void OnTriggerEnter( Collider other )
	{
		if( !bFalling )
		{
			switch( other.gameObject.tag )
			{
			case "MainTile":
				Debug.Log( "Touching main tile" );
				bTouchingATile = true;
				break;

			case "TeleporterTile":
				if( !bIsTeleporting && orientationState == OrientationState.VERTICAL )
				{
					Debug.Log( "Touched Teleporter" );
					TeleportPlayer( other.transform.position );
				}
				if( !bFalling )
					bTouchingATile = true;
				break;

			case "FallingTile":
				bTouchingATile = true;
				other.gameObject.SendMessage( "ToggleBeenTouched" );
				Debug.Log( "Touched falling tile" );
				break;

			case "Switch":
				Debug.Log( "Touched Switch" );
				EventAggregatorManager.Publish( new PlaySoundMessage( "switch", false ) );
				if( other.gameObject.GetComponent<CapsuleCollider>().enabled )
				{
					other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
					Messenger.Broadcast( "set active switch material" );
					Messenger.Broadcast( "activate bridge" );
				}
				break;

			case "BridgeTile":
				bTouchingATile = true;
				break;

			case "Ketchup":
				if( onCondimentAcquired != null )											// If there is a subscriber
					onCondimentAcquired();														// Send the message out
				bHasKetchup = true;															// Player has acquired the ketchup
				bFullDog = IsFullDog();														// Is the dog full?
				SetMaterial();																// Update the material
				EventAggregatorManager.Publish( new PlaySoundMessage( "splat", false ) );	// Play the splat sound
				Destroy( other.gameObject );
				Debug.Log( "You collided with ketchup" );
				break;

			case "Mustard":
				if( onCondimentAcquired != null )											// If there is a subscriber
					onCondimentAcquired();														// Send the message out
				bHasMustard = true;															// Player has acquired the mustard
				bFullDog = IsFullDog();														// Is the dog full?
				SetMaterial();																// Update the material
				EventAggregatorManager.Publish( new PlaySoundMessage( "splat", false ) );	// Play the splat sound
				Destroy( other.gameObject );
				Debug.Log( "You collided with mustard" );
				break;

			case "Relish":
				if( onCondimentAcquired != null )											// If there is a subscriber
					onCondimentAcquired();														// Send the message out
				bHasRelish = true;															// Player has acquired the relish
				bFullDog = IsFullDog();														// Is the dog full?
				SetMaterial();																// Update the material
				EventAggregatorManager.Publish( new PlaySoundMessage( "splat", false ) );	// Play the splat sound
				Destroy( other.gameObject );
				Debug.Log( "You collided with relish" );
				break;

			case "GoalTile":
				if( orientationState == OrientationState.VERTICAL )
				{
					if( onLevelComplete != null )
						onLevelComplete(IsFullDog());
					Debug.Log( "Hit goal tile!" );
					EventAggregatorManager.Publish( new PlaySoundMessage( "goal", false ) );// Play the goal sound
					bCanMove = false;														// Don't allow the player to move now
					StartCoroutine( "FallThroughGoal" );
				}
				bTouchingATile = true;
				break;

			case "zEmptyTile":
				if( !bOnConveyor )
				{
					Debug.Log( "Touched empty tile" );
					bTouchingATile = true;
					bCanMove = false;
					bFalling = true;
					StartCoroutine( FallDown() );
				}
				break;
			}
		}
	}

	#endregion

	#region void OnTriggerStay( Collider other )
	void OnTriggerStay( Collider other )
	{
		switch( other.gameObject.tag )
		{
		case "Conveyor":
			Debug.Log( other.transform.rotation.eulerAngles.y );
			if( bCanMove && !bTouchingATile )
			{
				StartCoroutine( MoveDogOnConveyor( other.gameObject ) );
			}
			break;

		case "zEmptyTile":
			if( !bFalling && !bOnConveyor )
			{
				Debug.Log( "Staying on empty tile" );
				bTouchingATile = true;
				bCanMove = false;
				bFalling = true;
				StartCoroutine( FallDown() );
			}
			break;
		}
	}
	#endregion

	#region void OnTriggerExit( Collider other )
	void OnTriggerExit( Collider other )
	{
		switch( other.gameObject.tag )
		{
		case "TeleporterTile":
			bIsTeleporting = false;
			break;
		}
	}
	#endregion

	#region IEnumerator MoveDogOnConveyor( GameObject other )
	IEnumerator MoveDogOnConveyor( GameObject other )
	{
		bCanMove = false;
		bOnConveyor = true;
		int step = 0;

		#region If on a conveyor moving right
		if( other.transform.rotation.eulerAngles.y == 0.0f )
		{
			if( orientationState == OrientationState.VERTICAL || orientationState == OrientationState.VERTANDHORZ )
			{
				while( step < 25 )
				{
					transform.position += new Vector3( iConveyorSpeed, 0, 0 );
					step++;
					yield return null;
				}
			}
			else
			{
				while( step < 25 )
				{
					transform.position += new Vector3( iConveyorSpeed, 0, 0 );
					step++;
					yield return null;
				}
			}
		}
		#endregion

		#region If on a conveyor moving down
		else if( other.transform.rotation.eulerAngles.y == 90.0f )
		{
			if( orientationState == OrientationState.VERTICAL || orientationState == OrientationState.HORIZONTAL )
			{
				while( step < 25 )
				{
					transform.position -= new Vector3( 0, 0, iConveyorSpeed );
					step++;
					yield return null;
				}
			}
			else
			{
				while( step < 50 )
				{
					transform.position -= new Vector3( 0, 0, iConveyorSpeed );
					step++;
					yield return null;
				}
			}
		}
		#endregion

		#region If on a conveyor moving up
		else if( other.transform.rotation.eulerAngles.y == 270.0f )
		{
			if( orientationState == OrientationState.VERTICAL || orientationState == OrientationState.HORIZONTAL )
			{
				while( step < 25 )
				{
					transform.position += new Vector3( 0, 0, iConveyorSpeed );
					step++;
					yield return null;
				}
			}
			else
			{
				while( step < 25 )
				{
					transform.position += new Vector3( 0, 0, iConveyorSpeed );
					step++;
					yield return null;
				}
			}
		}
		#endregion

		#region If on a conveyor moving left
		else
		{
			if( orientationState == OrientationState.VERTICAL || orientationState == OrientationState.VERTANDHORZ )
			{
				while( step < 25 )
				{
					transform.position -= new Vector3( iConveyorSpeed, 0, 0 );
					step++;
					yield return null;
				}
			}
			else
			{
				while( step < 25 )
				{
					transform.position -= new Vector3( iConveyorSpeed, 0, 0 );
					step++;
					yield return null;
				}
			}
		}
		#endregion

		bOnConveyor = false;
		bCanMove = true;
	}
	#endregion

	#region IEnumerator FallDown()
	public IEnumerator FallDown()
	{
		#region Get the number of empty tiles that the dog is touching
		numTouching = 0;
		int oState = (int)orientationState;
		GameObject[] goList = new GameObject[2];
		// Check to see how many of the empty tiles the dog is touching
		if( oState == 1 )
		{
			counter = 0;
			while( counter < allEmptyTiles.Count )
			{
				if( allEmptyTiles[counter] != null )
				{
					if( (int)Mathf.Round(this.transform.position.x) == (int)Mathf.Round(allEmptyTiles[counter].transform.position.x) &&
					   (int)Mathf.Round(this.transform.position.z) == (int)Mathf.Round(allEmptyTiles[counter].transform.position.z) )
					{
						numTouching++;
						break;
					}
					counter += 1;
				}
				else
				{
					counter += 1;
				}
			}
		}
		if( oState == 2 )
		{
			counter = 0;
			while( counter < allEmptyTiles.Count )
			{
				if( allEmptyTiles[counter] != null )
				{
					if( ( (int)Mathf.Round(this.transform.position.x) == (int)(Mathf.Round(allEmptyTiles[counter].transform.position.x) + 25 ) ||
					     (int)Mathf.Round(this.transform.position.x) == (int)(Mathf.Round(allEmptyTiles[counter].transform.position.x) - 25 ) ) &&
					   (int)Mathf.Round(this.transform.position.z) == (int)Mathf.Round(allEmptyTiles[counter].transform.position.z) )
					{
						numTouching++;
						if( goList[0] == null )
							goList[0] = allEmptyTiles[counter];
						else
							goList[1] = allEmptyTiles[counter];
						if( numTouching >= 2 )
							break;
					}
					counter += 1;
				}
				else
				{
					counter += 1;
				}
			}
		}
		if( oState == 3 )
		{
			counter = 0;
			while( counter < allEmptyTiles.Count )
			{
				if( allEmptyTiles[counter] != null )
				{
					if( (int)Mathf.Round(this.transform.position.x) == (int)Mathf.Round(allEmptyTiles[counter].transform.position.x) &&
					   ( (int)Mathf.Round(this.transform.position.z) == (int)(Mathf.Round(allEmptyTiles[counter].transform.position.z) - 25 ) || 
						 (int)Mathf.Round(this.transform.position.z) == (int)(Mathf.Round(allEmptyTiles[counter].transform.position.z) + 25 ) ) )
					{
						numTouching++;
						if( goList[0] == null )
							goList[0] = allEmptyTiles[counter];
						else
							goList[1] = allEmptyTiles[counter];
						if( numTouching >= 2 )
							break;
					}
					counter += 1;
				}
				else
				{
					counter += 1;
				}
			}
		}
		#endregion

		while( transform.position.y > fKillHeight )
		{
			if( transform.position.y < fSizzleHeight )
				EventAggregatorManager.Publish(new PlaySoundMessage("death", false));

			#region Vert / Horz Fall
			// If the dog's orientation is vert/horz
			if( oState == 3 )
			{
				// If dog is touching 2 empty tiles
				if( numTouching == 2 )
				{
					// If the last key pressed was L
					if( sLastKeyUsed == "L" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.forward, 2.5f );
						yield return null;
					}
					// If the last key pressed was R
					else if( sLastKeyUsed == "R" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.back, 2.5f );
						yield return null;
					}
					// If the last key pressed was U
					else if( sLastKeyUsed == "U" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.right, 2.5f );
						yield return null;
					}
					// If the last key pressed was D
					else if( sLastKeyUsed == "D" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.left, 2.5f );
						yield return null;
					}
				}
				// If dog is touching 1 empty tile
				else if( numTouching == 1 )
				{
					transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
					// If the dogs's x pos is == goList object x pos and dog's z pos == goList object z pos - 25   >> empty above
					if( (int)Mathf.Round(this.transform.position.x) == (int)Mathf.Round(goList[0].transform.position.x) &&
					    (int)Mathf.Round(this.transform.position.z) == (int)(Mathf.Round(goList[0].transform.position.z) - 25) )
					{
						transform.RotateAround( transform.position, Vector3.right, 2.5f );
						yield return null;
					}
					// If the dog's x pos is == goList object x pos and dog's z pos == goList object z pos + 25  >> empty below
					else if( (int)Mathf.Round(this.transform.position.x) == (int)Mathf.Round(goList[0].transform.position.x) &&
					    (int)Mathf.Round(this.transform.position.z) == (int)(Mathf.Round(goList[0].transform.position.z) + 25) )
					{
						transform.RotateAround( transform.position, Vector3.left, 2.5f );
						yield return null;
					}
					
				}
			}
			#endregion

			#region Vertical Fall
			// If the dog's orientation is vertical
			else if( oState == 1 )
			{
				// If dog is touching 1 empty tile
				if( numTouching == 1 )
				{
					// If the last key pressed was L
					if( sLastKeyUsed == "L" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.forward, 2.5f );
						yield return null;
					}
					// If the last key pressed was R
					else if( sLastKeyUsed == "R" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.back, 2.5f );
						yield return null;
					}
					// If the last key pressed was U
					else if( sLastKeyUsed == "U" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.right, 2.5f );
						yield return null;
					}
					// If the last key pressed was D
					else if( sLastKeyUsed == "D" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.left, 2.5f );
						yield return null;
					}
				}
			}
			#endregion

			#region Horizontal Fall
			// If the dog's orientation is horizontal
			else if( oState == 2 )
			{
				// If dog is touching 2 empty tiles
				if( numTouching == 2 )
				{
					// If the last key pressed was L
					if( sLastKeyUsed == "L" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.forward, 2.5f );
						yield return null;
					}
					// If the last key pressed was R
					else if( sLastKeyUsed == "R" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.back, 2.5f );
						yield return null;
					}
					// If the last key pressed was U
					else if( sLastKeyUsed == "U" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.right, 2.5f );
						yield return null;
					}
					// If the last key pressed was D
					else if( sLastKeyUsed == "D" )
					{
						transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
						transform.RotateAround( transform.position, Vector3.left, 2.5f );
						yield return null;
					}
				}
				// If dog is touching 1 empty tile
				else if( numTouching == 1 )
				{
					transform.position -= new Vector3( 0, fDeadSpeed * Time.deltaTime, 0 );
					// If the dog's x pos is == goList object x pos + 25 AND dog's z pos == goList object z pos   >> empty to the left
					if( (int)Mathf.Round(this.transform.position.x) == (int)Mathf.Round(goList[0].transform.position.x) + 25 &&
					   	(int)Mathf.Round(this.transform.position.z) == (int)Mathf.Round(goList[0].transform.position.z) )
					{
						transform.RotateAround( transform.position, Vector3.forward, 2.5f );
						yield return null;
					}
					// If the dog's x pos is == goList object x pos - 25 AND dog's z pos == goList object z pos   >> empty to the right
					if( (int)Mathf.Round(this.transform.position.x) == (int)Mathf.Round(goList[0].transform.position.x) - 25 &&
					   	(int)Mathf.Round(this.transform.position.z) == (int)Mathf.Round(goList[0].transform.position.z) )
					{
						transform.RotateAround( transform.position, Vector3.back, 2.5f );
						yield return null;
					}
				}
			}
			#endregion
		}

		StopAllCoroutines();
        EventAggregatorManager.Publish(new DestroyLevelMessage(LevelGeneratorScript.sLevel));
        EventAggregatorManager.Publish(new LoadLevelMessage(LevelGeneratorScript.sLevel));

	}
	#endregion

	#region IEnumerator FallThroughGoal()
	IEnumerator FallThroughGoal()
	{
		while( true )
		{
			transform.position -= new Vector3( 0, fGoalSpeed * Time.deltaTime, 0 );
			yield return null;
		}
		Debug.Log( "Testing" );
	}
	#endregion

	#region bool IsFullDog()
	bool IsFullDog()
	{
		if( bHasKetchup && bHasMustard && bHasRelish )
			return true;
		else
			return false;
	}
	#endregion

	#region void ResetPlayer()
	public void ResetPlayer()
	{
		transform.rotation = Quaternion.Euler( v3OriginalRotation );					// Reset the player's rotation
		transform.position = v3OriginalPosition;										// Reset the player's position
		orientationState = oStateOriginal;												// Reset the player's orientation state
		bCanMove = true;																// Allow the player to move again
		bFalling = false;
	}
	#endregion

	#region void SetMaterial()
	void SetMaterial()
	{
		// If you have no condiments
		if( !bHasKetchup && !bHasMustard && !bHasRelish )
			renderer.material = hotdogMaterials[0];
		// If you only have ketchup
		else if( bHasKetchup && !bHasMustard && !bHasRelish )
			renderer.material = hotdogMaterials[1];
		// If you only have mustard
		else if( !bHasKetchup && bHasMustard && !bHasRelish )
			renderer.material = hotdogMaterials[2];
		// If you only have relish
		else if( !bHasKetchup && !bHasMustard && bHasRelish )
			renderer.material = hotdogMaterials[3];
		// If you have ketchup & mustard
		else if( bHasKetchup && bHasMustard && !bHasRelish )
			renderer.material = hotdogMaterials[4];
		// If you have ketchup & relish
		else if( bHasKetchup && !bHasMustard && bHasRelish )
			renderer.material = hotdogMaterials[5];
		// If you have mustard & relish
		else if( !bHasKetchup && bHasMustard && bHasRelish )
			renderer.material = hotdogMaterials[6];
		// If you have all condiments
		else if( bFullDog )
			renderer.material = hotdogMaterials[7];
	}
	#endregion

	#region void AddEmptyTileToList( GameObject tile )
	void AddEmptyTileToList( GameObject tile )
	{
		GameObject levGen = GameObject.FindGameObjectWithTag("LevelGenerator");
		tile.transform.parent = levGen.transform;
		allEmptyTiles.Add( tile );

		// Check to see if the dog is currently in this position 
		if( orientationState == OrientationState.VERTICAL )
		{
			// Make it fall if it is
			if( (int)Mathf.Round(transform.position.x) == (int)Mathf.Round(tile.transform.position.x) &&
			    (int)Mathf.Round(transform.position.z) == (int)Mathf.Round(tile.transform.position.z) )
			{
				if( !bFalling )
				{
					Debug.Log( "Staying on empty tile" );
					bTouchingATile = true;
					bCanMove = false;
					bFalling = true;
					StartCoroutine( FallDown() );
				}
			}
		}
		else if( orientationState == OrientationState.HORIZONTAL )
		{
			if( ( (int)Mathf.Round(transform.position.x) == (int)Mathf.Round(tile.transform.position.x + 25) ||
			      (int)Mathf.Round(transform.position.x) == (int)Mathf.Round(tile.transform.position.x - 25) ) &&
			      (int)Mathf.Round(transform.position.z) == (int)Mathf.Round(tile.transform.position.z) )
			{
				if( !bFalling )
				{
					Debug.Log( "Staying on empty tile" );
					bTouchingATile = true;
					bCanMove = false;
					bFalling = true;
					StartCoroutine( FallDown() );
				}
			}
		}
		else if( orientationState == OrientationState.VERTANDHORZ )
		{
			if( (int)Mathf.Round(transform.position.x) == (int)Mathf.Round(tile.transform.position.x) &&
			    ( (int)Mathf.Round(transform.position.z) == (int)Mathf.Round(tile.transform.position.z - 25 ) ||
			 	  (int)Mathf.Round(transform.position.z) == (int)Mathf.Round(tile.transform.position.z + 25 ) ) )
			{
				if( !bFalling )
				{
					Debug.Log( "Staying on empty tile" );
					bTouchingATile = true;
					bCanMove = false;
					bFalling = true;
					StartCoroutine( FallDown() );
				}
			}
		}
	}
	#endregion
}
