using UnityEngine;
using System.Collections;

public class LiveArea : MonoBehaviour {

	public bool isHide = true;
	private bool isExited;
	
	void Start() {
		isExited = false;
		renderer.enabled = !isHide;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (isExited == false && other.tag == "Player") {
			isExited = true;
			
			PlayerDeath playerDeath = other.gameObject.GetComponent<PlayerDeath>();
			playerDeath.setPlayerDead();
		}
	}
}
