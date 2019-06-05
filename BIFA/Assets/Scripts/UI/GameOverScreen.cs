#region System & Unity
using System.Collections;
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

public class GameOverScreen : MonoBehaviour
{
	#region Private Variables
	private int _hInd, _vInd, _buttonIndex;

	private float _deadZone = .1f;

	private bool _canSelect = true, _submitted = false;

	private Player _p;
	#endregion

	#region Public Variables
	public string[] explanations;

	public TextMeshProUGUI scoreDisplay, e1Display, e2Display, explanationsDisplay;

	public PInfos[] infos;

	public Sprite[] selectedSprites, idleSprites;

	public TransformArray[] buttonsArray;
	#endregion

	#region Methods
	void OnEnable() {
		scoreDisplay.text = ScoreManager.scoreInst.Score1.ToString() + " - " + ScoreManager.scoreInst.Score2.ToString();

		for (int i = 0; i < infos.Length; i++) {
			if (infos[i].equipe == 0)
				e1Display.text += infos[i].pseudo + '\n';
			if (infos[i].equipe == 1)
				e2Display.text += infos[i].pseudo + '\n';
		}

		_p = ReInput.players.GetPlayer(infos[0].pIndex);

		ButtonSelect(buttonsArray, 0, false);
	}

	void Update() {
		//On récupère les inputs du joueur
		float h = _p.GetAxis("Hor");
		float v = _p.GetAxis("Ver");

		if (h * h > v * v)
			v = 0;
		else
			h = 0;

		if (!_submitted) {
			if ((Mathf.Abs(h) >= _deadZone || Mathf.Abs(v) >= _deadZone) && _canSelect) {
				_canSelect = false;
				//Si le joueur dirige son stick horizontalement
				if (Mathf.Abs(h) >= _deadZone)
					ButtonSelect(buttonsArray, Mathf.Sign(h), false);
				//Si le joueur dirige son stick verticalement
				if (Mathf.Abs(v) >= _deadZone)
					ButtonSelect(buttonsArray, Mathf.Sign(v), true);
			}
			else if ((Mathf.Abs(h) < _deadZone && Mathf.Abs(v) < _deadZone))
				_canSelect = true;
		}

		if (_p.GetButtonDown("Submit")) {
			_submitted = true;
			switch (_buttonIndex) {
				case 0:
					//On charge la scène de sélection des personnages correspondante
					GameManager.gmInst.ReloadSelection();
					break;
				case 1:
					//On recharge la scène, on masque l'écran de settings, on loade les settings
					GameManager.gmInst.ReloadGame();
					break;
				case 2:
					//On charge la scène du menu principal
					GameManager.gmInst.LoadMainMenu();
					break;
				case 3:
					//On quitte le jeu
					GameManager.gmInst.Quit();
					break;
			}
		}
	}

	void ButtonSelect(TransformArray[] array, float nextPrev, bool ver) {
		array[_vInd].transforms[_hInd].GetComponent<Image>().sprite = idleSprites[_buttonIndex];

		if (ver) {
			_vInd = (int)Mathf.Repeat(_vInd + nextPrev, array.Length);
		}
		else {
			_hInd = (int)Mathf.Repeat(_hInd + nextPrev, array[_vInd].transforms.Length);
		}

		_buttonIndex = _vInd * array[0].transforms.Length + _hInd;

		array[_vInd].transforms[_hInd].GetComponent<Image>().sprite = selectedSprites[_buttonIndex];
		explanationsDisplay.text = explanations[_buttonIndex];
	}
	#endregion
}
