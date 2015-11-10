using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Info : NetworkBehaviour {

	private ArrayList CharacterRoster = new ArrayList();	// roster of my characters;; transforms

	// Use this for initialization
	void Start () {
		GameObject.Find ("MainUI").GetComponent<UI_Player>().player = this.gameObject;
	}

	public void AddCharacterToRoster( string name)
	{
		CharacterRoster.Add ( name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
