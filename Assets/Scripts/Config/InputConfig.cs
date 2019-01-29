using UnityEngine;

public class InputConfig {
	public int direction;
	public Vector2 vectorDirection;
	
	public InputConfig (int  direction, Vector2 vectorDirection) {
		this.direction = direction;
		this.vectorDirection = vectorDirection;
	}
}