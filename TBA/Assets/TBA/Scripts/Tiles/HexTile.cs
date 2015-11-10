using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HexTile : NetworkBehaviour {

	// public parts
    public string Type;
    public bool cubeCoordSetUp = false;
	public bool visited = false;
    public Vector3 cubeCoord;
    public GameObject left, right, up_left, up_right, down_left, down_right;
	public ArrayList occupants = new ArrayList();
	public ArrayList occupantTeams = new ArrayList();
	public Vector3 centre;

	// private parts
	// private Mesh highlight;

	// Use this for initialization
	void Start () {
		centre = gameObject.transform.position + new Vector3( 32, 0, 0);
//		HighlightTile ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public ArrayList Neighbours( Moveable peep)
	{
		Character peepChar = peep.gameObject.GetComponent<Character> ();
		bool peepCanPathThroughEnemies = peep.canPathThroughEnemies;

		ArrayList neighbourhood = new ArrayList ();
		if (left != null && !peep.unpathables.Contains(left.GetComponent<HexTile>().Type)
		    && ( peepCanPathThroughEnemies 
		    ||   !ArrayListsIntersect( left.GetComponent<HexTile>().occupantTeams, peepChar.enemyTeams)))
			neighbourhood.Add (left);
		if (right != null && !peep.unpathables.Contains(right.GetComponent<HexTile>().Type)
		    && ( peepCanPathThroughEnemies 
		    ||   !ArrayListsIntersect( right.GetComponent<HexTile>().occupantTeams, peepChar.enemyTeams)))
			neighbourhood.Add (right);
		if (up_left != null && !peep.unpathables.Contains(up_left.GetComponent<HexTile>().Type)
		    && ( peepCanPathThroughEnemies 
		    ||   !ArrayListsIntersect( up_left.GetComponent<HexTile>().occupantTeams, peepChar.enemyTeams)))
			neighbourhood.Add (up_left);
		if (up_right != null && !peep.unpathables.Contains(up_right.GetComponent<HexTile>().Type)
		    && ( peepCanPathThroughEnemies 
		    ||   !ArrayListsIntersect( up_right.GetComponent<HexTile>().occupantTeams, peepChar.enemyTeams)))
			neighbourhood.Add (up_right);
		if (down_right != null && !peep.unpathables.Contains(down_right.GetComponent<HexTile>().Type)
		    && ( peepCanPathThroughEnemies 
		    ||   !ArrayListsIntersect( down_right.GetComponent<HexTile>().occupantTeams, peepChar.enemyTeams)))
			neighbourhood.Add (down_right);
		if (down_left != null && !peep.unpathables.Contains(down_left.GetComponent<HexTile>().Type)
		    && ( peepCanPathThroughEnemies 
		    ||   !ArrayListsIntersect( down_left.GetComponent<HexTile>().occupantTeams, peepChar.enemyTeams)))
			neighbourhood.Add (down_left);
		
		return neighbourhood;
	}

	public ArrayList Neighbours()
	{
		ArrayList neighbourhood = new ArrayList ();
		if (left != null)
			neighbourhood.Add (left);
		if (right != null)
			neighbourhood.Add (right);
		if (up_left != null)
			neighbourhood.Add (up_left);
		if (up_right != null)
			neighbourhood.Add (up_right);
		if (down_right != null)
			neighbourhood.Add (down_right);
		if (down_left != null)
			neighbourhood.Add (down_left);

		return neighbourhood;
	}

	// highlight the til with the sprite attached
	// this is still a work in progress
	public void HighlightTile()
	{

	}

	public bool ArrayListsIntersect( ArrayList a1, ArrayList a2)
	{
		foreach (Object obj in a1) {
			if ( a2.Contains( obj))
				return true;
		}

		return false;
	}

	// Note:
	// This won't work because meshes are assholes.
	// We will use sprites instead. Should be easy enough.
//	public void HighlightTile()
//	{
//		Mesh mesh = new Mesh ();
//		GetComponent<MeshFilter> ().mesh = mesh;
//
//		// this is garbage
//		Vector3 offsetX = new Vector3( 64,  0,  0);
//		Vector3 offsetY = new Vector3( 0,  48,  0);
//		Vector3 tilePos = gameObject.transform.position;
//
//		offsetX *= tilePos.x / 64;
//		offsetY *= tilePos.y / 48;
//
//		Vector3 totalOffset = offsetX + offsetY;
//
//		// psa this code is readable
//		Vector3 uv, ulv, urv, dv, drv, dlv;
//		drv = hex_corner_int ( CornerDirection.dr ) - totalOffset;
//		dv  = hex_corner_int ( CornerDirection.d  ) - totalOffset;
//		dlv = hex_corner_int ( CornerDirection.dl ) - totalOffset;
//		ulv = hex_corner_int ( CornerDirection.ul ) - totalOffset;
//		uv  = hex_corner_int ( CornerDirection.u  ) - totalOffset;
//		urv = hex_corner_int ( CornerDirection.ur ) - totalOffset;
//
//		Vector2 druv, duv, dluv, uluv, uuv, uruv;
//		druv = new Vector2 ( 1.0f, 0.25f );
//		duv  = new Vector2 ( 0.5f, 0f    );
//		dluv = new Vector2 ( 0f,   0.25f );
//		uluv = new Vector2 ( 0f,   0.75f );
//		uuv  = new Vector2 ( 0.5f, 1.0f  );
//		uruv = new Vector2 ( 1.0f, 0.75f );
//
//		Debug.Log (gameObject.transform.position);
//
//		Vector3[] vertices  = new Vector3[] { drv,  dv,  dlv,  ulv,  uv,  urv    };
//		Vector2[] uvs       = new Vector2[] { druv, duv, dluv, uluv, uuv, uruv   };
//		int[]     triangles = new int[]     { 0, 1, 2, 0, 2, 3, 3, 5, 0, 3, 4, 5 };
//
//		mesh.vertices = vertices;
//		mesh.uv = uvs;
//		mesh.triangles = triangles;
//
//		Color32 newColor = new Color32 (255, 50, 50, 25);
//		mesh.colors32 = new Color32[] { newColor, newColor, newColor, newColor, newColor, newColor };
//
//		Material material = new Material(Shader.Find("Custom/Vertex Colored"));
////		material.color = Color.green;
//		GetComponent<Renderer>().material = material;
//
////		GetComponent<MeshRenderer> ().enabled = false;
//
//	}

	public void UnHighlightTile()
	{
		GetComponent<MeshFilter> ().mesh.Clear ();
	}

	// | 					| //
	// |	Hex Methods		| //
	// |					| //

	//  PSEUDOCODE for corner method
	//	function hex_corner(center, size, i):
	//		var angle_deg = 60 * i   + 30
	//			var angle_rad = PI / 180 * angle_deg
	//			return Point(center.x + size * cos(angle_rad),
	//			             center.y + size * sin(angle_rad))

	Vector3 hex_corner( CornerDirection i)
	{
		int angle_deg = 60 * (int)i + 30;
		float angle_rad = Mathf.PI / 180.0f * angle_deg;
		return new Vector3 ( gameObject.transform.position.x + (int)(32 * Mathf.Cos (angle_rad)),
		                     gameObject.transform.position.y + (int)(32 * Mathf.Sin (angle_rad)),
		                     gameObject.transform.position.z );
	}

	Vector3 hex_corner_int ( CornerDirection i)
	{
		switch (i) {
		case CornerDirection.dr:
			return gameObject.transform.position + new Vector3( 32, -16, 0) + new Vector3( 32, -16, 0);
		case CornerDirection.d:
			return gameObject.transform.position + new Vector3( 0, -32, 0) + new Vector3( 32, -16, 0);
		case CornerDirection.dl:
			return gameObject.transform.position + new Vector3( -32, -16, 0) + new Vector3( 32, -16, 0);
		case CornerDirection.ul:
			return gameObject.transform.position + new Vector3( -32, 16, 0) + new Vector3( 32, -16, 0);
		case CornerDirection.u:
			return gameObject.transform.position + new Vector3( 0,  32, 0) + new Vector3( 32, -16, 0);
		case CornerDirection.ur:
			return gameObject.transform.position + new Vector3( 32, 16, 0) + new Vector3( 32, -16, 0);
		default: 
			return gameObject.transform.position;
		}
	}

}
