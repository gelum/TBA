using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * 	Script to handle the UI for the current player.
 * 	Players plug themselves in to the UI when they are created, using the isLocalPlayer check
 *  to ensure only one player prefab is registered to a given UI.
 * 
 *  The UI then issues [Command]s directly from the player by way of its reference to that player.
 *  i.e. the UI is driving the registered player.
 * 
 *  This will be used to deal with abilities on the UI, and also for testing purposes.
 * 
 * */
public class UI_Player : NetworkBehaviour {

	public GameObject player;	// The Player Prefab registered with this UI.

}
