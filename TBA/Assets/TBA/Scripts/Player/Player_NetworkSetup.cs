using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField] Camera cam;
	[SerializeField] AudioListener listener;
	private Vector3 spawn_point = new Vector3( 500, -350, -1);

	// Use this for initialization
	void Start () {
		if ( isLocalPlayer)
		{
			GameObject.Find ("Scene Camera").SetActive( false);

			GetComponent<Player_Info>().enabled = true;
			GetComponent<Player_CameraControl>().enabled = true;
			GetComponent<Player_Interact>().enabled = true;
			listener.enabled = true;
			cam.enabled = true;

			transform.position = spawn_point;
		}
	}

}
