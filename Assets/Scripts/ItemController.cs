using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			// player got the key now
			PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();
			playerInventory.obtainItem(this.tag);

			// disable and hide key
			this.gameObject.SetActive(false);
		}
	}
}
