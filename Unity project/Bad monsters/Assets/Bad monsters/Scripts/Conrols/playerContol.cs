using UnityEngine;
using System.Collections;

public class playerContol : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;

	[HideInInspector]
	//Можем ли мы управлять персонажем
	public bool isActive = true;

	public float moveForce = 365f;
	public float maxSpeed = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Направление движения
		float h = Input.GetAxis("Horizontal");

		if(h * rigidbody2D.velocity.x < maxSpeed) {
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		}

		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}

	}
}
