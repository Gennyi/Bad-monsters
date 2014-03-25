using UnityEngine;
using System.Collections;

public class lightControl : MonoBehaviour {

	public GameObject light;
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.F)) {
				//Переключить свет
				toggleLight();
			}
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
