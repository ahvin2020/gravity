using UnityEngine;
using System.Collections;

public class SpikeController : MonoBehaviour {
	
	private bool isCollided;
	
	void Start() {
		isCollided = false;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (isCollided == false && other.tag == "Player") {
			isCollided = true;

			PlayerDeath playerDeath = other.gameObject.GetComponent<PlayerDeath>();
			playerDeath.setPlayerDead();
		}
	}
}
