using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TutoManager : MonoBehaviour
{

	public GameObject[] particles;
	public GameObject panel, ball;

	Stopwatch timer = new Stopwatch();

	private void Start() {
		panel.SetActive(false);
		ball.SetActive(false);
	}

	void Update()
    {
		if (allInactive()) {
			panel.SetActive(true);
			timer.Start();
		}

		if (timer.Elapsed.Seconds >= 10)
			ball.SetActive(true);
    }

	bool allInactive() {
		bool response = true;
		for (int i = 0; i < particles.Length; i++) {
			if (particles[i].activeInHierarchy) {
				response = false;
				break;
			}
		}
		return response;
	}
}
