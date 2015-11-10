using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Interact : NetworkBehaviour {

	[SerializeField] Camera pCam;
	private RaycastHit2D hit;

	// selection info
	public GameObject selectedObject;			// reference to the last selected object
	public bool isMyObject;						// does the object belong to me?
	public bool unitIsSelected = false;			// did i select a unit lately

	// Update is called once per frame
	void Update () {
		if ( isLocalPlayer)
		{
			CheckForInput();
		}
	}

	void CheckForInput()
	{
		if ( Input.GetMouseButtonUp( 0))	// on left click
		{
			SelectTileAtCursor();
		}
	}

	void SelectTileAtCursor ()
	{
		Vector2 position = pCam.ScreenToWorldPoint( Input.mousePosition);

		// Ignore the default mask
		// int layerMask = ~(1 << LayerMask.NameToLayer("Default"));
		hit = Physics2D.Raycast(position, Vector2.zero, 0);//, layerMask);

		if (hit)
		{
			GameObject targetObj = hit.transform.gameObject;

			if ( !unitIsSelected)
			{
				if ( targetObj.GetComponent<HexTile>().occupants.Count == 0)
					return;

				unitIsSelected = true;
				GameObject target = targetObj.GetComponent<HexTile>().occupants[0] as GameObject;
				string myPID = GetComponent<Player_UID>().playerUniqueIdentifier;
				selectedObject = target;

				if ( target.GetComponent<Character>().ownerID == myPID)
				{
					Debug.Log ( "This is mine.");
					isMyObject = true;
				}
				else
				{
					Debug.Log ( "This is not mine.");
					isMyObject = false;
				}
			}
			else
			{
				// we have a unit selected
				// we must move to the selected tile if possible

				// check if we can move to the tile
				if ( isMyObject && canPathToTile( selectedObject.GetComponent<Moveable>().occupying, 
				                    targetObj.GetComponent<HexTile>(), 
				                   	selectedObject))
				{
					// actually path to tile
					initiateMoveToTile( selectedObject.GetComponent<Moveable>().occupying,
					                    targetObj.GetComponent<HexTile>(),
					                    selectedObject);
				}
				else if ( !isMyObject)
				{
					unitIsSelected = false;
					selectedObject = null;
				}
			}
		}
	}

	private void initiateMoveToTile( HexTile src, HexTile dst, GameObject unit)
	{
//		GameObject gm = GameObject.Find("GameMaster");
//		Pathfinder pf = gm.GetComponent<Pathfinder>();

		GetComponent<Player_CommandList>().CmdMoveUnitToTile( unit, src.gameObject, dst.gameObject);
	}

	private bool canPathToTile( HexTile src, HexTile dst, GameObject unit)
	{
		GameObject gm = GameObject.Find("GameMaster");
		Pathfinder pf = gm.GetComponent<Pathfinder>();

		ArrayList reachables = pf.reachables( unit.GetComponent<Moveable>(), src);

		if ( reachables.Contains( dst))
			return true;

		return false;
	}

}
