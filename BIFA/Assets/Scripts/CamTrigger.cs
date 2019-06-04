using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CamTrigger : MonoBehaviour
{
	public int desiredPriority;

	public CinemachineVirtualCamera cam;

    void OnTriggerStay(Collider col) {
		if (col.CompareTag("Player"))
		cam.Priority = desiredPriority;
	}

	void OnTriggerExit(Collider col) {
		if (col.CompareTag("Player"))
			cam.Priority = 0;
	}
}
