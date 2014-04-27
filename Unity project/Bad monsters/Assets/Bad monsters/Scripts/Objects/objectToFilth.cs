using UnityEngine;
using System.Collections;

public class objectToFilth : MonoBehaviour {

	public bool isFilthInside = false ;
	private GUIButton EButton;
	private Timer time;
	public PickObject.usingObjects objectToUse;

	void Start() {
		time = gameObject.AddComponent("Timer") as Timer;
		EButton = GameObject.Find("EBut").GetComponent<GUIButton>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && playerControl.inventory.Contains(objectToUse)) {
			EButton.ShowBut();
			if (Input.GetButtonDown("Action")){
				playerControl.inventory.Remove(objectToUse);
				isFilthInside = true;
				EButton.HideBut();
			}
		}

		if(other.gameObject.tag == "Child" && isFilthInside){
			isFilthInside = false;
			time.startTimer();
		}

		if(time.getTime() > 2f) {
			childControl ourChild = GameObject.Find("Child").GetComponent<childControl>();
			ourChild.findWay(ourChild.pointsOfInterests[0]);
			ourChild.currentState = childControl.states.moving;
			ourChild.scaryLevel++;
			time.stopTimer();
		}

	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			EButton.HideBut();
		}
	}
}
