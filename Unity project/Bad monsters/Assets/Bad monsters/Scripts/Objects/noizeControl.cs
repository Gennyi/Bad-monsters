using UnityEngine;
using System.Collections;

public class noizeControl : MonoBehaviour {

	public GameObject child;
	private bool dispatchOnce = true;
	private GUIButton EButton;
	
	void Start() {
		EButton = GameObject.Find("EBut").GetComponent<GUIButton>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			EButton.ShowBut();
			childControl ourChild = child.GetComponent<childControl>();
			//Только если спим или боимся
			if (Input.GetButtonDown("Action") && ourChild.currentState >= childControl.states.sleeping && dispatchOnce) {
				dispatchOnce = false;
				ourChild.scaryLevel += 20;
				ourChild.pointToMove = transform;
				ourChild.currentState = childControl.states.moving;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			EButton.HideBut();
		}
	}

}
