using UnityEngine;
using System.Collections;

public class HotDogScript : MonoBehaviour 
{
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

	enum OrientationState { VERTICAL, HORIZONTAL, VERTANDHORZ };
	OrientationState orientationState;


	// Use this for initialization
	void Start () 
	{
		orientationState = OrientationState.HORIZONTAL;
	}
	
	// Update is called once per frame
	void Update () 
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

				break;
			}
		}
	}

	void OnTriggerEnter( Collider other )
	{
		switch( other.gameObject.tag )
		{
		case "TempKetchup":
			Destroy( other.gameObject );
			Debug.Log( "You collided with ketchup" );
			break;
		}
	}
}
