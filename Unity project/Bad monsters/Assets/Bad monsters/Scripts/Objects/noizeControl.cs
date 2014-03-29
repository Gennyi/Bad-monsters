using UnityEngine;
using System.Collections;

public class noizeControl : MonoBehaviour {

	public GameObject child;
	private bool dispatchOnce = true;
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			childControl ourChild = child.GetComponent<childControl>();
			//Только если спим или боимся
			if (Input.GetKeyDown(KeyCode.F) && ourChild.currentState >= childControl.states.sleeping && dispatchOnce) {
				dispatchOnce = false;
				ourChild.scaryLevel += 20f;
				ourChild.pointToMove = transform;
				ourChild.currentState = childControl.states.moving;
			}
		}
	}

}
