using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	const int size = 10;
	
	Vector3[,] points = new Vector3[size, size];
	const float widhtScreen = 12.8f;
	
	public float doorWidht = 5f;
	
	private playerContol playerCtrl;
	private Vector2 currentPos = new Vector2(0,0);
	private Vector3 newPlayerPos;
	
	// Use this for initialization
	void Start () {
		playerCtrl = GameObject.Find("Player").GetComponent<playerContol>();
		for (int i = 0; i < size; i++){
			for(int j = 0; j < size; j++) {
				points[i, j].z = -10f;
			}
		}
		points[0,0].x = 4.8f;
		points[0,0].y = 4.3f;
		points[1,0].x = 17f;
		points[1,0].y = 4.3f;
		points[2,0].x = 24f;
		points[2,0].y = 4.3f;
		
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
				}
			}
			if(playerCtrl.currentState == playerContol.states.enteringRoom) {
				playerCtrl.currentState = playerContol.states.normal;
			}
		} else { //если камера перемещается
			//всегда передвигаем камеру в нужную позицию
			playerCtrl.currentState = playerContol.states.enteringRoom;
			playerCtrl.transform.position = Vector3.Lerp(playerCtrl.transform.position, newPlayerPos, Time.deltaTime);
			transform.position = Vector3.Lerp(transform.position, points[(int)currentPos.x, (int)currentPos.y], Time.deltaTime);
		}
	}
}