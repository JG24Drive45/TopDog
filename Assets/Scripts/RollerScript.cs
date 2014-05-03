using UnityEngine;
using System.Collections;

public class RollerScript : MonoBehaviour {

	public float rollerRotSpeed = 50.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAround( transform.position, Vector3.back, rollerRotSpeed * Time.deltaTime );
	}
}
