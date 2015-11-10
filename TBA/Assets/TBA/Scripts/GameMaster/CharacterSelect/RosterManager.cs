using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RosterManager : NetworkBehaviour {

	public GameObject alienGreen;
	public GameObject alienYellow;
	public GameObject pawnMan;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	/*	
	 * 	Spawn aliens for given player.
	 * 	PlayerIDs are unique.
	 * 	[TODO] some more consideration for the select screen.
	 * 
	 * */

//	[Command]
//	public void CmdSpawnAlienGreen( )
//	{
//		Vector3 spawnLoc = GameObject.Find("StartTest").GetComponent<HexTile>().transform.position;
//		GameObject go = Instantiate( alienGreen, spawnLoc + new Vector3( 32, -32, 0), Quaternion.identity) as GameObject;
//		NetworkServer.Spawn( go);
//	}
//
//	[Command]
//	public void CmdSpawnAlienYellow( )
//	{
//		Vector3 spawnLoc = GameObject.Find("EndTest").GetComponent<HexTile>().transform.position;
//		GameObject go = Instantiate( alienYellow, spawnLoc + new Vector3( 32, -32, 0), Quaternion.identity) as GameObject;
//		NetworkServer.Spawn( go);
//	}
}
