#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class ShowPause : MonoBehaviour
{
	#region Private Variables
	private float _blurVal = 0f;

	[SerializeField]
	private GameObject _blurEffect, _screen;

	[SerializeField]
	private Animator _camEffect;
	#endregion

	#region Methods
	void OnEnable() {
		GameManager.onGamePause += ShowScreen;
		GameManager.onGameResume += HideScreen;
	}

	void OnDisable() {
		GameManager.onGamePause -= ShowScreen;
		GameManager.onGameResume -= HideScreen;
	}
	#endregion

	#region Public Methods
	public void ShowScreen() {
		StartCoroutine(Pause());
	}

	public void HideScreen() {
		StartCoroutine(Resume());
	}
	#endregion

	#region Coroutines
	IEnumerator Pause() {
		_camEffect.SetBool("Desat", true);
		_blurEffect.SetActive(true);
		_blurVal = Mathf.Lerp(_blurVal, 2f, .25f);
		yield return new WaitForSeconds(.25f);
		_blurEffect.GetComponent<Image>().material.SetFloat("_Size", 2f);
		_screen.SetActive(true);
		Time.timeScale = 0;
	}

	IEnumerator Resume() {
		_camEffect.SetBool("Desat", false);
		_blurVal = Mathf.Lerp(_blurVal, 0f, .25f);
		yield return new WaitForSeconds(.25f);
		_blurEffect.GetComponent<Image>().material.SetFloat("_Size", 0f);
		_screen.SetActive(false);
		_blurEffect.SetActive(false);
	}
	#endregion
}
