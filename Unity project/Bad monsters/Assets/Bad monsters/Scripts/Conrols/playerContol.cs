using UnityEngine;
using System.Collections;

public class playerContol : MonoBehaviour {
	
	[HideInInspector]
	public bool facingRight = true;
	//0 - не у двери, 1 - у двери, 2 - идем вниз, 3 - наверх
	public int isInTransZone = 0;
	public float speed = 5f;
	//0 - свободное перемещение, 1 - только влево, 2 - только вправо
	[HideInInspector]
	public int canGoToDirection = 0;
	public enum states {normal, enteringRoom, hiding};
	[HideInInspector]
	public states currentState = states.normal;
	[HideInInspector]
	public Vector3 pointMove;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Направление движения
		float h = Input.GetAxis("Horizontal");
		
		Vector2 posHero = new Vector2 (transform.position.x, transform.position.y);
		
		//Блокируем движение у двери
		if(canGoToDirection == 1 && h > 0){
			h = 0;
		} else if(canGoToDirection == 2 && h < 0){
			h = 0;
		}
		
		if(currentState == states.normal) {
			//функция движения оп горизонтали
			if(h != 0) {
				transform.position = Vector2.Lerp(transform.position, posHero + Vector2.right * h * speed, Time.deltaTime);
			}
			if(h > 0 && !facingRight) {
				Flip();
			} else if(h < 0 && facingRight) {
				Flip();
			}
		} else if(currentState == states.hiding) {
			//Если мы сейчас спрятаны
			if (Input.GetKeyDown(KeyCode.Q)) {
				currentState = states.normal;
				Vector3 newScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, transform.localScale.z);
				transform.localScale = newScale;
			}
		} else if (currentState == states.enteringRoom) {
			//Если мы сейчас перемещаемся между комнатами
			CameraControl camera = GameObject.Find("MainCamera").GetComponent<CameraControl>();
			transform.position = Vector3.MoveTowards(transform.position, pointMove, Time.deltaTime * speed);
		}
	}
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Door") {
			isInTransZone = 1;
			if (canGoToDirection == 0) {
				if(facingRight){
					canGoToDirection = 1;
				} else {
					canGoToDirection = 2;
				}
			}
		} else if (other.gameObject.tag == "Curtains") {
			if (Input.GetKeyDown(KeyCode.E) && currentState == states.normal) {
				currentState = states.hiding;
				Vector3 newScale = new Vector3(transform.localScale.x * 0.5f, transform.localScale.y * 0.5f, transform.localScale.z);
				transform.localScale = newScale;
			}
		} else if (other.gameObject.tag == "Light") {
			Time.timeScale = 0;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Door") {
			isInTransZone = 0;
			canGoToDirection = 0;
		}
	}
}