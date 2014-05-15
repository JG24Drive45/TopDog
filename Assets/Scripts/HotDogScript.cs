using UnityEngine;
using System.Collections;

public class HotDogScript : MonoBehaviour 
{
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
	enum OrientationState { NONE, VERTICAL, HORIZONTAL, VERTANDHORZ };
	OrientationState orientationState = OrientationState.NONE;

	private bool bCanMove = true;												// Can the player currently move
	private char sLastKeyUsed;													// Last arrow button the player used

	private Vector3 v3OriginalPosition;											// Starting position for the level
	private Vector3 v3OriginalRotation;											// Starting rotation for the level

	private float fKillHeight = -200.0f;										// Terminating Y-Coordinate value

	// Use this for initialization
	void Start () 
	{
		//orientationState = OrientationState.NONE;								// Starting state for the hotdog
		v3OriginalPosition = transform.position;								// Cache original position
		v3OriginalRotation = transform.rotation.eulerAngles;					// Cache original rotation
		Physics.gravity *= 5.0f;												// Testing - Increasing the rate of gravity
		Debug.Log( Physics.gravity );											// Testing	

		renderer.material = hotdogMaterials[0];									// Give the hotdog the empty material
	}

	public void OnEnable()
	{
		// Add listeners here
		Messenger<int>.AddListener( "set player orientation state", SetPlayerOrientationState );
		Messenger<int,int,int>.AddListener( "set player original rotation", SetPlayerOriginalRotation );
	}

	public void OnDisable()
	{
		// Remove listeners here
		Messenger<int>.RemoveListener( "set player orientation state", SetPlayerOrientationState );
		Messenger<int,int,int>.RemoveListener( "set player original rotation", SetPlayerOriginalRotation );
	}
	
	// Update is called once per frame
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
				case OrientationState.HORIZONTAL:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTHORZPIVOTOFFSET, Vector3.back, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( 'R' );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTHORZPIVOTOFFSET, Vector3.forward, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( 'L' );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPHORZPIVOTOFFSET, Vector3.right, 90.0f );
						SetLastKeyUsed( 'U' );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNHORZPIVOTOFFSET, Vector3.left, 90.0f );
						SetLastKeyUsed( 'D' );
					}

					//audio.Play();
					EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
					break;
				case OrientationState.VERTICAL:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTVERTPIVOTOFFSET, Vector3.back, 90.0f );
						orientationState = OrientationState.HORIZONTAL;
						SetLastKeyUsed( 'R' );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTVERTPIVOTOFFSET, Vector3.forward, 90.0f );
						orientationState = OrientationState.HORIZONTAL;
						SetLastKeyUsed( 'L' );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPVERTPIVOTOFFSET, Vector3.right, 90.0f );
						orientationState = OrientationState.VERTANDHORZ;
						SetLastKeyUsed( 'U' );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNVERTPIVOTOFFSET, Vector3.left, 90.0f );
						orientationState = OrientationState.VERTANDHORZ;
						SetLastKeyUsed( 'D' );
					}

					//audio.Play();
					EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
					break;
				case OrientationState.VERTANDHORZ:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTVHPIVOTOFFSET, Vector3.back, 90.0f );
						SetLastKeyUsed( 'R' );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTVHPIVOTOFFSET, Vector3.forward, 90.0f );
						SetLastKeyUsed( 'L' );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPVHPIVOTOFFSET, Vector3.right, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( 'U' );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNVHPIVOTOFFSET, Vector3.left, 90.0f );
						orientationState = OrientationState.VERTICAL;
						SetLastKeyUsed( 'D' );
					}

					//audio.Play();
					EventAggregatorManager.Publish(new PlaySoundMessage("hotdogStep", false));
					break;
				}
			}
		}
	}

	void SetLastKeyUsed( char key )
	{
		sLastKeyUsed = key;
	}

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

	public void SetPlayerOrientationState( int orientation )
	{
		orientationState = (OrientationState)orientation;
	}

	public void SetPlayerOriginalRotation( int x, int y, int z )
	{
		v3OriginalRotation.x = x;
		v3OriginalRotation.y = y;
		v3OriginalRotation.z = z;
	}

	void OnTriggerEnter( Collider other )
	{
		switch( other.gameObject.tag )
		{
		case "EmptyTile":
			Debug.Log( "Touched empty tile" );
			bCanMove = false;
			break;

		case "FallingTile":
			other.gameObject.SendMessage( "ToggleBeenTouched" );
			Debug.Log( "Touched falling tile" );
			break;

		case "Switch":
			break;

		case "Ketchup":
			Messenger.Broadcast( "acquired condiment" );								// Send the message to update the score
			bHasKetchup = true;															// Player has acquired the ketchup
			bFullDog = IsFullDog();														// Is the dog full?
			SetMaterial();																// Update the material
			EventAggregatorManager.Publish( new PlaySoundMessage( "splat", false ) );	// Play the splat sound
			Destroy( other.gameObject );
			Debug.Log( "You collided with ketchup" );
			break;

		case "Mustard":
			Messenger.Broadcast( "acquired condiment" );								// Send the message to update the score
			bHasMustard = true;															// Player has acquired the mustard
			bFullDog = IsFullDog();														// Is the dog full?
			SetMaterial();																// Update the material
			EventAggregatorManager.Publish( new PlaySoundMessage( "splat", false ) );	// Play the splat sound
			Destroy( other.gameObject );
			Debug.Log( "You collided with mustard" );
			break;

		case "Relish":
			Messenger.Broadcast( "acquired condiment" );								// Send the message to update the score
			bHasRelish = true;															// Player has acquired the relish
			bFullDog = IsFullDog();														// Is the dog full?
			SetMaterial();																// Update the material
			EventAggregatorManager.Publish( new PlaySoundMessage( "splat", false ) );	// Play the splat sound
			Destroy( other.gameObject );
			Debug.Log( "You collided with relish" );
			break;
		}

	}

	bool IsFullDog()
	{
		if( bHasKetchup && bHasMustard && bHasRelish )
			return true;
		else
			return false;
	}

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

}
