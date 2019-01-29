using UnityEngine;
using System.Collections;

public class SignController : MonoBehaviour {

	public GUIText TutorialText;
	public string Message;

	private bool isColliding;

	// Use this for initialization
	void Start () {
		isColliding = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isColliding == false && other.tag == "Player") {
			isColliding = true;
			
			TutorialText.text = Message.Replace("___","\n");;
			TutorialText.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (isColliding == true && other.tag == "Player") {
			isColliding = false;
			TutorialText.enabled = false;
		}
	}
}
