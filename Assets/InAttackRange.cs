using UnityEngine;
using System.Collections;

public class InAttackRange : MonoBehaviour {
	private PlayerController player;
	public GameObject camera;
	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<PlayerController>();

	}
	
	// Update is called once per frame
	void Update () {
		if (player.colliding) {
			camera.transform.position = new Vector2 (camera.transform.position.x, camera.transform.position.y + 1);
		}
	}

	void OnTriggerEnter2D (Collider2D objectCollidedWith) {
		player.colliding = true;
	}

	void OnTriggerExit2D (Collider2D objectCollidedWith) {
		player.colliding = false;
	}
}
