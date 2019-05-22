using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
	float timeElapsed;
	const float minTimeToShow = 5f;

	bool isLoading;

	[SerializeField]
	GameObject loadUI;

	AsyncOperation currentLoadingOp;

	public static LoadingUI loadInst;

	void Awake() {
		if (loadInst == null) {
			loadInst = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
			return;
		}

		Hide();
	}

	void Update() {
		if (isLoading) {
			float alpha = Mathf.Abs(Mathf.Sin(Time.time));
			loadUI.GetComponent<Image>().color = new Color(1, 1, 1, alpha);

			if (currentLoadingOp.isDone)
				Hide();
			else {
				timeElapsed += Time.deltaTime;
				if (timeElapsed >= minTimeToShow)
					currentLoadingOp.allowSceneActivation = true;
			}
		}
	}

	public void Hide() {
		loadUI.GetComponent<Image>().color = new Color(1, 1, 1, 0);
		loadUI.SetActive(false);
		currentLoadingOp = null;
		isLoading = false;
	}

	public void Show(AsyncOperation op) {
		loadUI.SetActive(true);
		currentLoadingOp = op;
		currentLoadingOp.allowSceneActivation = false;
		timeElapsed = 0f;
		isLoading = true;
	}
}
