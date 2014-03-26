using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	const int size = 10;
	
	Vector3[,] points = new Vector3[size, size];
	const float widhtScreen = 12.8f;
	
	public float doorWidht = 5f;
	public float speed = 2f;
	
	private playerContol playerCtrl;
	// Наша текущая позиция камеры (среди комнат)
	private Vector2 currentPos = new Vector2(5,0);
	// Наша предыдущая позиция камеры
	private Vector2 prePos;
	[HideInInspector]
	public Vector3 newPlayerPos;

	// Use this for initialization
	void Start () {
		prePos = currentPos;

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

		// Возвращаем камеры в предыдущую позицию, если мы отжали КУ или вышли из зоны двери
		if (Input.GetKeyUp(KeyCode.Q) || playerCtrl.isInTransZone == 0){
			currentPos = prePos;
		}

		if (playerCtrl.isInTransZone == 1){
			// Если стоим у двери, считываем нажатие на клавишу КУ
			float h = Input.GetAxis("LookAtDoor");
			// Запоминаем предыдущую позицию, если КУ не нажата
			if (h == 0) {
				prePos = currentPos;
			}
			// При нажатии на КУ проверяем направление и задаем камере новую позицию
			if (h > 0  && playerCtrl.currentState == playerContol.states.normal) {
				if (playerCtrl.canGoToDirection == 1) {
					currentPos = new Vector2(prePos.x + 1, prePos.y);
				} else if (playerCtrl.canGoToDirection == 2) { //входим в левую дверь
					currentPos = new Vector2(prePos.x - 1, prePos.y);
				}
			}
		}

		//Если камера находится в покое
		if (dist < 0.1f) {
			if(playerCtrl.currentState == playerContol.states.enteringRoom) {
				playerCtrl.currentState = playerContol.states.normal;
			}
			if (playerCtrl.isInTransZone == 1){
				if (Input.GetKeyDown(KeyCode.E)) {
					playerCtrl.currentState = playerContol.states.enteringRoom;
					if (playerCtrl.canGoToDirection == 1) { //входим в правую дверь
						currentPos = new Vector2(currentPos.x + 1, currentPos.y);
						newPlayerPos = new Vector3(playerCtrl.transform.position.x + doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
					} else if (playerCtrl.canGoToDirection == 2) { //входим в левую дверь
						currentPos = new Vector2(currentPos.x - 1, currentPos.y);
						newPlayerPos = new Vector3(playerCtrl.transform.position.x - doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
					}
				}
			}

		} else { //если камера перемещается
			//всегда передвигаем камеру в нужную позицию
			transform.position = Vector3.Lerp(transform.position, points[(int)currentPos.x, (int)currentPos.y], Time.deltaTime * speed);
		}
	}
}