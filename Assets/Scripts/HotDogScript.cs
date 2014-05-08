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
	private Vector3 v3OriginalRotation;

	private float fKillHeight = -200.0f;										// Terminating Y-Coordinate value
	private bool bFallDownRunning = false;										// Is the FallDown Coroutine running?

	// Use this for initialization
	void Start () 
	{
		//orientationState = OrientationState.NONE;								// Starting state for the hotdog
		v3OriginalPosition = transform.position;								// Cache original position
		v3OriginalRotation = transform.rotation.eulerAngles;					// Cache original rotation
		Physics.gravity *= 5.0f;												// Testing - Increasing the rate of gravity
		Debug.Log( Physics.gravity );											// Testing	


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
		case "Ketchup":
			// TODO: update hasKetchup bool
			Messenger.Broadcast( "acquired condiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with ketchup" );
			break;

		case "Mustard":
			// TODO: update hasMustard bool
			Messenger.Broadcast( "acquired condiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with mustard" );
			break;

		case "Relish":
			// TODO: update hasRelish bool
			Messenger.Broadcast( "acquired condiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with relish" );
			break;
		}

	}

}
