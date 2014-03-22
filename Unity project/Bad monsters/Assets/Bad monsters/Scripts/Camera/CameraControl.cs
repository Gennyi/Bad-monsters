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
		points[1,0].x = 14f;
		points[1,0].y = 4.3f;
		points[2,0].x = 24f;
		points[2,0].y = 4.3f;

	}
	
	// Update is called once per frame
	void Update () {

		float dist = Vector3.Distance(points[(int)currentPos.x, (int)currentPos.y], transform.position);
		if (dist < 0.1f) {
			if (playerCtrl.isInTransZone == 1){
				if (Input.GetKeyDown(KeyCode.E)) {
					//двигаемся вправо
					if (playerCtrl.transform.position.x > transform.position.x) {
						currentPos = new Vector2(currentPos.x + 1, currentPos.y);
						newPlayerPos = new Vector3(playerCtrl.transform.position.x + doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
					} else {
						currentPos = new Vector2(currentPos.x - 1, currentPos.y);
						newPlayerPos = new Vector3(playerCtrl.transform.position.x - doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
					}
				}
			}
			if(playerCtrl.isActive == false) {
				playerCtrl.isActive = true;
			}
		} else {
		//всегда передвигаем камеру в нужную позицию
			playerCtrl.isActive = false;
			playerCtrl.transform.position = Vector3.Lerp(playerCtrl.transform.position, newPlayerPos, Time.deltaTime);
			transform.position = Vector3.Lerp(transform.position, points[(int)currentPos.x, (int)currentPos.y], Time.deltaTime);
		}
	}
}
