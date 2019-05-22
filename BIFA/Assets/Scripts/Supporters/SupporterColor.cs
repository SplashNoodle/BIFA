using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupporterColor : MonoBehaviour {
	private Renderer _renderer;
	private MaterialPropertyBlock _propBlock;

	public Supporter sInfos;

	//BIFA_TODO Quand textures dispo, remplacer les matériaux par les textures dans ce script.

	void Awake() {
		_renderer = GetComponent<Renderer>();
		_propBlock = new MaterialPropertyBlock();
		SupColor(sInfos);
	}

	void SupColor(Supporter info) {
		//On attribue le matériau en fonction de la position en x
		_renderer.GetPropertyBlock(_propBlock);
		if (transform.position.x < 0) {
			if (!CompareTag("Crete"))
				_propBlock.SetTexture("_MainTex", info.textures[0]);
			else
				_propBlock.SetColor("_Color", info.colors[0]);
		}
		else {
			if (!CompareTag("Crete"))
				_propBlock.SetTexture("_MainTex", info.textures[1]);
			else
				_propBlock.SetColor("_Color", info.colors[1]);
		}
		if (info.normalMap != null)
			_propBlock.SetTexture("_Bump", info.normalMap);
		_renderer.SetPropertyBlock(_propBlock);
	}
}
