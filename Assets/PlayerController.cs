using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	float moveH;
	public bool colliding = false;
	//public float moveSpeed = 
	//private GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		moveH = Input.GetAxis("Horizontal");

		if(moveH < 0) {
			rigidbody2D.AddForce (new Vector2 (-50, 0));
		} else if (moveH > 0) {
			rigidbody2D.AddForce (new Vector2 (50,0));
		} else {
		
		}
	}
}
