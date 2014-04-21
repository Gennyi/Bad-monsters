using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour {

	private playerControl playerCtrl;
	public Vector2 offset;

	// Use this for initialization
	void Start () {
		HideBut();
		playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2(playerCtrl.transform.position.x + offset.x, playerCtrl.transform.position.y + offset.y);
	}

	public void ShowBut(){
		transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
	}
	public void HideBut(){
		transform.localScale = new Vector3();
	}
}
