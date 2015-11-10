using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_UID : NetworkBehaviour {

	[SyncVar] public string playerUniqueIdentifier;
	[SyncVar] public Team myTeam = Team.Left;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer ()
	{
		GetNetIdentity();
		SetIdentity();
		if (isServer)
		{
			myTeam = Team.Right;
		}

		GetComponent<Player_CommandList>().CmdSpawnPawnMan( MakeUniqueIdentity());
	}

	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if ( myTransform.name == "" || myTransform.name == "Player(Clone)")
		{
			SetIdentity();
		}
	}

	[Client] 
	void GetNetIdentity()
	{
		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerMyIdentity( MakeUniqueIdentity());
	}

	string MakeUniqueIdentity()
	{
		string uniqueName = "Player " + playerNetID.ToString();
		return uniqueName;
	}

	[Command]
	void CmdTellServerMyIdentity( string name)
	{
		playerUniqueIdentifier = name;
	}
	
	void SetIdentity()
	{
		if ( !isLocalPlayer)
		{
			myTransform.name = playerUniqueIdentifier;
		}
		else
		{
			myTransform.name = MakeUniqueIdentity();
		}
	}
}
