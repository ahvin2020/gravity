using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

	public PlayerController playerController;
	private Animator animator;

	void Start() {
		playerController = gameObject.GetComponent<PlayerController>();
		animator = gameObject.GetComponent<Animator>();
	}

	public void setPlayerDead() {
//		playerController.changeDirection((int)PlayerController.Direction.DOWN, false); // make player fall to floor
		playerController.enabled = false; // disable player controller

		animator.SetTrigger("Die");

		StartCoroutine("reloadLevel");
	}

	private IEnumerator reloadLevel() {
		yield return new WaitForSeconds(1);
		Application.LoadLevel(Application.loadedLevel);
	}
}
