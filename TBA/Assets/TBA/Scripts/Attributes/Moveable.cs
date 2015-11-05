using UnityEngine;
using System.Collections;

public class Moveable : MonoBehaviour {

	public ArrayList unpathables = new ArrayList() {"Empty"}; // array of strings of names of unpathable tiles
	public int range;
	public int remRange;
	public bool canPathThroughEnemies;
	public HexTile occupying;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
