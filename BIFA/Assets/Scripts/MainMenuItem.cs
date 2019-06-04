using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

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

	public PInfos pInfos;

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Player"))
			if (anim.Length != 0)
				for (int i = 0; i < anim.Length; i++) {
				anim[i].SetBool("OnTrigger", true);
				}
	}

	void OnTriggerStay(Collider col) {
		if(ReInput.players.GetPlayer(pInfos.pIndex).GetButtonDown("Submit"))
			switch (item) {
				case Item.None:
					break;
				case Item.Leave:
					GameManager.gmInst.Quit();
					break;
				case Item.Shop:
					break;
				case Item.Options:
					break;
				case Item.Clothes:
					break;
				case Item.OneVOne:
					GameManager.gmInst.Load1V1();
					break;
				case Item.TwoVTwo:
					GameManager.gmInst.Load2V2();
					break;
			}
	}

	void OnTriggerExit(Collider col) {
		if (col.CompareTag("Player"))
			if (anim.Length != 0)
				for (int i = 0; i < anim.Length; i++) {
					anim[i].SetBool("OnTrigger", false);
				}
	}
}
