#region System & Unity
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

#region TextMeshPro
using TMPro;
#endregion

#region Rewired
using Rewired;
#endregion

[System.Serializable]
public class TransformArray
{
	public Transform[] transforms;
}

public class CharAndTeeSelection : MonoBehaviour
{
	#region Private Variables
	private int _infoIndex, _tIndex, _oldTIndex, _hInd, _vInd;

	private float _deadZone = .1f;

	private bool _canSelect = true, _charSelDone = false, _teeSelDone = false;

	private Sprite _removedSprite;

	private AudioSource _src;

	private Player p;
	#endregion

	#region Public Variables
	public int SetMaxReadyCount;

	public GameObject ready, flag;

	public TransformArray[] chars;

	public Image charImage, tenueImage;

	public Sprite[] charSprites, clothesSprites;

	public AudioClip move, select, cancel;

	public TextMeshProUGUI pseudoDisplay;

	public PInfos pInfos, mateInfos;
	public enum SelectionMode
	{
		Character,
		Tenue
	}

	private SelectionMode selMode = SelectionMode.Character;
	#endregion

	#region Public Static Variables
	public static int readyCount, maxReadyCount;

	public static Dictionary<Sprite, bool> tenuesDispos = new Dictionary<Sprite, bool>();
	#endregion

	#region Methods
	void OnEnable() {
		_src = GetComponent<AudioSource>();
		if (pInfos.selectorIndex < 3)
			InfoSelect(chars, 0, false);
		else
			InfoSelect(chars, 0, true);
		p = ReInput.players.GetPlayer(pInfos.pIndex);

		for (int i = 0; i < clothesSprites.Length; i++) {
			if (!tenuesDispos.ContainsKey(clothesSprites[i]))
				tenuesDispos.Add(clothesSprites[i], true);
		}
		readyCount = 0;
		maxReadyCount = SetMaxReadyCount;

		_src.volume = SoundManager.sInst.settings.interfaceVolume;

		ReInput.ControllerDisconnectedEvent += ReInput_ControllerDisconnectedEvent;
	}

	void OnDisable() {
		ReInput.ControllerDisconnectedEvent -= ReInput_ControllerDisconnectedEvent;
	}

	void ReInput_ControllerDisconnectedEvent(ControllerStatusChangedEventArgs obj) {
		for (int i = 0; i < ReInput.players.playerCount; i++) {
			Player p = ReInput.players.GetPlayer(i);
			if (p.controllers.ContainsController(obj.controllerType, obj.controllerId)) {
				Debug.Log("Player " + i + " a perdu sa manette !");
			}
		}
	}

	void Update() {
		//On récupère les inputs du joueur
		float h = p.GetAxis("Hor");
		float v = p.GetAxis("Ver");

		if (h * h > v * v)
			v = 0;
		else
			h = 0;

		switch (selMode) {
			case SelectionMode.Character:
				CharSelection(h, v);
				break;
			case SelectionMode.Tenue:
				TeeSelection(v);
				break;
		}

		//On définit le pseudo dans le scriptable object
		pInfos.pseudo = pseudoDisplay.text;
	}

	void CharSelection(float h, float v) {
		if (!_charSelDone) {
			if ((Mathf.Abs(h) >= _deadZone || Mathf.Abs(v) >= _deadZone) && _canSelect) {
				_canSelect = false;
				//Si le joueur dirige son stick horizontalement
				if (Mathf.Abs(h) >= _deadZone)
					InfoSelect(chars, Mathf.Sign(h), false);
				//Si le joueur dirige son stick verticalement
				if (Mathf.Abs(v) >= _deadZone)
					InfoSelect(chars, Mathf.Sign(v), true);
			}
			else if ((Mathf.Abs(h) < _deadZone && Mathf.Abs(v) < _deadZone))
				_canSelect = true;

			charImage.sprite = charSprites[_infoIndex];
			pseudoDisplay.text = chars[_vInd].transforms[_hInd].GetComponent<CharInfos>().charName;

			//Si le joueur appuie sur le bouton Submit
			if (p.GetButtonDown("Submit")) {
				_src.clip = select;
				SoundManager.sInst.PlayClip(_src, _src.clip, SoundManager.sInst.settings.interfaceVolume);
				//On termine la sélection du personnage
				_charSelDone = true;
				//On désactive le bouton
				chars[_vInd].transforms[_hInd].GetComponent<Image>().color = new Color(1f, 1f, 1f, .5f);
				//On set l'index du personnage
				pInfos.characterIndex = _infoIndex;
				pInfos.uiSprite = chars[_vInd].transforms[_hInd].GetComponent<CharInfos>().uiSprite;

				//On vérifie si le joueur est le capitaine de l'équipe
				if (pInfos.selectorIndex < 2) {
					flag.SetActive(true);
					_tIndex = _infoIndex;
					selMode = SelectionMode.Tenue;
				}
				else {
					ready.SetActive(true);
					readyCount++;
				}
			}
		}
		else if (p.GetButtonDown("Cancel")) {
			_src.clip = cancel;
			SoundManager.sInst.PlayClip(_src, _src.clip, SoundManager.sInst.settings.interfaceVolume);
			//On reprend la sélection
			_charSelDone = false;
			//On réactive le bouton
			chars[_vInd].transforms[_hInd].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			if (pInfos.selectorIndex >= 2) {
				ready.SetActive(false);
				readyCount--;
			}
		}
	}

	void TeeSelection(float v) {

		if (!_teeSelDone) {
			if (Mathf.Abs(v) >= _deadZone && _canSelect) {
				_canSelect = false;
				_tIndex += (int)Mathf.Sign(v);
				_tIndex = (int)Mathf.Repeat(_tIndex, clothesSprites.Length);
			}
			else if (Mathf.Abs(v) < _deadZone)
				_canSelect = true;

			bool value;
			tenuesDispos.TryGetValue(clothesSprites[_tIndex], out value);
			if (!value) {
				_tIndex += (int)Mathf.Sign(v);
				_tIndex = (int)Mathf.Repeat(_tIndex, clothesSprites.Length);
			}

			tenueImage.sprite = clothesSprites[_tIndex];

			//Si le joueur appuie sur le bouton de retour
			if (p.GetButtonDown("Cancel")) {
				_src.clip = cancel;
				SoundManager.sInst.PlayClip(_src, _src.clip, SoundManager.sInst.settings.interfaceVolume);
				//On masque le drapeau
				flag.SetActive(false);
				//On repasse en mode sélection de personnage
				_charSelDone = false;
				chars[_vInd].transforms[_hInd].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				selMode = SelectionMode.Character;
			}

			//Si le joueur appuie sur le bouton Submit
			if (p.GetButtonDown("Submit")) {
				_src.clip = select;
				SoundManager.sInst.PlayClip(_src, _src.clip, SoundManager.sInst.settings.interfaceVolume);
				//On termine la sélection du personnage
				_teeSelDone = true;
				//On désactive le bouton
				tenueImage.color = new Color(1f, 1f, 1f, .5f);
				//On enlève la tenue des choix disponibles
				_oldTIndex = _tIndex;
				tenuesDispos[clothesSprites[_tIndex]] = false;
				//On set l'index de la tenue
				pInfos.clothesIndex = _tIndex;
				if (mateInfos != null)
					mateInfos.clothesIndex = _tIndex;
				//On affiche le message ready
				ready.SetActive(true);
				readyCount++;
			}
		}
		else if (p.GetButtonDown("Cancel")) {
			_src.clip = cancel;
			SoundManager.sInst.PlayClip(_src, _src.clip, SoundManager.sInst.settings.interfaceVolume);
			_teeSelDone = false;
			tenueImage.color = new Color(1f, 1f, 1f, 1f);
			tenuesDispos[clothesSprites[_oldTIndex]] = true;
			ready.SetActive(false);
			readyCount--;
		}
	}

	void InfoSelect(TransformArray[] array, float nextPrev, bool ver) {
		if (_src.clip != move || _src.clip == null)
			_src.clip = move;

		array[_vInd].transforms[_hInd].GetChild(pInfos.selectorIndex).gameObject.SetActive(false);

		if (ver) {
			_vInd = (int)Mathf.Repeat(_vInd + nextPrev, array.Length);
		}
		else {
			_hInd = (int)Mathf.Repeat(_hInd + nextPrev, array[_vInd].transforms.Length);
		}

		bool selectable = true;
		selectable = CheckSelection(array, selectable);

		if (selectable) {
			array[_vInd].transforms[_hInd].GetChild(pInfos.selectorIndex).gameObject.SetActive(true);
		}
		else {
			if (nextPrev == 0)
				nextPrev = 1;

			InfoSelect(array, nextPrev, ver);
		}

		SoundManager.sInst.PlayClip(_src, _src.clip, SoundManager.sInst.settings.interfaceVolume);

		_infoIndex = _vInd * array[0].transforms.Length + _hInd;
	}

	private bool CheckSelection(TransformArray[] array, bool selectable) {
		for (int i = 0; i < array[_vInd].transforms[_hInd].childCount; i++) {
			if (array[_vInd].transforms[_hInd].GetChild(i).gameObject.activeSelf)
				selectable = false;
		}

		return selectable;
	}
	#endregion

	#region Public Properties
	public SelectionMode SelMode { get => selMode; set => selMode = value; }
	#endregion
}
