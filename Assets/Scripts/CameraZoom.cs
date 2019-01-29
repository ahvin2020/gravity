using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	int zoom = 10;
	int normal = 40;
	float smooth = 5;
	private bool isZoomed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

//			if(Input.GetKeyDown("z")){
//			Debug.Log ("here");
//				isZoomed = !isZoomed;
//			}
//			
//			if(isZoomed == true){
//			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize ,zoom,Time.deltaTime*smooth);
//			}
//			else{
//			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize ,normal,Time.deltaTime*smooth);
//			}
	}
}
