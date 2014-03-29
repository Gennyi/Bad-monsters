using UnityEngine;
using System.Collections;

public class toiletControl : MonoBehaviour {

	public GameObject toilet;
	public Vector2 room = new Vector2(0,0);
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.F)) {
				//Переключить свет
				CameraControl camera = GameObject.Find("MainCamera").GetComponent<CameraControl>();
				playerControl playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
				toiletControl anotherToilet = toilet.GetComponent<toiletControl>();
				camera.currentPos = anotherToilet.room;
				camera.prePos = camera.currentPos;
				playerCtrl.transform.position = anotherToilet.transform.position;
			}
		}
	}
}
