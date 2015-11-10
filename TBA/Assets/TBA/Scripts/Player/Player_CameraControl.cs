using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_CameraControl : NetworkBehaviour {
	
	float cumulativeZ;
	[SerializeField] Camera pCam;
	
	// Use this for initialization
	void Start () { }   
	
	// Update is called once per frame
	void Update () {
//		float zMin = 100f;
//		float zMax = 300f;
		float xMin = 0f;
		float xMax = 600f;
		float yMax = 0f;
		float yMin = -600f;
		
		//x, y, z movement
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");
		float zAxisValue = Input.GetAxis("Mouse ScrollWheel") * 20f;//*10: Increase speed
		
		//z movement:page up, page down
		if (Input.GetKey(KeyCode.PageUp))
			zAxisValue += 1.5f;
		else if (Input.GetKey(KeyCode.PageDown))
			zAxisValue -= 1.5f;
		//        Debug.Log(Camera.main.transform.position);
		
		zAxisValue *= 0.35f;
		
		//moving the camera
		if (pCam != null)
		{
			//the y for the camera is the z for the world
			Vector3 saveMe = pCam.transform.position;
			
			if ( saveMe.x + xAxisValue < xMax && saveMe.x + xAxisValue > xMin &&
			    saveMe.y + yAxisValue < yMax && saveMe.y + yAxisValue > yMin)
				saveMe = new Vector3( saveMe.x + xAxisValue, saveMe.y + yAxisValue, saveMe.z);
			
			//			if ( saveMe.y + zAxisValue*0.8f < zMax && saveMe.y + zAxisValue*0.8f > zMin)
			//				saveMe = new Vector3( saveMe.x, saveMe.y + zAxisValue, saveMe.z);
			
			pCam.transform.position = saveMe;
			
			if ( cumulativeZ + zAxisValue > -50 && cumulativeZ + zAxisValue < 50)
			{
				pCam.orthographicSize += Time.deltaTime * zAxisValue * 30.0f;
				cumulativeZ += zAxisValue;
			}
			
			//            if ((Camera.main.transform.position.y >= zMax) && (zAxisValue < 0))
			//                Camera.main.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0)); 
			//            else if ((Camera.main.transform.position.y <= zMin) && (zAxisValue > 0)) 
			//                Camera.main.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0));
			//            else
			//                Camera.main.transform.Translate(new Vector3(xAxisValue, yAxisValue, zAxisValue));
		}
	}
}
