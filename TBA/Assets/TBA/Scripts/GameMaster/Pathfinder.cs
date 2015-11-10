using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

	// public parts ;; Testing
	// 				;; Uncomment as needed
//	public GameObject test;
//	public GameObject sourceTest;
//	public GameObject destTest;

	// private parts
	private HexTile lastProcessedDestination;	// keep track of last looked for destination tile
												// in order to prune unnecessary computations

	// private parts ;; A* Searching
//	private Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
//	private Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();

	
	// Use this for initialization
	void Start () {
		// debugging ;; Uncomment as needed
//		ArrayList things = reachables (GameObject.Find ("alienYellow").GetComponent<Moveable> (), 
//		                               sourceTest.GetComponent<HexTile>());
//
//		Debug.Log ("NumReachables: " + things.Count);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public Dictionary<HexTile, HexTile> GetPathToTile( Moveable peep, HexTile source, HexTile destination)
	{	
		Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
		Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();

		var frontier = new PriorityQueue<HexTile>();
		frontier.Enqueue(source, 0);
		
		cameFrom[source] = source;
		costSoFar[source] = 0;
		
		while (frontier.Count > 0)
		{
			var current = frontier.Dequeue();
			
			if (current.Equals(destination))
			{
				break;
			}
			
			foreach (var next in current.Neighbours( peep))
			{
				GameObject nextObj = (GameObject) next;
				HexTile nextTile = nextObj.GetComponent<HexTile>();
				int newCost = costSoFar[current] + Cost( current, nextTile);
				if (!costSoFar.ContainsKey(nextTile) || newCost < costSoFar[nextTile])
				{
					costSoFar[nextTile] = newCost;
					int priority = newCost + Heuristic(nextTile, destination);
					frontier.Enqueue(nextTile, priority);
					cameFrom[nextTile] = current;
				}
			}
		}

		return cameFrom;
	}

	// This cost will depend on Tile stats, when we get there.
	// Update as needed, so long as its always an integer.
	int Cost ( HexTile a, HexTile b)
	{
		return 1;
	}

	void ShowPathToTile( Moveable peep, HexTile source, HexTile destination)
	{
		
	}
	
	// Rechables pseudocode
	/*
	function cube_reachable(start, movement):
	    var visited = set()
	    add start to visited
	    var fringes = []
	    fringes.append([start])

	    for each 1 < k ≤ movement:
	        fringes.append([])
	        for each cube in fringes[k-1]:
	            for each 0 ≤ dir < 6:
	                var neighbor = cube_neighbor(cube, dir)
	                if neighbor not in visited, not blocked:
	                    add neighbor to visited
	                    fringes[k].append(neighbor)

	    return visited
	 */
	public ArrayList reachables( Moveable peep, HexTile root)
	{
		ArrayList visited = new ArrayList ();					// list of visited noted
		visited.Add (root);										// add root for obvious reasons
		ArrayList fringes = new ArrayList ( peep.range + 1);	// list of lists
																// fringes[k] = all nodes reachable in k
		fringes.Add (new ArrayList() { root});					// add [ root] to fringes[ 0]

		for (int i = 1; i <= peep.range; i++) {
			fringes.Add( new ArrayList());
			foreach( HexTile tile in (ArrayList)fringes[ i-1])
			{
				foreach( GameObject neighbourGO in tile.Neighbours( peep))
				{
					HexTile neighbour = neighbourGO.GetComponent<HexTile>();
					if ( !visited.Contains( neighbour))
					{
						visited.Add ( neighbour);
						((ArrayList) fringes[ i]).Add(neighbour);
					}
				}
			}
		}

		return visited;

	}

	// show all paths
	// this should highlight all available destinations the current character can go to.
	void ShowAllPaths( Moveable peep, HexTile root, int depth)
	{

	}

	//		Support methods for pathfinding

	// Heuristic method to help with pruning in A* algorithm.
	// Compute the cube coord distance between both tiles, and use that for heuristics.
	public static int Heuristic(HexTile a, HexTile b)
	{
		return (int) ((Math.Abs (a.cubeCoord.x - b.cubeCoord.x) + Math.Abs (a.cubeCoord.y - b.cubeCoord.y)
		        + Math.Abs (a.cubeCoord.z - b.cubeCoord.z)) / 2);
	}
}
