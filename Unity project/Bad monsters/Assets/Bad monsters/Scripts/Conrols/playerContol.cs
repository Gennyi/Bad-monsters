using UnityEngine;
using System.Collections;

public class playerContol : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;

	[HideInInspector]
	//Можем ли мы управлять персонажем
	public bool isActive = true;
	
	public float speed = 5f;
	//0 - не у двери, 1 - у двери, 2 - идем вниз, 3 - наверх
	public int isInTransZone = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Направление движения
		float h = Input.GetAxis("Horizontal");
		
		Vector2 posHero = new Vector2 (transform.position.x, transform.position.y);
		//функция движения оп горизонтали
		if(h != 0)
			transform.position = Vector2.Lerp(transform.position, posHero + Vector2.right * h * speed, Time.deltaTime);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Door")
			isInTransZone = 1;
	}
}
