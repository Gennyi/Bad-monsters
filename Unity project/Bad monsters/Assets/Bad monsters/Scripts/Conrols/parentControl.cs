﻿using UnityEngine;
using System.Collections;

public class parentControl : MonoBehaviour {

	public enum states {moving, interacting, found};
	
	[HideInInspector]
	public bool facingRight = false;			// Куда наш персонаж смотрит
	public Transform[] pointsOfInterests;
	public Transform[] newPointsOfInterests;
	public Transform[] pathMemory;
	public float speed = 5f;
	public float timeOfInteracting = 9f;
	public states currentState = states.moving;
	
	private playerControl playerCtrl;		// Reference to the PlayerControl script.
	private Transform pointToMove;
	private Timer time;
	private float timeOfInteractingReal = 0f;
	private Animator anim;					// Анимационный компонент
	public int pointMemory = 0;

	
	void Awake()
	{
		time = gameObject.AddComponent("Timer") as Timer;
		playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
		pathMemory = new Transform[3];
		findWay(getPoint());
		anim = GetComponent<Animator>();
	}
	
	Transform getPoint()
	{
		Transform tmp = pointToMove;
		Transform result = tmp;
		while (result == tmp) {
			result = pointsOfInterests[Random.Range(0, pointsOfInterests.Length)];
		}
		return result;
	}

	public static Transform findPoint(Vector3 selfPos, string layer, string tag){
		Transform point;
		RaycastHit2D hitLeft = Physics2D.Raycast(selfPos, -Vector2.right, 100f, 3 << LayerMask.NameToLayer(layer));
		RaycastHit2D hitRight = Physics2D.Raycast(selfPos, Vector2.right, 100f, 3 << LayerMask.NameToLayer(layer));
		float minDistL = 999999f;
		float minDistR = 999999f;
		if (hitLeft != null && hitLeft.collider != null) {
			if(hitLeft.collider.gameObject.tag == tag) {
				minDistL = Vector3.Distance(hitLeft.collider.gameObject.transform.position, selfPos);
			}
		}
		if (hitRight != null && hitRight.collider != null) {
			if(hitRight.collider.gameObject.tag == tag) {
				minDistR = Vector3.Distance(hitRight.collider.gameObject.transform.position, selfPos);
			}
		}
		if (minDistL < minDistR) {
			point = hitLeft.collider.gameObject.transform;
		} else {
			point = hitRight.collider.gameObject.transform;
		}
		return point;

	} 

	// Update is called once per frame
	void Update () {
		if(currentState == states.interacting){

			anim.SetFloat("Speed", 0f);

			time.startTimer();
			//Если простояли достаточное время на точке
			if (time.getTime() > timeOfInteractingReal){

				findWay(getPoint());
				currentState = states.moving;
			}
		} else if ( currentState == states.moving) {

			anim.SetBool("Sleep", false);
			anim.SetFloat("Speed", 1f);

			pointToMove = pathMemory[pointMemory];

			//Определяем, нашли ли монстрика
			Vector2 directionLook;
			if(facingRight){
				directionLook = Vector2.right;
			} else {
				directionLook = -Vector2.right;
			}

			// 513 = layers Default and Walls;
			RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), directionLook, 15f, 513);
			if (hit != null && hit.collider != null) {
				if(hit.collider.gameObject.tag == "Player") {
					currentState = states.found;
				}
			}
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
				if (pointMemory == 2) {
					currentState = states.interacting;
					//Если подошли к кровати, то устанавливаем анимацию сна
					Transform bad = GameObject.Find("badDad").GetComponent<Transform>();
					if (transform.position.x == bad.position.x) {
						anim.SetBool("Sleep", true);
					}
					//Если дошли до паука, то проверяем его наличие
					PickObject spider = GameObject.Find("spider").GetComponent<PickObject>();
					if (spider.myObject == PickObject.usingObjects.empty && Mathf.Abs(transform.position.x - pointsOfInterests[1].position.x) < 0.1f) {
						pointsOfInterests = newPointsOfInterests;
					}
					timeOfInteractingReal = Random.Range(timeOfInteracting - 2, timeOfInteracting + 2);
					time.stopTimer();
				} else if (pointMemory == 0){
					transform.position = pathMemory[++pointMemory].position;
					pointMemory++;
				}
			}
		} else if ( currentState == states.found) {
			Time.timeScale=0;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && playerCtrl.currentState != playerControl.states.hiding) {
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

	public void findWay(Transform target) {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position.x > transform.position.x ? Vector2.right : -Vector2.right , Mathf.Abs(transform.position.x - target.transform.position.x), 1 << LayerMask.NameToLayer("Walls"));
		if ((hit.collider != null) || Mathf.Abs(transform.position.y - target.position.y) > 3) {
			pointMemory = 0;
			pathMemory[0] = findPoint(transform.position ,"Walls", "Backdoor");
			pathMemory[1] = findPoint(target.transform.position ,"Walls", "Backdoor");
			pathMemory[2] = target;
			
		} else {
			pointMemory = 2;
			pathMemory[2] = target;
		}
	}
}
