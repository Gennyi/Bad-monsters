﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public Vector3[,] points;
	const float widhtScreen = 12.8f;
	const float originSize = 3.58f;
	const float freeSize = 8f;

	
	public float doorWidht = 3.5f;
	public float speed = 2f;
	
	private playerControl playerCtrl;
	// Наша текущая позиция камеры (среди комнат)
	public Vector2 currentPos = new Vector2(5,5);
	// Наша предыдущая позиция камеры
	[HideInInspector]
	public Vector2 prePos;
	//Состояния камеры
	[HideInInspector]
	public bool isFree = false;

	// Use this for initialization
	void Start () {
		prePos = currentPos;
		playerCtrl = GameObject.Find("Player").GetComponent<playerControl>();
		points = CameraPositions.loadFirstLvl();
	}
	
	// Update is called once per frame
	void Update () {
		float dist;
		if (isFree) {
			dist = Vector3.Distance(playerCtrl.transform.position, transform.position);
			camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, freeSize, Time.deltaTime * 2);
		} else {
			dist = Vector3.Distance(points[(int)currentPos.x, (int)currentPos.y], transform.position);
			camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, originSize, Time.deltaTime * 2);
		}

		float h = Input.GetAxis("LookAtDoor");
		// Возвращаем камеры в предыдущую позицию, если мы отжали КУ или вышли из зоны двери
		if ((h == 0 || playerCtrl.isInTransZone == 0) && playerCtrl.currentState == playerControl.states.normal){
			currentPos = prePos;
		}
		// Если стоим у двери, считываем нажатие на клавишу КУ
		if (playerCtrl.isInTransZone == 1){
			// Запоминаем предыдущую позицию, если КУ не нажата
			if (h == 0) {
				prePos = currentPos;
			}
			// При нажатии на КУ проверяем направление и задаем камере новую позицию
			if (h > 0  && playerCtrl.currentState == playerControl.states.normal) {
				if (playerCtrl.canGoToDirection == 1) {
					currentPos = new Vector2(prePos.x + 1, prePos.y);
				} else if (playerCtrl.canGoToDirection == 2) { //входим в левую дверь
					currentPos = new Vector2(prePos.x - 1, prePos.y);
				}
			}
		}

		//Возвращаем персонажу нужное состояние
		if(playerCtrl.currentState == playerControl.states.enteringRoom && Vector3.Distance(playerCtrl.transform.position, playerCtrl.pointMove) == 0) {
			playerCtrl.currentState = playerControl.states.normal;
		}
		//Отслеживаем переход в новую комнату
		if (playerCtrl.isInTransZone == 1 && playerCtrl.currentState == playerControl.states.normal){
			if (Input.GetButtonDown("goOut")) {
				if (h == 1) {
					prePos = currentPos;
				}
				playerCtrl.currentState = playerControl.states.enteringRoom;
				if (playerCtrl.canGoToDirection == 1) { //входим в правую дверь
					currentPos = new Vector2( h == 1 ? currentPos.x : currentPos.x + 1, currentPos.y);
					playerCtrl.pointMove = new Vector3(playerCtrl.transform.position.x + doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
				} else if (playerCtrl.canGoToDirection == 2) { //входим в левую дверь
					currentPos = new Vector2(h == 1 ? currentPos.x : currentPos.x - 1, currentPos.y);
					playerCtrl.pointMove = new Vector3(playerCtrl.transform.position.x - doorWidht, playerCtrl.transform.position.y, playerCtrl.transform.position.z);
				}
			}
		}

		//если камере нужно перемещается
		if (dist > 0.1f) { 
			//всегда передвигаем камеру в нужную позицию
			if (isFree) {
				Vector3 playerPos = new Vector3(playerCtrl.transform.position.x, playerCtrl.transform.position.y, transform.position.z);
				transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * speed);
			} else {
				transform.position = Vector3.Lerp(transform.position, points[(int)currentPos.x, (int)currentPos.y], Time.deltaTime * speed);
			}
		}
	}
}