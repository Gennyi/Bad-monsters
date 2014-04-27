using UnityEngine;
using System.Collections;

public class PickObject : MonoBehaviour {

	private bool dispatchOnce = true;
	private GUIButton EButton;
	public enum usingObjects {difusBlade, spider, yeast, empty};
	public usingObjects myObject;
	
	void Start() {
		EButton = GameObject.Find("EBut").GetComponent<GUIButton>();
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && dispatchOnce) {
			EButton.ShowBut();
			if (Input.GetButtonDown("Action")) {
				dispatchOnce = false;
				transform.localScale = new Vector3();
				playerControl.inventory.Add(myObject);
				myObject = usingObjects.empty;
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
