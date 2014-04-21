using UnityEngine;
using System.Collections;

public class childControl : MonoBehaviour {

	public enum states {moving, movingToBad, interacting, sleeping, scaring};

	[HideInInspector]
	private bool facingRight = false;			// Куда наш персонаж смотрит
	public Transform[] pointsOfInterests;
	public bool dispatchOnce = true;
	public int scaryLevel = 0;
	public float speed = 5f;
	public float timeOfInteracting = 9f;
	public float timeOfSleep = 15f;
	private playerControl playerCtrl;
	public Transform pointToMove;
	public states currentState = states.sleeping;
	private float time = 0f;
	private Transform[] pathMemory;		//Путь до точки
	private int pointMemory = 0;		//Текущая цель
	private int WCorFRE = 1;
	
	void Awake()
	{
		pathMemory = new Transform[3];
		playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
	}

	void Update () {
		if(currentState == states.scaring){

		} else if(currentState == states.sleeping){

			time += Time.deltaTime;
			//Если захотели прогуляться
			if (time > timeOfSleep){
				pointToMove = pointsOfInterests[WCorFRE];
				// Если сходили в туалет, то идем к холодильнику и наоборот 
				WCorFRE = WCorFRE%2 + 1;
				findWay(pointsOfInterests[WCorFRE]);
				currentState = states.moving;
			}
		} else if(currentState == states.interacting){
			time += Time.deltaTime;
			//Если простояли достаточное время на точке
			if (time > timeOfInteracting){
				findWay(pointsOfInterests[0]);
				currentState = states.moving;
			}
		} else if ( currentState <= states.movingToBad) {
			pointToMove = pathMemory[pointMemory];

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
				//если это объект
				if (pointMemory == 2){
					if (currentState == states.moving) {
						currentState = newPos.x == pointsOfInterests[0].position.x ? states.sleeping : states.interacting;
					} else {
						//если мы напуганные добежали до кровати
						currentState = states.sleeping;
						parentControl parent = GameObject.Find("Parent").GetComponent<parentControl>();
						parent.currentState = parentControl.states.moving;
						parent.findWay(transform);			//Родитель ищет путь до ребенка
					}
					//если это промежуточная дверь
				} else if (pointMemory == 0){
					transform.position = pathMemory[++pointMemory].position;
					pointMemory++;
				}
				time = 0f;
			}	
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && playerCtrl.currentState != playerControl.states.hiding && currentState != states.movingToBad) {
			findWay(pointsOfInterests[0]);
			currentState = states.movingToBad;
		}
	}

//	void OnGUI () {
//		string str = "Scary: " + scaryLevel;
//		GUI.Label (new Rect (10,25,150,100), str);
//	}

	void Flip ()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//Находим путь до объекта
	public void findWay(Transform target) {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position.x > transform.position.x ? Vector2.right : -Vector2.right , Mathf.Abs(transform.position.x - target.transform.position.x), 1 << LayerMask.NameToLayer("Walls"));
		if ((hit.collider != null) || Mathf.Abs(transform.position.y - target.position.y) > 3) {
			pointMemory = 0;
			pathMemory[0] = parentControl.findPoint(transform.position ,"Walls", "Backdoor");
			pathMemory[1] = parentControl.findPoint(target.transform.position ,"Walls", "Backdoor");
			pathMemory[2] = target;

		} else {
			pointMemory = 2;
			pathMemory[2] = target;
		}
	}
}
