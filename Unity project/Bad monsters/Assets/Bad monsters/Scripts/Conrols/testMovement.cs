using UnityEngine;
using System.Collections;

public class testMovement : MonoBehaviour {
	
	public float speed = 2;
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");

		Vector2 posHero = new Vector2 (transform.position.x, transform.position.y);
		if(h != 0)
			transform.position = Vector2.Lerp(transform.position, posHero + Vector2.right * h * speed, Time.deltaTime);
	}

	void move()
	{

	}
}
