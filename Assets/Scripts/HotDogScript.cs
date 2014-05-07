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

	// Describes how the hotdog is currently oriented.
	// The pivot point to use depends upon this state.
	enum OrientationState { NONE, VERTICAL, HORIZONTAL, VERTANDHORZ };
	OrientationState orientationState = OrientationState.NONE;

	private Vector3 v3OriginalPosition;
	private Vector3 v3OriginalRotation = new Vector3( 0.0f, 270.0f, 90.0f );

	private float fKillHeight = -200.0f;										// Terminating Y-Coordinate value
	private bool bFallDownRunning = false;										// Is the FallDown Coroutine running?

	// Use this for initialization
	void Start () 
	{
		//orientationState = OrientationState.NONE;								// Starting state for the hotdog
		v3OriginalPosition = transform.position;								// Cache original position
		Physics.gravity *= 5.0f;												// Testing - Increasing the rate of gravity
		Debug.Log( Physics.gravity );											// Testing	


	}

	public void OnEnable()
	{
		// Add listeners here
		Messenger<int>.AddListener( "set player orientation state", SetPlayerOrientationState );
	}

	public void OnDisable()
	{
		// Remove listeners here
		Messenger<int>.RemoveListener( "set player orientation state", SetPlayerOrientationState );
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Stop player from moving if they are currently falling
		if( !bFallDownRunning )
		{
			// MOVEMENT IF THE PLAYER PRESSES <LEFT>, <RIGHT>, <UP>, OR <DOWN>
			if ( Input.GetKeyDown( KeyCode.DownArrow ) || Input.GetKeyDown( KeyCode.UpArrow ) || 
			    Input.GetKeyDown( KeyCode.RightArrow ) || Input.GetKeyDown( KeyCode.LeftArrow ) )
			{

				switch( orientationState )
				{
				case OrientationState.HORIZONTAL:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTHORZPIVOTOFFSET, Vector3.back, 90.0f );
						orientationState = OrientationState.VERTICAL;
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTHORZPIVOTOFFSET, Vector3.forward, 90.0f );
						orientationState = OrientationState.VERTICAL;
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPHORZPIVOTOFFSET, Vector3.right, 90.0f );
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNHORZPIVOTOFFSET, Vector3.left, 90.0f );
					}

					audio.Play();
					break;
				case OrientationState.VERTICAL:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTVERTPIVOTOFFSET, Vector3.back, 90.0f );
						orientationState = OrientationState.HORIZONTAL;
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTVERTPIVOTOFFSET, Vector3.forward, 90.0f );
						orientationState = OrientationState.HORIZONTAL;
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPVERTPIVOTOFFSET, Vector3.right, 90.0f );
						orientationState = OrientationState.VERTANDHORZ;
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNVERTPIVOTOFFSET, Vector3.left, 90.0f );
						orientationState = OrientationState.VERTANDHORZ;
					}

					audio.Play();
					break;
				case OrientationState.VERTANDHORZ:
					if( Input.GetKeyDown( KeyCode.RightArrow ) )
					{
						transform.RotateAround( transform.position + RIGHTVHPIVOTOFFSET, Vector3.back, 90.0f );
					}
					else if( Input.GetKeyDown( KeyCode.LeftArrow ) )
					{
						transform.RotateAround( transform.position + LEFTVHPIVOTOFFSET, Vector3.forward, 90.0f );
					}
					else if( Input.GetKeyDown( KeyCode.UpArrow ) )
					{
						transform.RotateAround( transform.position + UPVHPIVOTOFFSET, Vector3.right, 90.0f );
						orientationState = OrientationState.VERTICAL;
					}
					else if( Input.GetKeyDown( KeyCode.DownArrow ) )
					{
						transform.RotateAround( transform.position + DOWNVHPIVOTOFFSET, Vector3.left, 90.0f );
						orientationState = OrientationState.VERTICAL;
					}

					audio.Play();
					break;
				}
			}
		}
	}

	public void SetPlayerOrientationState( int orientation )
	{
		orientationState = (OrientationState)orientation;
	}

	void OnTriggerEnter( Collider other )
	{
		switch( other.gameObject.tag )
		{
		case "Ketchup":
			Debug.Log( other.gameObject.tag );
			Messenger.Broadcast( "acquired condiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with ketchup" );
			break;

//		case "TempMustard":
//			cam.SendMessage( "AcquiredCondiment" );
//			Destroy( other.gameObject );
//			Debug.Log( "You collided with mustard" );
//			break;
//
//		case "TempRelish":
//			cam.SendMessage( "AcquiredCondiment" );
//			Destroy( other.gameObject );
//			Debug.Log( "You collided with relish" );
//			break;

		case "Sphere":
			Debug.Log( "Touched Sphere" );
			gameObject.rigidbody.isKinematic = false;
			char direction;

			if( Input.GetKey( KeyCode.LeftArrow ) )
				direction = 'L';
			else if( Input.GetKey( KeyCode.RightArrow ) )
				direction = 'R';
			else if( Input.GetKey( KeyCode.UpArrow ) )
				direction = 'U';
			else
				direction = 'D';

				
			StartCoroutine( "FallDown", direction );

			break;
		}

	}

	IEnumerator FallDown( char arrowDir )
	{
		bFallDownRunning = true;
		Vector3 fallingDirection = Vector3.zero;

		if( arrowDir == 'L' )
			fallingDirection = Vector3.forward;
		else if( arrowDir == 'R' )
			fallingDirection = Vector3.back;
		else if( arrowDir == 'U' )
			fallingDirection = Vector3.right;
		else
			fallingDirection = Vector3.left;

		while( transform.position.y > fKillHeight )
		{
			transform.RotateAround( transform.position, fallingDirection, 1.0f );
			Debug.Log( "Running Coroutine" );
			yield return null;
		}

		transform.rigidbody.isKinematic = true;
		transform.rotation = Quaternion.Euler( v3OriginalRotation );
		transform.position = v3OriginalPosition;
		orientationState = OrientationState.HORIZONTAL;
		bFallDownRunning = false;
	}
}
