using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour 
{
	public Material[] switchMaterials;
		// 0 - Unactivated material
		// 1 - Activated material

	// Use this for initialization
	void Start () 
	{
		renderer.material = switchMaterials[0];
	}

	void OnEnable()
	{
		//Messenger.AddListener( "set active switch material", SetActiveMaterial );
		HotDogScript.onActivateSwitch += SetActiveMaterial;
	}

	void OnDisable()
	{
		//Messenger.RemoveListener( "set active switch material", SetActiveMaterial );
		HotDogScript.onActivateSwitch -= SetActiveMaterial;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetActiveMaterial()
	{
		renderer.material = switchMaterials[1];
	}
}
