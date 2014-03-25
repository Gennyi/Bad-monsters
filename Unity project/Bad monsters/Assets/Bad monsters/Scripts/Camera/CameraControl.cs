using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	const int size = 10;
	
	Vector3[,] points = new Vector3[size, size];
	const float widhtScreen = 12.8f;
	
	public float doorWidht = 5f;
	public float speed = 2f;
	
	private playerContol playerCtrl;
	private Vector2 currentPos = new Vector2(5,0);
	private Vector3 newPlayerPos;
	
	// Use this for initialization
	void Start () {
		playerCtrl = GameObject.Find("Player").GetComponent<playerContol>();
		for (int i = 0; i < size; i++){
			for(int j = 0; j < size; j++) {
				points[i, j].z = -10f;
			}
		}
		points[3,0].x = -9.2f;
		points[3,0].y = 4.3f;
		points[4,0].x = -3.2f;
		points[4,0].y = 4.3f;
		points[5,0].x = 4.8f;
		points[5,0].y = 4.3f;
		points[6,0].x = 17f;
		points[6,0].y = 4.3f;
		points[7,0].x = 24f;
		points[7,0].y = 4.3f;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		float dist = Vector3.Distance(points[(int)currentPos.x, (int)currentPos.y], transform.position);
		//Если камера находится в покое
		if (dist < 0.1f) {
			if (playerCtrl.isInTransZone == 1){
				if (Input.GetKeyDown(KeyCode.E)) {
					//входим в правую дверь
					if (playerCtrl.transform.position.x > transform.position.x) {
						currentPos = new Vector2(currentPos.x + 1, currentPos.y);
						newPlayerPos = new Vector3(playerCtrl.transform.position.x + doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
					} else { //входим в левую дверь
						currentPos = new Vector2(currentPos.x - 1, currentPos.y);
						newPlayerPos = new Vector3(playerCtrl.transform.position.x - doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
					}
				} else if (Input.GetKeyDown(KeyCode.Q)) {
					transform.position = Vector3.Lerp(transform.position, points[(int)currentPos.x + 1, (int)currentPos.y], Time.deltaTime * speed);
				}
			}
			if(playerCtrl.currentState == playerContol.states.enteringRoom) {
				playerCtrl.currentState = playerContol.states.normal;
			}
		} else if (dist > 0.1f && !Input.GetKeyDown(KeyCode.Q)) { //если камера перемещается
			//всегда передвигаем камеру в нужную позицию
			playerCtrl.currentState = playerContol.states.enteringRoom;
			playerCtrl.transform.position = Vector3.MoveTowards(playerCtrl.transform.position, newPlayerPos, Time.deltaTime * speed);
			transform.position = Vector3.Lerp(transform.position, points[(int)currentPos.x, (int)currentPos.y], Time.deltaTime * speed);
		}
	}
}