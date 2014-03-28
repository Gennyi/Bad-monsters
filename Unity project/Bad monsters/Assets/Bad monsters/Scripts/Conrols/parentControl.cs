using UnityEngine;
using System.Collections;

public class parentControl : MonoBehaviour {
	
	enum states {moving, interacting, found};
	
	[HideInInspector]
	public bool facingRight = false;			// Куда наш персонаж смотрит
	public Transform[] pointsOfInterests;
	public float speed = 5f;
	public float lengthRoom = 7f;
	public float timeOfInteracting = 9f;

	private float halfOfParentBody = 1f;
	private playerContol playerCtrl;		// Reference to the PlayerControl script.
	private Transform pointToMove;
	public float leftPoint;
	public float rightPoint;
	private states currentState = states.moving;
	private float time = 0f;
	private float timeOfInteractingReal = 0f;

	private float distToLeft;
	private float distToRight;
	
	void Awake()
	{
		playerCtrl = GameObject.Find("Player").GetComponent<playerContol>();
		leftPoint = transform.position.x - halfOfParentBody;
		rightPoint = leftPoint + lengthRoom + 2*halfOfParentBody;

		getPoint();
	}

	void getPoint()
	{
		Transform tmp = pointToMove;
		while (pointToMove == tmp) {
			pointToMove = pointsOfInterests[Random.Range(0, pointsOfInterests.Length)];
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(currentState == states.interacting){
			time += Time.deltaTime;
			//Если простояли достаточное время на точке
			if (time > timeOfInteracting){
				getPoint();
				currentState = states.moving;
			}
		} else if ( currentState == states.moving) {
			Vector3 newPos = new Vector3(pointToMove.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * speed);

			//Вычисляем расстояние
			float distance = transform.position.x - playerCtrl.transform.position.x;
			distToLeft = Mathf.Abs(transform.position.x - leftPoint);
			distToRight = Mathf.Abs(transform.position.x - rightPoint);
			float distanceY = transform.position.y - playerCtrl.transform.position.y;
			//Определяем, нашли ли монстрика
			if(facingRight && distance < 0 && distanceY < 3 && Mathf.Abs(distance) < distToRight && playerCtrl.currentState != playerContol.states.hiding){
				currentState = states.found;
			}
			if(!facingRight && distance > 0 && distanceY < 3 && Mathf.Abs(distance) < distToLeft && playerCtrl.currentState != playerContol.states.hiding){
				currentState = states.found;
			}

			//поворачиваем родителя
			float direction = newPos.x - transform.position.x;
			if(direction > 0 && !facingRight) {
				Flip();
			} else if(direction < 0 && facingRight) {
				Flip();
			}
			//Если дошли до точки интереса
			if(transform.position == newPos){
				currentState = states.interacting;
				timeOfInteractingReal = Random.Range(timeOfInteracting - 2, timeOfInteracting + 2);
				time = 0f;
			}
		} else if ( currentState == states.found) {
			Time.timeScale=0;

		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && playerCtrl.currentState != playerContol.states.hiding) {
			currentState = states.found;
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
