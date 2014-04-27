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
			childControl ourChild = child.GetComponent<childControl>();
			//Только если спим
			if (ourChild.currentState == childControl.states.sleeping && dispatchOnce) {
				EButton.ShowBut();
					if (Input.GetButtonDown("Action")) {
					dispatchOnce = false;
					ourChild.scaryLevel++;
					Animation anim = GetComponent<Animation>();
					anim.Play();
					ourChild.findWay(transform);
					ourChild.currentState = childControl.states.moving;
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

}
