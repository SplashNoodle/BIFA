using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuItem : MonoBehaviour
{
	public Animator[] anim;

	public enum Item
	{
		None,
		Leave,
		Shop,
		Options,
		Clothes,
		OneVOne,
		TwoVTwo
	}

	public Item item = Item.None;

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Player"))
			if (anim.Length != 0)
				for (int i = 0; i < anim.Length; i++) {
				anim[i].SetBool("OnTrigger", true);
				}
	}

	void OnTriggerStay(Collider col) {

	}

	void OnTriggerExit(Collider col) {
		if (col.CompareTag("Player"))
			if (anim.Length != 0)
				for (int i = 0; i < anim.Length; i++) {
					anim[i].SetBool("OnTrigger", false);
				}
	}
}
