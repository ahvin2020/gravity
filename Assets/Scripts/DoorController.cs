using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public string nextLevelName;
	private bool isCollided;

	void Start() {
		isCollided = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isCollided == false && other.tag == "Player") {
			PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();
			bool hasItem = playerInventory.hasItem(PlayerInventory.KEY_TAG);

			if (hasItem) {
				float playerRotation = other.gameObject.transform.localRotation.eulerAngles.z;
				float doorRotation = transform.localRotation.eulerAngles.z;

				if (Mathf.Abs(doorRotation - playerRotation) < 1) {
					isCollided = true;

					if (nextLevelName != null) {
						Application.LoadLevel(nextLevelName);
					}
				}
			}
		}
	}
}
