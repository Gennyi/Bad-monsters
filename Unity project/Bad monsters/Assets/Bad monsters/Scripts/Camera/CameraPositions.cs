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

		points[5,5].x = -0.42f;
		points[5,5].y = 0f;
		points[6,5].x = 5.3f;
		points[6,5].y = 0f;
		points[7,5].x = 17.45f;
		points[7,5].y = 0f;
		points[5,4].x = 0.77f;
		points[5,4].y = -9.45f;
		points[6,4].x = 12.73f;
		points[6,4].y = -9.45f;

		return points;
	}
}
