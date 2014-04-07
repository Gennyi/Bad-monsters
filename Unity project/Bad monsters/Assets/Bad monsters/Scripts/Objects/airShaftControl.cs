using UnityEngine;
using System.Collections;

public class airShaftControl : MonoBehaviour {

	public GameObject exitPoint;
	public enum types {enter, exit};
	public types type;
	public Vector2 room;
	private GUIButton FButton;
	
	void Start() {
		FButton = GameObject.Find("FBut").GetComponent<GUIButton>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			FButton.ShowBut();
			if (Input.GetButtonDown("goOut")) {
				//Переходим в шахту
				CameraControl camera = GameObject.Find("MainCamera").GetComponent<CameraControl>();
				playerControl playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
				if(type == types.enter){
					camera.isFree = true;
				} else {
					camera.isFree = false;
					camera.currentPos = room;
					camera.prePos = camera.currentPos;
				}
//				camera.prePos = camera.currentPos;
				playerCtrl.transform.position = exitPoint.transform.position;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			FButton.HideBut();
		}
	}

}


