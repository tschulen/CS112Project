using UnityEngine;
using System.Collections;

public enum JumpState
{
	GROUNDED,
	JUMPING,
	FALLING
}

public class CharControl : MonoBehaviour {


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

	void Start () {
		groundCheck = transform.Find ("groundCheck").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float moveH = Input.GetAxis ("Horizontal");
		Flip (moveH);
		
		if(moveH > 0)
		{
			if(rigidbody2D.velocity.x <= maxSpeed)
				rigidbody2D.AddForce(new Vector2 (moveH * addSpeed, 0));
		}
		else if (moveH == 0) ; //set animation to idle here.
		else if(moveH < 0)
		{
			if(rigidbody2D.velocity.x > -maxSpeed)
				rigidbody2D.AddForce(new Vector2 (moveH * addSpeed, 0));
		}
	}

	void Update() {
		switch (Jump) {
			
		case JumpState.GROUNDED: 
			if(Input.GetKey(KeyCode.Space) && isGrounded()) {
				Jump = JumpState.JUMPING;
			}
			break;
			
		case JumpState.JUMPING: 
			if(Input.GetKey(KeyCode.Space) && CurrJumpForce < MaxJumpForce) {
				var timeDiff = Time.deltaTime * 10;
				var forceToAdd = PlusJumpForce*timeDiff;
				CurrJumpForce += forceToAdd;
				rigidbody2D.AddForce(new Vector2(0, forceToAdd));
			}
			else {
				Jump = JumpState.FALLING;
				CurrJumpForce = 0;
			}
			break;
			
		case JumpState.FALLING: 
			if (isGrounded() && rigidbody2D.velocity.y <= 0) {
				Jump = JumpState.GROUNDED;
			}
			break;
			
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
