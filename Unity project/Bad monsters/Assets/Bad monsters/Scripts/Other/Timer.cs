using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float time = 0f;
	private bool isActive = false;

	// Update is called once per frame
	void Update () {
		if(isActive) {
			time += Time.deltaTime;
		}
	}

	public void startTimer() {
		isActive = true;
	}

	public void stopTimer() {
		isActive = false;
		time = 0f;
	}

	public void pauseTimer() {
		isActive = false;
	}

	public float getTime() {
		return time;
	}

}
