using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 2f;			// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 2f;			// Distance in the y axis the player can move before the camera follows.
	public float smooth = 2.5f;			// How smoothly the camera catches up with it's target movement in the x axis.

	private Vector2 playAreaMaxXAndY;	// The maximum x and y coordinates the for play area
	private Vector2 playAreaMinXAndY;	// The minimum x and y coordinates the for play area
	private Vector2 liveAreaMinXAndY;	// The minimum x and y coordinates the for live area
	private Vector2 liveAreaMaxXAndY;	// The minimum x and y coordinates the for live area
	private Transform player;			// Reference to the player's transform.
	private GameObject playArea;
	private GameObject liveArea;

	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
	
		// calculate min max x and y
		calculateMinMaxXAndY();

		// set the camera to focus on player
		float cameraPosX = Mathf.Clamp(player.position.x, playAreaMinXAndY.x, playAreaMaxXAndY.x);
		float cameraPosY = Mathf.Clamp(player.position.y, playAreaMinXAndY.y, playAreaMaxXAndY.y);
		transform.position = new Vector3(cameraPosX, cameraPosY, transform.position.z);
	}
	
	
	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}
	
	
	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}
	
	
	void FixedUpdate ()
	{
		TrackPlayer();
	}
	
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		Vector3 targetPosition = new Vector3(transform.position.x,transform.position.y, transform.position.z);
		
		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetPosition.x = player.position.x;//Mathf.Lerp(transform.position.x, player.position.x, smooth * Time.deltaTime);
		
		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetPosition.y = player.position.y;//Mathf.Lerp(transform.position.y, player.position.y, smooth * Time.deltaTime);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		if (playArea.renderer.bounds.Contains(player.position) || liveArea == null) {
			targetPosition.x = Mathf.Clamp(targetPosition.x, playAreaMinXAndY.x, playAreaMaxXAndY.x);
			targetPosition.y = Mathf.Clamp(targetPosition.y, playAreaMinXAndY.y, playAreaMaxXAndY.y);
		} else {
			targetPosition.x = Mathf.Clamp(targetPosition.x, liveAreaMinXAndY.x, liveAreaMaxXAndY.x);
			targetPosition.y = Mathf.Clamp(targetPosition.y, liveAreaMinXAndY.y, liveAreaMaxXAndY.y);
		}

		transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
	}

	private void calculateMinMaxXAndY() {
		float minX, minY, maxX, maxY;

		// camera
		Camera camera = Camera.main;
		float cameraHeight = 2f * camera.orthographicSize;
		float cameraWidth = cameraHeight * camera.aspect;
		
		// --- get playAreaMinXAndY, playAreaMaxXAndY ---
		playArea = GameObject.Find("PlayArea");
		float playAreaWidth = playArea.renderer.bounds.size.x;
		float playAreaHeight = playArea.renderer.bounds.size.y;

		// camera width bigger than play area width?
		if (cameraWidth > playAreaWidth) {
			// yes! set camera minX and maxX to be play area position x
			minX = maxX = playArea.transform.position.x;
		} else {
			// no! calculate get minX and maxX
			minX = playArea.transform.position.x - playAreaWidth * 0.5f + cameraWidth * 0.5f;
			maxX = playArea.transform.transform.position.x + playAreaWidth * 0.5f - cameraWidth * 0.5f;
		}

		// camera height bigger than play area height?
		if (cameraHeight > playAreaHeight) {
			// yes! set camera minY and maxY to be play area position y
			minY = maxY = playArea.transform.position.y;
		} else {
			// no! calculate get minY and maxY
			minY = playArea.transform.position.y - playAreaHeight * 0.5f + cameraHeight * 0.5f;
			maxY = playArea.transform.position.y + playAreaHeight * 0.5f - cameraHeight * 0.5f;
		}

		playAreaMinXAndY = new Vector2(minX, minY);
		playAreaMaxXAndY = new Vector2(maxX, maxY);

		// --- get liveAreaMinXAndY, liveAreaMaxXAndY
		liveArea = GameObject.Find("LiveArea");
		if (liveArea) {
			float liveAreaWidth = liveArea.renderer.bounds.size.x;
			float liveAreaHeight = liveArea.renderer.bounds.size.y;

			minX = liveArea.transform.position.x - liveAreaWidth * 0.5f + cameraWidth * 0.5f;
			maxX = liveArea.transform.position.x + liveAreaWidth * 0.5f - cameraWidth * 0.5f;

			minY = liveArea.transform.position.y - liveAreaHeight * 0.5f + cameraHeight * 0.5f;
			maxY = liveArea.transform.position.y + liveAreaHeight * 0.5f - cameraHeight * 0.5f;

			liveAreaMinXAndY = new Vector2(minX, minY);
			liveAreaMaxXAndY = new Vector2(maxX, maxY);
		}
	}
}
