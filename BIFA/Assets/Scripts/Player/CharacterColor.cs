using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour {
	private Renderer _renderer;

	private MaterialPropertyBlock _propBlock;

	public PInfos _pInfos;

	public Texture2D[] textures;

	void Awake() {
		_renderer = GetComponent<Renderer>();
		_propBlock = new MaterialPropertyBlock();
		CharColor(_pInfos);
	}

	void CharColor(PInfos infos) {
		_renderer.GetPropertyBlock(_propBlock);
		_propBlock.SetTexture("_MainTex", textures[_pInfos.clothesIndex]);
		_renderer.SetPropertyBlock(_propBlock);
	}
}