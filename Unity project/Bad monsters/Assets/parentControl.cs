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

	public float distToLeft;
	public float distToRight;
	
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
			//Определяем, нашли ли монстрика
			if(facingRight && distance < 0 && Mathf.Abs(distance) < distToRight && playerCtrl.currentState != playerContol.states.hiding){
				currentState = states.found;
			}
			if(!facingRight && distance > 0 && Mathf.Abs(distance) < distToLeft && playerCtrl.currentState != playerContol.states.hiding){
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



//		time += Time.deltaTime;
//		timeEvent += Time.deltaTime;
//		if (timeEvent > currentWaitForEvent && currentState < states.lookBack) {
//			if (Random.value > 0.5f) {
//				timeEvent = 0f;
//				Flip();
//				previousState.Push(currentState);
//				currentState = states.lookBack;
//			} else 
//				timeEvent = 0f;
//		}
//		
//		if (currentState == states.leftWaiting) {
//			if (time >= timeToWait + Random.Range(0, randomTime)) {
//				Flip();
//				currentState = states.goToRight;
//			}
//		} else if (currentState == states.goToRight) {
//			if(rigidbody2D.velocity.x < maxSpeed)
//				rigidbody2D.AddForce(Vector2.right * moveForce);
//			if (transform.position.x >= rightPoint) {
//				currentState = states.rightWaiting;
//				time = 0f;
//			}
//			
//		} else if (currentState == states.rightWaiting) {
//			
//			if (time >= timeToWait + Random.Range(0, randomTime)) {
//				Flip();
//				currentState = states.goToLeft;
//			}
//			
//		} else if (currentState == states.goToLeft) {
//			
//			if((-1) * rigidbody2D.velocity.x < maxSpeed)
//				rigidbody2D.AddForce(Vector2.right * moveForce * (-1));
//			if (transform.position.x <= leftPoint) {
//				currentState = states.leftWaiting;
//				time = 0f;
//			}
//			
//		} else if (currentState == states.lookBack) {
//			if (timeEvent >= 2) {
//				Flip();
//				currentState = (states)previousState.Pop();
//				timeEvent = 0f;
//			}
//		} else if (currentState == states.running) {
//			if(rigidbody2D.velocity.x < maxSpeed) {
//				if (transform.position.x - player.position.x > 0)
//					rigidbody2D.AddForce(Vector2.right * moveForce * (-1));
//				else 
//					rigidbody2D.AddForce(Vector2.right * moveForce);
//			}
//		} else if (currentState == states.postLooking) {
//			timeEvent -= 2*Time.deltaTime;
//			if (timeEvent < 0) {
//				currentState = (states)previousState.Pop();
//			}
//		} 
//		
//		//Проверяем, видим ли мы ГГ
//		float distance = transform.position.x - player.position.x;
//		float timeToFind = Mathf.Abs((distance - foundZone)*koefToCountTime) + (playerCtrl.onShadow?2:0);
//		
//		//		
//		
//		if (!playerCtrl.isHide &&
//		    ((distance < viewZone && distance > -1.5f && !facingRight) || (distance > -viewZone && distance < 1.5f && facingRight)) &&
//		    player.position.y + 0.6 > transform.position.y && player.position.y - 0.6 < transform.position.y) {
//			if (currentState < states.looking) {
//				previousState.Push(currentState);
//				previousState.Push(states.postLooking);
//				currentState = states.looking;
//				isOnFov = true;
//				timeEvent = 0f;
//			}
//			//нашли нас
//			if (isOnFov && timeEvent > timeToFind){
//				if ((distance > 0 && facingRight) || (distance < 0 && !facingRight))
//					Flip();
//				currentState = states.running;
//				//потеряли из виду
//			} else if (!isOnFov && timeEvent < timeToFind) {
//				currentState = (states)previousState.Pop();
//				timeEvent = 0f;
//			}
//		} else {
//			isOnFov = false;
//			if (currentState == states.looking){
//				currentState = (states)previousState.Pop();
//				//timeEvent = 0f;
//			}
//		}
//		
//		// Если скорость больше максимальной
//		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
//			// Устанавливаем текущую скорость на максимальную
//			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
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
