using UnityEngine;
using System.Collections;

public class LevelMenuItem : MonoBehaviour {

	public string levelName;
	private bool isClicked;

	// Use this for initialization
	void Start () {
		isClicked = false;	
	}

//	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			isClicked = true;
		} else if (Input.GetMouseButtonUp(0) && isClicked == true) {
			isClicked = false;

			if (guiText.GetScreenRect().Contains(Input.mousePosition) && levelName != null) {
				Application.LoadLevel(levelName);
			}
		}
	}
}
