using UnityEngine;

public class MagnetConfig {
	public Vector2 gravityDirection;
	public float rotation;
	public Vector2 jumpDirection;
	public float moveDirection;

	public MagnetConfig (Vector2 gravityDirection, float rotation, Vector2 jumpDirection, float moveDirection) {
		this.gravityDirection = gravityDirection;
		this.rotation = rotation;
		this.jumpDirection = jumpDirection;
		this.moveDirection = moveDirection;
	}
}