using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
	public PInfos pInfos;

	public GameObject[] sprites;

	void Start() {
		sprites[pInfos.characterIndex].SetActive(true);
		enabled = false;
	}
}
