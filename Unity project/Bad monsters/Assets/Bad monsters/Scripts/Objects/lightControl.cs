using UnityEngine;
using System.Collections;

public class lightControl : MonoBehaviour {

	public GameObject lamp;
	private GUIButton EButton;
	public bool isRequireObject = false;
	public PickObject.usingObjects objectToUse;

	void Start() {
		EButton = GameObject.Find("EBut").GetComponent<GUIButton>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
				//Переключить свет
			if(!isRequireObject){
				EButton.ShowBut();
				if (Input.GetButtonDown("Action")) {
					toggleLight();
				}
			}
			if (isRequireObject && playerControl.inventory.Contains(objectToUse)) {
				EButton.ShowBut();
				if (Input.GetButtonDown("Action")) {
					playerControl.inventory.Remove(objectToUse);
					toggleLight();
				}
			} else {
				EButton.HideBut();
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
		if (lamp.transform.localScale == hide) {
			hide = new Vector3(1,1,1);
		}
		lamp.transform.localScale = hide;
	}
}
