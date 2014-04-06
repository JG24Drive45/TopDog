using UnityEngine;
using System.Collections;

public class RollerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAround( transform.position, Vector3.back, 50 * Time.deltaTime );
	}
}
