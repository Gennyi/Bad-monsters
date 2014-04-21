using UnityEngine;
using System.Collections;

public class PickObject : MonoBehaviour {

	private bool dispatchOnce = true;
	private GUIButton EButton;
	public enum usingObjects {vase};
	public usingObjects myObject;
	
	void Start() {
		EButton = GameObject.Find("EBut").GetComponent<GUIButton>();
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			EButton.ShowBut();
			if (Input.GetButtonDown("Action") && dispatchOnce) {
				dispatchOnce = false;
				transform.localScale = new Vector3();
				//playerControl player = GameObject.Find("Player").GetComponent<playerControl>;

			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			EButton.HideBut();
		}
	}
}
