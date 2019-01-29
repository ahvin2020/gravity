using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public enum Direction {
		UP = 1 ,
		DOWN = 2,
		LEFT = 3,
		RIGHT = 4
	};

	public GameObject MagnetSuccessMarkerPrefab;
	public GameObject MagnetFailMarkerPrefab;
	public float MoveGroundSpeed = 5.5f;	// what move speed when player on ground
	public float MoveAirSpeed = 4f;		// what move speed when player on air
	public float JumpForce = 310f;
	public float GravityForce = 8f;
	public float RaycastDistance = 300f;

	private Dictionary<int, MagnetConfig> MAGNET_CONFIGS;
	private Dictionary<KeyCode, InputConfig> INPUT_CONFIGS;

	private GameObject magnetSuccessMarker;
	private GameObject magnetFailMarker;
	private LayerMask groundLayerMask;
	private Animator animator;
	private Transform groundCheck;
	private Transform wallCheck;

	private int currentDirection;
	private bool isFacingRight;					// player is facing right?
	private bool isJumping;						// player is jumping now?
	private bool isTriggerJump;					// trigger jump in next FixedUpdate?
	private bool isChangedDirection;			// player used magnet and haven't touched ground?

	// Use this for initialization
	void Start () {
		initMagnetConfigs();
		initInputConfigs();
		initReferences();

		currentDirection = (int)Direction.DOWN;
		isFacingRight = true;
		isTriggerJump = false;
		isJumping = false;
		isChangedDirection = false;

		// go downwards in the beginnings
		changeDirection((int)PlayerController.Direction.DOWN, false);
	}

	/***** UPDATE FUNCTIONS *****/

	// Update is called once per frame
	void Update() {
		// player is jumping or changed direction?
		RaycastHit2D groundCheckRaycast = Physics2D.Linecast(transform.position, groundCheck.position, groundLayerMask);

		if (groundCheckRaycast) {
			if (isJumping || isChangedDirection) {
				// player no longer jumping
				if (isJumping) {
					animator.SetBool("Jumping", false);
				}

				// prevent player from sliding
				rigidbody2D.velocity = Vector2.zero;
				//rigidbody2D.angularVelocity  = 0;

				isJumping = false;
				isChangedDirection = false;
			}

			// player not standing on metal
			if (groundCheckRaycast.collider.tag != "Metal") {
				changeDirection((int)Direction.DOWN, false);
			}
		}

		// check can jump?
		if (!isJumping && Input.GetButtonDown("Jump")) {
			isTriggerJump = true;
		}

		// only allow direction to change once until reached ground
//		if (!isChangedDirection) {
			checkMagnetInput();
//		}
	}

	void FixedUpdate() {
		updateMovement();
		updateJump();
	}

	// move left right
	private void updateMovement() {
		float moveSpeed = MAGNET_CONFIGS[currentDirection].moveDirection;

		// in air?
		if (isJumping) {
			// move with air speed
			moveSpeed *= MoveAirSpeed;
		} else {
			// not in air, move with ground speed
			moveSpeed *= MoveGroundSpeed;
		}

		if (currentDirection == (int)Direction.DOWN || currentDirection == (int)Direction.UP) {
			moveSpeed *= Input.GetAxis("Horizontal");
		} else {
			moveSpeed *= Input.GetAxis("Vertical");
		}
		
		// wallCheck colliding with wall?
		if (!Physics2D.Linecast(transform.position, wallCheck.position, groundLayerMask)) {
			transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f);
		}
		
		// move right but not facing right, or move left but not facing left?
		if (moveSpeed > 0 && !isFacingRight || moveSpeed < 0 && isFacingRight) {
			isFacingRight = !isFacingRight;
			
			Vector3 playerScale = transform.localScale;
			playerScale.x *= -1;
			transform.localScale = playerScale;
		}
		
		// set moveSpeed parameter in animator
		animator.SetFloat("MoveSpeed", Mathf.Abs(moveSpeed));
	}
	
	// jump if needed
	private void updateJump() {
		if (isTriggerJump) {
			rigidbody2D.AddForce(MAGNET_CONFIGS[currentDirection].jumpDirection * JumpForce);
			animator.SetBool("Jumping", true);
			
			isJumping = true;
			isTriggerJump = false;
		}
	}

	public void changeDirection(int newDirection, bool isManual) {
		currentDirection = newDirection;
		
		transform.localRotation = Quaternion.Euler(0,0, MAGNET_CONFIGS[newDirection].rotation);
		Physics2D.gravity = MAGNET_CONFIGS[currentDirection].gravityDirection * GravityForce;

		// reset speed
		rigidbody2D.velocity = Vector2.zero;

		// is manual and not going down?
		if (isManual == true && newDirection != (int)Direction.DOWN) {
			isChangedDirection = true;
		}
	}

	// check magnet inputs
	private void checkMagnetInput() {
		int newDirection = -1;
		RaycastHit2D raycast;

		foreach (KeyValuePair<KeyCode, InputConfig> entry in INPUT_CONFIGS) {
			if (Input.GetKeyDown(entry.Key)) {
				if (currentDirection != entry.Value.direction) {
					raycast = Physics2D.Raycast(transform.position, entry.Value.vectorDirection, RaycastDistance, groundLayerMask);

					// go downwards OR hit a metal and can change direction?
					if (entry.Value.direction == (int)Direction.DOWN || raycast && raycast.collider.tag == "Metal" && !isChangedDirection) {
						newDirection = entry.Value.direction;

						// if raycast hits something, show Magnet Success Marker at that location
						if (raycast) {
							showMagnetMarker(raycast.point, Quaternion.Euler(0,0, MAGNET_CONFIGS[entry.Value.direction].rotation), true);
						}
					} 
					// unable to change direction but raycast hit something
					else if (raycast && !isChangedDirection) {
						showMagnetMarker(raycast.point, Quaternion.Euler(0,0, MAGNET_CONFIGS[entry.Value.direction].rotation), false);
					}
				}
				break;
			}
		}

		// is there a change in gravity?
		if (newDirection != -1) {
			changeDirection(newDirection, true);
		}
	}

	/***** COROUTINE FUNCTIONS *****/

	private IEnumerator hideMagnetSuccessMarker() {
		yield return new WaitForSeconds(0.5f);
		magnetSuccessMarker.renderer.enabled = false;
	}

	private IEnumerator hideMagnetFailMarker() {
		yield return new WaitForSeconds(0.5f);
		magnetFailMarker.renderer.enabled = false;
	}

	private void showMagnetMarker(Vector3 position, Quaternion rotation, bool isSuccess) {
		GameObject marker;

		if (isSuccess) {
			marker = magnetSuccessMarker;

//			if (marker.renderer.enabled) {
				StopCoroutine("hideMagnetSuccessMarker");
//			}

			StartCoroutine("hideMagnetSuccessMarker");
		} else {
			marker = magnetFailMarker;

//			if (marker.renderer.enabled) {
				StopCoroutine("hideMagnetFailMarker");
//			}

			StartCoroutine("hideMagnetFailMarker");
		}

		marker.renderer.enabled = true;
		marker.transform.position = position;
		marker.transform.localRotation = rotation;
	}


	/***** INIT FUNCTIONS *****/

	// init input configs
	private void initInputConfigs() {
		InputConfig downInputConfig = new InputConfig((int)Direction.DOWN, -Vector2.up);
		InputConfig upInputConfig = new InputConfig((int)Direction.UP, Vector2.up);
		InputConfig leftInputConfig = new InputConfig((int)Direction.LEFT, -Vector2.right);
		InputConfig rightInputConfig = new InputConfig((int)Direction.RIGHT, Vector2.right);
		
		INPUT_CONFIGS = new Dictionary<KeyCode, InputConfig>();
		INPUT_CONFIGS.Add(KeyCode.K, downInputConfig);
		INPUT_CONFIGS.Add(KeyCode.I, upInputConfig);
		INPUT_CONFIGS.Add(KeyCode.J, leftInputConfig);
		INPUT_CONFIGS.Add(KeyCode.L, rightInputConfig);
	}
	
	// init magnet configs
	private void initMagnetConfigs() {
		MagnetConfig downGravityConfig = new MagnetConfig(new Vector2(0f, -1), 0f, new Vector2(0f, 1), 1);
		MagnetConfig upGravityConfig = new MagnetConfig(new Vector2(0f, 1), 180f, new Vector2(0f, -1), -1);
		MagnetConfig leftGravityConfig = new MagnetConfig(new Vector2(-1, 0f), 270f, new Vector2(1, 0f), -1);
		MagnetConfig rightGravityConfig = new MagnetConfig(new Vector2(1, 0f), 90f, new Vector2(-1, 0f), 1);
		
		MAGNET_CONFIGS = new Dictionary<int, MagnetConfig>();
		MAGNET_CONFIGS.Add((int)Direction.DOWN, downGravityConfig);
		MAGNET_CONFIGS.Add((int)Direction.UP, upGravityConfig);
		MAGNET_CONFIGS.Add((int)Direction.LEFT, leftGravityConfig);
		MAGNET_CONFIGS.Add((int)Direction.RIGHT, rightGravityConfig);
	}
	
	private void initReferences() {
		magnetSuccessMarker = Instantiate(MagnetSuccessMarkerPrefab) as GameObject;
		magnetSuccessMarker.renderer.enabled = false;
		
		magnetFailMarker = Instantiate(MagnetFailMarkerPrefab) as GameObject;
		magnetFailMarker.renderer.enabled = false;
		
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
		animator = gameObject.GetComponent<Animator>();
		groundCheck = transform.Find("GroundCheck");
		wallCheck = transform.Find("WallCheck");
	}
}
