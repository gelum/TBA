using UnityEngine;
using System.Collections;

public class Level_Processor : MonoBehaviour {

    public GameObject centreTile;

	// Use this for initialization
	void Start () {

        // we are using cube coordinates for hexes.
        // look this up/ask alexi as necessary
        centreTile.GetComponent<HexTile>().cubeCoord = new Vector3( 0, 0, 0);

        // set up the adjacency for the tiles.
        // uncomment as needed
        // SetUpAdjacency();

        SetUpCubeCoords(centreTile.GetComponent<HexTile>(), new Vector3( 0, 0, 0));

	}
    
    void SetUpCubeCoords( HexTile root, Vector3 theCoord)
    {
        if ( !root.cubeCoordSetUp)
        {
            root.cubeCoordSetUp = true;
            root.cubeCoord = theCoord;

            // set up left
            Vector3 theNewCoord = theCoord + new Vector3(1, -1, 0);
            if ( root.left != null)
                SetUpCubeCoords(root.left.GetComponent<HexTile>(), theNewCoord);

            // set up up left
            theNewCoord = theCoord + new Vector3(1, 0, -1);
            if (root.up_left != null)
                SetUpCubeCoords(root.up_left.GetComponent<HexTile>(), theNewCoord);

            // set up up right
            theNewCoord = theCoord + new Vector3(0, 1, -1);
            if (root.up_right != null)
                SetUpCubeCoords(root.up_right.GetComponent<HexTile>(), theNewCoord);

            // set up right
            theNewCoord = theCoord + new Vector3(-1, 1, 0);
            if (root.right != null)
                SetUpCubeCoords(root.right.GetComponent<HexTile>(), theNewCoord);

            // set up down right
            theNewCoord = theCoord + new Vector3(-1, 0, 1);
            if (root.down_right != null)
                SetUpCubeCoords(root.down_right.GetComponent<HexTile>(), theNewCoord);

            // set up down left
            theNewCoord = theCoord + new Vector3(0, -1, 1);
            if (root.down_left != null)
                SetUpCubeCoords(root.down_left.GetComponent<HexTile>(), theNewCoord);
        }
    }

    // go through each tile and set their neighbours
    // up, down, upleft, upright, downleft and downright will be set
    // this method will also set up the cubeCoords on the various tiles
    // probably expensive so run once and save the new prefab	
    void SetUpAdjacency()
    {
        /* Algorithm ;; This is O( shit^2)

            foreach GameObject tagged Tile do
                left = Tile with centre = (myCentre.x - 64, myCentre.y)
                etc.
            endforeach
        */

        bool isFirst = true;

        foreach( GameObject obj in GameObject.FindGameObjectsWithTag("Tile"))
        {
            int objX = (int) obj.transform.position.x;
            int objY = (int) obj.transform.position.y;
            int objZ = (int) obj.transform.position.z;

            HexTile theScript = obj.GetComponent<HexTile>();

            Vector3 left = new Vector3(objX - 64, objY, objZ);
            Vector3 up_left = new Vector3(objX - 32, objY - 48, objZ);
            Vector3 up_right = new Vector3(objX + 32, objY - 48, objZ);
            Vector3 right = new Vector3(objX + 64, objY, objZ);
            Vector3 down_left = new Vector3(objX - 32, objY + 48, objZ);
            Vector3 down_right = new Vector3(objX + 32, objY + 48, objZ);

            // cannot go over y = 0
            // cannot go under x = (numTiles - 1)*width - width/2 // for lvl 1 thats 928

            foreach (GameObject objA in GameObject.FindGameObjectsWithTag("Tile"))
            {
                if ( isFirst)
                {
                    Debug.Log(objA.transform.position);
                    Debug.Log(right);
                }
                if (objA.transform.position.Equals(left))
                {
                    theScript.left = objA;
                }
                else if (objA.transform.position.Equals(right))
                {
                    theScript.right = objA;
                }
                else if (objA.transform.position.Equals(up_right))
                {
                    theScript.up_right = objA;
                }
                else if (objA.transform.position.Equals(up_left))
                {
                    theScript.up_left = objA;
                }
                else if (objA.transform.position.Equals(down_right))
                {
                    theScript.down_right = objA;
                }
                else if (objA.transform.position.Equals(down_left))
                {
                    theScript.down_left = objA;
                }
            }
            isFirst = false;

        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
