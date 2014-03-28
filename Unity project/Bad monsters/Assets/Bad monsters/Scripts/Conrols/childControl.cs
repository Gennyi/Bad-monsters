using UnityEngine;
using System.Collections;

public class childControl : MonoBehaviour {

	enum states {moving, interacting, sleeping};
	
	[HideInInspector]
	public bool facingRight = false;			// Куда наш персонаж смотрит
	public Transform[] pointsOfInterests;
	public float speed = 5f;
	public float timeOfInteracting = 9f;
	public float timeOfSleep = 15f;
	private playerControl playerCtrl;		// Reference to the PlayerControl script.
	private Transform pointToMove;
	private states currentState = states.sleeping;
	private float time = 0f;
	
	void Awake()
	{
		playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(currentState == states.sleeping){
			time += Time.deltaTime;
			//Если простояли достаточное время на точке
			if (time > timeOfSleep){
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
	
			//поворачиваем родителя
			float direction = newPos.x - transform.position.x;
			if(direction > 0 && !facingRight) {
				Flip();
			} else if(direction < 0 && facingRight) {
				Flip();
			}
			//Если дошли до точки интереса
			if(transform.position == newPos){
				currentState = newPos.x == pointsOfInterests[0].position.x ? states.sleeping : states.interacting;
				time = 0f;
			}	
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && playerCtrl.currentState != playerControl.states.hiding) {
		}
	}

	void Flip ()
	{
		// Разворачиваем нашего персонажа
		facingRight = !facingRight;
		
		// Для это просто зеркально скейлим его по оси x
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
