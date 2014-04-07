using UnityEngine;
using System.Collections;

public class lightControl : MonoBehaviour {

	public GameObject light;
	private GUIButton EButton;

	void Start() {
		EButton = GameObject.Find("EBut").GetComponent<GUIButton>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			EButton.ShowBut();
			if (Input.GetButtonDown("Action")) {
				//Переключить свет
				toggleLight();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			EButton.HideBut();
		}
	}

	void toggleLight() {
		Vector3 hide = new Vector3(0,0,0);
		if (light.transform.localScale == hide) {
			hide = new Vector3(1,1,1);
		}
		light.transform.localScale = hide;
	}
}
