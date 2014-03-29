using UnityEngine;
using System.Collections;

public class childControl : MonoBehaviour {

	public enum states {moving, interacting, sleeping, scaring};

	[HideInInspector]
	public bool facingRight = false;			// Куда наш персонаж смотрит
	public Transform[] pointsOfInterests;
	public bool dispatchOnce = true;
	public float scaryLevel = 0f;
	public float speed = 5f;
	public float timeOfInteracting = 9f;
	public float timeOfSleep = 15f;
	private playerControl playerCtrl;		// Reference to the PlayerControl script.
	public Transform pointToMove;
	public states currentState = states.sleeping;
	private float time = 0f;
	
	void Awake()
	{
		playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentState == states.scaring){
			scaryLevel = Mathf.MoveTowards(scaryLevel, 0f, Time.deltaTime * 2);
			//Если перестали бояться, то засыпаем
			if (scaryLevel == 0f){
				currentState = states.sleeping;
			}
		} else if(currentState == states.sleeping){
			time += Time.deltaTime;
			//Если захотели прогуляться
			if (time > timeOfSleep){
				scaryLevel += 10f;
				pointToMove = pointsOfInterests[1];
				currentState = states.moving;
			}
		} else if(currentState == states.interacting){
			time += Time.deltaTime;
			//Если простояли достаточное время на точке
			if (time > timeOfInteracting){
				pointToMove = pointsOfInterests[0];
				currentState = states.moving;
			}
		} else if ( currentState == states.moving) {
			Vector3 newPos = new Vector3(pointToMove.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * speed);
	
			//поворачиваем ребенка
			float direction = newPos.x - transform.position.x;
			if(direction > 0 && !facingRight) {
				Flip();
			} else if(direction < 0 && facingRight) {
				Flip();
			}
			//Если дошли до точки интереса
			if(transform.position == newPos){
				currentState = newPos.x == pointsOfInterests[0].position.x ? states.scaring : states.interacting;
				time = 0f;
			}	
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && playerCtrl.currentState != playerControl.states.hiding) {
		}
	}

	void OnGUI () {
		string str = "Scary: " + scaryLevel;
		GUI.Label (new Rect (10,25,150,100), str);
	}

	void Flip ()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
