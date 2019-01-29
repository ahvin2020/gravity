using UnityEngine;
using System.Collections;

public class PlayArea : MonoBehaviour {

	public bool isHide = true;
	
	void Start() {
		renderer.enabled = !isHide;
	}
}
