using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauxLCD : MonoBehaviour
{
	private bool isWaiting = false, showAd = true;

	private Renderer _renderer;

	private MaterialPropertyBlock _propBlock;

	public Texture2D goalTex, streakerTex;

	public Texture2D[] textures;

	void Awake() {
		_renderer = GetComponent<Renderer>();
		_propBlock = new MaterialPropertyBlock();
	}

	void Update() {
		if (showAd) {
			if (!isWaiting)
				StartCoroutine(ShowPub());
		}
		else
			StopCoroutine(ShowPub());
	}

	public void SetGoal() {
		_renderer.GetPropertyBlock(_propBlock);
		_propBlock.SetTexture("_Diff", goalTex);
		_renderer.SetPropertyBlock(_propBlock);
	}

	public void SetStreaker() {
		_renderer.GetPropertyBlock(_propBlock);
		_propBlock.SetTexture("_Diff", streakerTex);
		_renderer.SetPropertyBlock(_propBlock);
	}

	IEnumerator ShowPub() {
		isWaiting = true;
		int i = Random.Range(0, textures.Length);
		_renderer.GetPropertyBlock(_propBlock);
		_propBlock.SetTexture("_Diff", textures[i]);
		_renderer.SetPropertyBlock(_propBlock);
		yield return new WaitForSeconds(10f);
		isWaiting = false;
	}
}
