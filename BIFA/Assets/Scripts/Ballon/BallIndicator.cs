using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIndicator : MonoBehaviour
{
	public GameObject ball;

	void OnEnable() {
		ScoreManager.onGameOver += DisableIndicator;
	}

	void OnDisable() {
		ScoreManager.onGameOver -= DisableIndicator;
	}

	private void Update() {
		float x, z;
		x = ball.transform.position.x;
		z = ball.transform.position.z;
		transform.position = new Vector3(x, 0.001f, z);
	}

	void DisableIndicator() {
		gameObject.SetActive(false);
	}
}
