using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
	[SerializeField]
	private GameObject _spawnEffect;

	private Vector3 lastPos;

    void OnEnable() {
		_spawnEffect.transform.position = transform.position;
		_spawnEffect.SetActive(true);
	}

	void Update() {
		lastPos = transform.position;
		_spawnEffect.transform.position = lastPos;
	}

	/*void OnDisable() {
		_spawnEffect.SetActive(true);
	}*/
}
