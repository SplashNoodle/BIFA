using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
	ParticleSystem pS;

	void Start() {
		pS = GetComponent<ParticleSystem>();
	}
	void Update() {
		if (pS.isStopped)
			gameObject.SetActive(false);
	}
}
