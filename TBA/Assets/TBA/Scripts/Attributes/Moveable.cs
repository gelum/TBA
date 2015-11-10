using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Moveable : NetworkBehaviour {

	[SyncVar] Vector3 syncPos;
	public ArrayList unpathables = new ArrayList() {"Empty"}; // array of strings of names of unpathable tiles
	[SyncVar] public ArrayList pathToTake = new ArrayList();
	private HexTile pathingTo = null;
	public int range;
	public int remRange;
	public bool canPathThroughEnemies;
	public HexTile occupying;
	public float speed;

	// Use this for initialization
	void Start () {
//		myTransform = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
//		if ( !isServer)
//		{

		if ( pathToTake != null && pathToTake.Count != 0 && pathingTo == null)
		{
			Debug.Log("Hi");
			GetComponent<Animator>().Play ("pawn_walk");
			pathingTo = ((GameObject)pathToTake[ 0]).GetComponent<HexTile>();
			pathingTo.occupants.Remove( gameObject);

			float checkMe = occupying.centre.x - pathingTo.centre.x;
			if ( checkMe < 0)
			{
				transform.localScale = new Vector3( -50, 50, 1);
			}
			else if ( checkMe > 0)
			{
				transform.localScale = new Vector3(  50, 50, 1);
			}

			pathToTake.RemoveAt( 0);
		}

		if ( pathingTo != null)
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards( transform.position, pathingTo.centre, step);

			if ( transform.position == pathingTo.centre)
			{
				pathingTo.occupants.Add ( gameObject);
				occupying = pathingTo;
				pathingTo = null;
				GetComponent<Animator>().Play ("pawn_idle");
			}
		}
//		} else {
//			LerpPosition();
//		}
	}

	void LerpPosition()
	{
		transform.position = Vector3.Lerp ( transform.position, syncPos, Time.deltaTime*speed);
	}

	[Command]
	void CmdProvideServerWithSyncPos( Vector3 pos)
	{

	}
	
}
