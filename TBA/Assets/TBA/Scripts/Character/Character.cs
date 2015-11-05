using UnityEngine;
using System.Collections;

public enum Team { Left, Right, Boss };

public class Character : MonoBehaviour {

	public Team team;				// which team are you on
	public ArrayList enemyTeams;	// which teams are you against
	public float hp;				// how much hp do you have

	// Use this for initialization
	void Start () {
		
	}

	void setTeam ( Team newTeam)
	{
		team = newTeam;

		enemyTeams = new ArrayList () { Team.Left, Team.Right, Team.Boss };
		enemyTeams.Remove (newTeam);	//fuck
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
