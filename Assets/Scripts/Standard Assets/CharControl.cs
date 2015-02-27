using UnityEngine;
using System.Collections;

public enum JumpState
{
	GROUNDED,
	JUMPING,
	FALLING
}

public class CharControl : MonoBehaviour {

	//Character controls
	KeyCode moveLeft;
	KeyCode moveRight;
	KeyCode jump;
	KeyCode dashLeft;
	KeyCode dashRight;

	//Movement variables
	public float maxSpeed = 100f;
	public float addSpeed = 1000f;
	private JumpState Jump = JumpState.GROUNDED;
	
	//Ground stuff
	[HideInInspector] public Transform groundCheck;
	float raycastLength = 0.15f;
	public LayerMask whatIsGround;

	//Jumpforce variables
	public float PlusJumpForce = 5f;
	public float CurrJumpForce = 0f;
	public float MaxJumpForce = 10f;

	public int horizDirection = 1;

	float moveH;
	public bool colliding = false;


	//Dash attack variables
	public float dashX = 10f;
	public float dashY = 10f;
	public bool dashLock = false;

	GameObject player;
	PlayerStats playerStats;

	void Awake () {
		var playerList = GameObject.FindGameObjectsWithTag ("Player");
		if(this.name.Equals ("Player1")){
			player = playerList[0];
		}else if (this.name.Equals ("Player2")){
			player = playerList[1];
		}
		playerStats = player.GetComponent <PlayerStats> ();

		Debug.Log (this.name);
	}

	void Start () {
		assignPlayerControls ();
		groundCheck = transform.Find ("groundCheck").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!playerStats.isDead) {
			float moveH = Input.GetAxis ("Horizontal");
			Flip (moveH);
		
			if (moveH > 0) {
				if (rigidbody2D.velocity.x <= maxSpeed)
					rigidbody2D.AddForce (new Vector2 (moveH * addSpeed, 0));
			} else if (moveH == 0); //set animation to idle here.
		    else if (moveH < 0) {
				if (rigidbody2D.velocity.x > -maxSpeed)
					rigidbody2D.AddForce (new Vector2 (moveH * addSpeed, 0));
			}
		}
	}

	void Update() {
	  if (!playerStats.isDead) {
			switch (Jump) {
			case JumpState.GROUNDED: 
				dashLock = false;
				if (Input.GetKey (jump) && isGrounded ()) {
					Jump = JumpState.JUMPING;
				}
			//DashAttack();
				break;
			
			case JumpState.JUMPING: 
				if (Input.GetKey (jump) && CurrJumpForce < MaxJumpForce) {
					var timeDiff = Time.deltaTime * 10;
					var forceToAdd = PlusJumpForce * timeDiff;
					CurrJumpForce += forceToAdd;
					rigidbody2D.AddForce (new Vector2 (0, forceToAdd));
				} else {
					Jump = JumpState.FALLING;
					CurrJumpForce = 0;
				}
			//DashAttack();
				break;
			
			case JumpState.FALLING: 
				if (isGrounded () && rigidbody2D.velocity.y <= 0) {
					Jump = JumpState.GROUNDED;
				}
			//DashAttack ();
				break;
			
			}

			DashAttack (); //Player can dash in any JumpState
		}
	}

	void DashAttack(){
		float xTotal = 0f;
		float yTotal = 0f;
		if (!dashLock) { //player can only dash once per jump
			if (Input.GetKey (dashRight)) {
				Flip (1);
				//rigidbody2D.AddForce (new Vector2 (100, 0));
				xTotal = dashX;
				playerStats.currentHealth-=10;
				dashLock = true;
			} else if (Input.GetKey (dashLeft)) {
				Flip (-1);
				//rigidbody2D.AddForce (new Vector2 (-100, 0));
				xTotal = -dashX;
				playerStats.currentHealth-=10;
				dashLock = true;
			}
			//dashLock = true;
		}
		//Jump = JumpState.FALLING;
		//CurrJumpForce = 0;
		rigidbody2D.AddForce (new Vector2 (xTotal, yTotal));

	}

	void assignPlayerControls(){
		if(player.name.Equals("Player1")){
			moveLeft = KeyCode.A;
			moveRight = KeyCode.D;
			jump = KeyCode.W;
			dashLeft = KeyCode.Q;
			dashRight = KeyCode.E;
			Debug.Log ("Player 1 Controls Set");
		}
		else if (player.name.Equals("Player2")){
			moveLeft = KeyCode.J;
			moveRight = KeyCode.L;
			jump = KeyCode.I;
			dashLeft = KeyCode.U;
			dashRight = KeyCode.O;
			Debug.Log ("Player 2 Controls Set");
		}
	}

	void Flip(float moveH)
	{
		if (moveH > 0)
			transform.localEulerAngles = new Vector3 (0, 0, 0);
		else if (moveH < 0)
			transform.localEulerAngles = new Vector3 (0, 180, 0);
	}

	public bool isGrounded()
	{
		return Physics2D.Raycast (groundCheck.position, -Vector2.up, raycastLength, whatIsGround);
	}


}
