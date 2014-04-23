using UnityEngine;
using System.Collections;

public class CameraPositions : MonoBehaviour {

	private const int size = 10;
	const float infinity = 999999f;
	private static Vector3[,] points;

	// Use this for initialization
	public static Vector3[,] loadFirstLvl() {
		points = new Vector3[size, size];
		for (int i = 0; i < size; i++){
			for(int j = 0; j < size; j++) {
				points[i, j] = new Vector3(infinity, infinity, -10f);
			}
		}
		
		points[3,5].x = -9.2f;
		points[3,5].y = 4.3f;
		points[4,5].x = -3.2f;
		points[4,5].y = 4.3f;
		points[5,5].x = 4.8f;
		points[5,5].y = 4.3f;
		points[6,5].x = 17f;
		points[6,5].y = 4.3f;
		points[6,4].x = 15.2f;
		points[6,4].y = -3f;
		points[5,4].x = 5f;
		points[5,4].y = -3f;

		return points;
	}
}
