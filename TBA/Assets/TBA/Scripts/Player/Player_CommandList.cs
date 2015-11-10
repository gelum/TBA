using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/*
 * 	Script with a list of commands the player may run. 
 * 
 *  Call methods by first finding the local player, then running the given command.
 * 
 * */
public class Player_CommandList : NetworkBehaviour {

	/*
	 * 	Spawn Commands
	 * 
	 * */

	[Command]
	public void CmdSpawnPawnMan( string pid)
	{
		GameObject pawnMan = GameObject.Find("GameMaster").GetComponent<RosterManager>().pawnMan;
		Vector3 spawnLoc = new Vector3( 0,0,0);
		if ( GetComponent<Player_UID>().myTeam == Team.Left)
			spawnLoc = GameObject.Find ("GameMaster").GetComponent<Level_Controller_SplitCastle>().leftSpawn.transform.position;
		else
			spawnLoc = GameObject.Find ("GameMaster").GetComponent<Level_Controller_SplitCastle>().rightSpawn.transform.position;
		GameObject go = Instantiate( pawnMan, spawnLoc + new Vector3( 32, 0, 0), Quaternion.identity) as GameObject;
		go.GetComponent<Character>().ownerID = pid;
		NetworkServer.Spawn( go);
		RpcUpdateTileWithNewSpawn( GetComponent<Player_UID>().myTeam == Team.Left, go);
	}
	
	[ClientRpc]
	private void RpcUpdateTileWithNewSpawn( bool isLeft, GameObject go)
	{
		Debug.Log("RPC Called");
		if ( isLeft)
		{
			go.GetComponent<Moveable>().occupying = GameObject.Find ("GameMaster").GetComponent<Level_Controller_SplitCastle>().leftSpawn.GetComponent<HexTile>();
			go.GetComponent<Moveable>().occupying.occupants.Add( go);
		}
		else
		{
			go.GetComponent<Moveable>().occupying = GameObject.Find ("GameMaster").GetComponent<Level_Controller_SplitCastle>().rightSpawn.GetComponent<HexTile>();
			go.GetComponent<Moveable>().occupying.occupants.Add( go);
		}
	}
//	[Command]
//	public void CmdSpawnYellowAlien( string pid)
//	{
//		GameObject alienYellow = GameObject.Find("GameMaster").GetComponent<RosterManager>().alienYellow;
//		Vector3 spawnLoc = GameObject.Find("StartTest").GetComponent<HexTile>().transform.position;
//		GameObject go = Instantiate( alienYellow, spawnLoc + new Vector3( 32, 0, 0), Quaternion.identity) as GameObject;
//		go.GetComponent<Character>().ownerID = pid;
//		NetworkServer.Spawn( go);
//		RpcUpdateTileWithNewSpawn( "StartTest", go);
//	}
//	
//	[Command]
//	public void CmdSpawnGreenAlien( string pid)
//	{
//		GameObject alienGreen = GameObject.Find("GameMaster").GetComponent<RosterManager>().alienGreen;
//		Vector3 spawnLoc = GameObject.Find("EndTest").GetComponent<HexTile>().transform.position;
//		GameObject go = Instantiate( alienGreen, spawnLoc + new Vector3( 32, 0, 0), Quaternion.identity) as GameObject;
//		go.GetComponent<Character>().ownerID = pid;
//		NetworkServer.Spawn( go);
//		RpcUpdateTileWithNewSpawn( "EndTest", go);
//	}
//
//	[ClientRpc]
//	private void RpcUpdateTileWithNewSpawn( string name, GameObject go)
//	{
//		Debug.Log("RPC Called");
//		GameObject.Find(name).GetComponent<HexTile>().occupants.Add( go);
//		go.GetComponent<Moveable>().occupying = GameObject.Find("StartTest").GetComponent<HexTile>();
//	}

	/*
	 * 	Move Commands
	 * 
	 * */
	[Command]
	public void CmdMoveUnitToTile( GameObject unit, GameObject dest)
	{
		Pathfinder finder = GameObject.Find ("GameMaster").GetComponent<Pathfinder>();
		Dictionary<HexTile, HexTile> thePath = finder.GetPathToTile( unit.GetComponent<Moveable>(),
		                                                             unit.GetComponent<Moveable>().occupying,
		                                                             dest.GetComponent<HexTile>());

//		unit.transform.position = dest.transform.position + new Vector3( 32, 0, 0);

		Debug.Log( thePath.Count + " elements. " + thePath.ToString());

//		foreach( KeyValuePair<HexTile, HexTile> tile in thePath)
//		{
//			Debug.Log( tile.Key.transform.position + " to " + tile.Value.transform.position);
//		}

		HexTile theTile = dest.GetComponent<HexTile>();
		int j = 0;
		ArrayList pathOrder = new ArrayList() { theTile.gameObject};
		while (theTile != unit.GetComponent<Moveable>().occupying)
		{
			theTile = thePath[ theTile];
			pathOrder.Add ( theTile.gameObject);
			Debug.Log( j++ + ": " + theTile);
		}

		pathOrder.Reverse ();

		unit.GetComponent<Moveable>().occupying.occupants.Remove( unit);
		unit.GetComponent<Moveable>().pathToTake = pathOrder;

//		unit.GetComponent<Moveable>().occupying = dest.GetComponent<HexTile>();
//		dest.GetComponent<HexTile>().occupants.Add( unit);
//		unit.transform.position = dest.transform.position;

		RpcIssueMoveOrderToClients( unit, dest);
	}

	// update variables local to each client
	[ClientRpc]
	private void RpcIssueMoveOrderToClients( GameObject unit, GameObject dest)
	{
		// no more teleporting
		Pathfinder finder = GameObject.Find ("GameMaster").GetComponent<Pathfinder>();
		Dictionary<HexTile, HexTile> thePath = finder.GetPathToTile( unit.GetComponent<Moveable>(),
		                                                            unit.GetComponent<Moveable>().occupying,
		                                                            dest.GetComponent<HexTile>());
		
		//		unit.transform.position = dest.transform.position + new Vector3( 32, 0, 0);
		
		Debug.Log( thePath.Count + " elements. " + thePath.ToString());
		
		//		foreach( KeyValuePair<HexTile, HexTile> tile in thePath)
		//		{
		//			Debug.Log( tile.Key.transform.position + " to " + tile.Value.transform.position);
		//		}
		
		HexTile theTile = dest.GetComponent<HexTile>();
		int j = 0;
		ArrayList pathOrder = new ArrayList() { theTile.gameObject};
		while (theTile != unit.GetComponent<Moveable>().occupying)
		{
			theTile = thePath[ theTile];
			pathOrder.Add ( theTile.gameObject);
			Debug.Log( j++ + ": " + theTile);
		}
		
		pathOrder.Reverse ();

		unit.GetComponent<Moveable>().occupying.occupants.Remove( unit);
		unit.GetComponent<Moveable>().pathToTake = pathOrder;

		// for now we teleport
//		unit.GetComponent<Moveable>().occupying.occupants.Remove( unit);
//		unit.GetComponent<Moveable>().pathToTake = pathOrder;
//		unit.GetComponent<Moveable>().occupying = dest.GetComponent<HexTile>();
//		unit.GetComponent<Moveable>().pathToTake = pathOrder;
//		dest.GetComponent<HexTile>().occupants.Add( unit);
//		unit.GetComponent<Moveable>().pathToTake = pathOrder;

//		unit.transform.position = dest.transform.position; // handled by server
	}

}
