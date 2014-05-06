using UnityEngine;
using System.Collections;

public class HotDogScript : MonoBehaviour 
{
	#region Hotdog Movement Offsets

	// Since our object is oblong we cannot rotate around the center.
	// These offsets provide for a pivot point relative to the center of the object.

	private Vector3 RIGHTVERTPIVOTOFFSET 	= new Vector3( 12.5f, -25.0f, 0.0f );
	private Vector3 LEFTVERTPIVOTOFFSET		= new Vector3( -12.5f, -25.0f, 0.0f );
	private Vector3 UPVERTPIVOTOFFSET		= new Vector3( 0.0f, -25.0f, 12.5f );
	private Vector3 DOWNVERTPIVOTOFFSET		= new Vector3( 0.0f, -25.0f, -12.5f );

	private Vector3 RIGHTHORZPIVOTOFFSET	= new Vector3( 25.0f, -12.5f, 0.0f );
	private Vector3 LEFTHORZPIVOTOFFSET		= new Vector3( -25.0f, -12.5f, 0.0f );
	private Vector3 UPHORZPIVOTOFFSET		= new Vector3( 0.0f, -12.5f, 12.5f );
	private Vector3 DOWNHORZPIVOTOFFSET		= new Vector3( 0.0f, -12.5f, -12.5f );

	private Vector3 RIGHTVHPIVOTOFFSET		= new Vector3( 12.5f, -12.5f, 0.0f );
	private Vector3 LEFTVHPIVOTOFFSET		= new Vector3( -12.5f, -12.5f, 0.0f );
	private Vector3 UPVHPIVOTOFFSET			= new Vector3( 0.0f, -12.5f, 25.0f );
	private Vector3 DOWNVHPIVOTOFFSET		= new Vector3( 0.0f, -12.5f, -25.0f );

	#endregion

	// Describes how the hotdog is currently oriented.
	// The pivot point to use depends upon this state.
	enum OrientationState { VERTICAL, HORIZONTAL, VERTANDHORZ };
	OrientationState orientationState;

	private Vector3 v3OriginalPosition = new Vector3( 12.5f, 15.0f, 0.0f );

	private float fKillHeight = -200.0f;										// Terminating Y-Coordinate value
	private bool bFallDownRunning = false;										// Is the FallDown Coroutine running?

	// Use this for initialization
	void Start () 
	{
		orientationState = OrientationState.HORIZONTAL;							// Starting state for the hotdog
		Physics.gravity *= 5.0f;												// Testing - Increasing the rate of gravity
		Debug.Log( Physics.gravity );											// Testing	
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

	void OnTriggerEnter( Collider other )
	{
		GameObject cam = GameObject.Find( "Main Camera" );

		switch( other.gameObject.tag )
		{
		case "TempKetchup":
			cam.SendMessage( "AcquiredCondiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with ketchup" );
			break;

		case "TempMustard":
			cam.SendMessage( "AcquiredCondiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with mustard" );
			break;

		case "TempRelish":
			cam.SendMessage( "AcquiredCondiment" );
			Destroy( other.gameObject );
			Debug.Log( "You collided with relish" );
			break;

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
		transform.rotation = Quaternion.identity;
		transform.position = v3OriginalPosition;
		orientationState = OrientationState.HORIZONTAL;
		bFallDownRunning = false;
	}
}
