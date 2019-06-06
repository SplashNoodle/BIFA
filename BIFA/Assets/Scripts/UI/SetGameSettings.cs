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

public class SetGameSettings : MonoBehaviour
{
	#region Private Variables
	private int _value = 5, _ballIndex = 0;

	private bool _canSelect = true, _allowChange = true;

	private GameSettings.GameType _type = GameSettings.GameType.Score;

	private GameSettings.BallType _ball = GameSettings.BallType.Foot;
	#endregion

	#region Public Variables
	public GameObject isEventActive, isObjectActive;

	public GameObject[] colored = new GameObject[6], gameBall = new GameObject[3], goldenBall = new GameObject[3], tempFootBalls = new GameObject[3], tempBasketBalls = new GameObject[3], tempBowlingBalls = new GameObject[3];

	public RectTransform[] values = new RectTransform[10];

	public string[] ballTypes = new string[3], ballTypeDesc = new string[3];

	public Image ballSprite;

	public Sprite[] ballSprites;

	public Color selectedColor, baseColor;

	public TextMeshProUGUI gameTypeDisplay, ballTypeDisplay, ballTypeDescDisplay;

	public GameSettings gs;

	public PInfos pInfos;

	public Player p;
	#endregion

	#region Methods
	void OnEnable() {
		p = ReInput.players.GetPlayer(pInfos.pIndex);

		gs.settingsMode = GameSettings.SettingsMode.Type;

		ballTypeDisplay.text = ballTypes[0];
		ballTypeDescDisplay.text = ballTypeDesc[0];

		GameManager.onGameReload += LoadSettings;
	}

	void OnDisable() {
		GameManager.onGameReload -= LoadSettings;
	}

	void Update() {
		if (p.GetAxis("Hor") == 0 && p.GetAxis("Ver") == 0)
			_allowChange = true;

		switch (gs.settingsMode) {
			case GameSettings.SettingsMode.Type:
				SetSelected(0);
				if (p.GetAxis("Hor") != 0 && _canSelect) {
					_canSelect = false;
					SetGameType();
				}
				if (p.GetAxis("Hor") == 0)
					_canSelect = true;

				if (p.GetButtonDown("Submit") || p.GetAxis("Ver") < 0) {
					gs.gameType = _type;
					gs.settingsMode = GameSettings.SettingsMode.Value;
					_allowChange = false;
				}
				break;
			case GameSettings.SettingsMode.Value:
				SetSelected(1);
				if (p.GetAxis("Hor") != 0 && _canSelect) {
					_canSelect = false;
					_value = SetValue(Mathf.Sign(p.GetAxis("Hor")));
				}
				if (p.GetAxis("Hor") == 0)
					_canSelect = true;

				if ((p.GetButtonDown("Submit") || p.GetAxis("Ver") < 0) && _allowChange) {
					gs.value = _value;
					gs.settingsMode = GameSettings.SettingsMode.Ball;
					_allowChange = false;
				}
				if ((p.GetButtonDown("Cancel") || p.GetAxis("Ver") > 0) && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Type;
					_allowChange = false;
				}
				break;
			case GameSettings.SettingsMode.Ball:
				SetSelected(2);
				if (p.GetAxis("Hor") != 0 && _canSelect) {
					_canSelect = false;
					SetBallType();
				}
				if (p.GetAxis("Hor") == 0)
					_canSelect = true;

				if ((p.GetButtonDown("Submit") || p.GetAxis("Ver") < 0) && _allowChange) {
					gs.ballType = _ball;
					gs.settingsMode = GameSettings.SettingsMode.Events;
					_allowChange = false;
				}
				if ((p.GetButtonDown("Cancel") || p.GetAxis("Ver") > 0) && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Value;
					_allowChange = false;
				}
				break;
			case GameSettings.SettingsMode.Events:
				SetSelected(3);
				if (p.GetButtonDown("Submit") && _canSelect) {
					_canSelect = false;
					gs.events = !gs.events;
					isEventActive.SetActive(gs.events);
				}
				if (p.GetButtonUp("Submit"))
					_canSelect = true;

				if (p.GetAxis("Hor") > 0 && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Objects;
					_allowChange = false;
				}
				if ((p.GetButtonDown("Cancel") || p.GetAxis("Ver") > 0) && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Ball;
					_allowChange = false;
				}
				break;
			case GameSettings.SettingsMode.Objects:
				SetSelected(4);
				if (p.GetButtonDown("Submit") && _canSelect) {
					_canSelect = false;
					gs.objects = !gs.objects;
					isObjectActive.SetActive(gs.objects);
				}
				if (p.GetButtonUp("Submit"))
					_canSelect = true;

				if (p.GetAxis("Ver") < 0 && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Validation;
					_allowChange = false;
				}
				if ((p.GetButtonDown("Cancel") || p.GetAxis("Hor") < 0) && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Events;
					_allowChange = false;
				}
				break;
			case GameSettings.SettingsMode.Validation:
				SetSelected(5);
				if (p.GetButtonDown("Submit")) {
					LoadSettings();
				}
				if ((p.GetButtonDown("Cancel") || p.GetAxis("Ver") > 0) && _allowChange) {
					gs.settingsMode = GameSettings.SettingsMode.Objects;
					_allowChange = false;
				}
				break;
		}
	}

	void SetSelected(int index) {
		for (int i = 0; i < colored.Length; i++) {
			if (i == index)
				colored[i].GetComponent<Image>().color = selectedColor;
			else
				colored[i].GetComponent<Image>().color = baseColor;
		}
	}

	void SetGameType() {
		if (_type == GameSettings.GameType.Score) {
			_type = GameSettings.GameType.Time;
			gameTypeDisplay.text = "Time";
		}
		else if (_type == GameSettings.GameType.Time) {
			_type = GameSettings.GameType.Score;
			gameTypeDisplay.text = "Score";
		}
	}

	int SetValue(float dir) {
		if (dir > 0 && _value < 10) {
			foreach (RectTransform item in values) {
				//Lorsqu'on augmente le score, les éléments se déplacent vers la gauche
				if (item.localPosition.x == 0)
					item.localPosition = new Vector3(item.localPosition.x - 195, item.localPosition.y, item.localPosition.z);
				else if (item.localPosition.x == 195)
					item.localPosition = new Vector3(item.localPosition.x - 195, item.localPosition.y, item.localPosition.z);
				else if (item.localPosition.x > -655)
					item.localPosition = new Vector3(item.localPosition.x - 115, item.localPosition.y, item.localPosition.z);
				else
					item.localPosition = new Vector3(655, item.localPosition.y, item.localPosition.z);

				//Lorsqu'on arrive en position 0, on change la taille du texte
				if (item.localPosition.x == 0)
					item.GetComponent<TextMeshProUGUI>().fontSize = 140;
				else
					item.GetComponent<TextMeshProUGUI>().fontSize = 80;
			}
			_value++;
		}
		else if (dir < 0 && _value > 1) {
			foreach (RectTransform item in values) {
				//Lorsqu'on diminue le score, les éléments se déplacent vers la droite		
				if (item.localPosition.x == 0)
					item.localPosition = new Vector3(item.localPosition.x + 195, item.localPosition.y, item.localPosition.z);
				else if (item.localPosition.x == -195)
					item.localPosition = new Vector3(item.localPosition.x + 195, item.localPosition.y, item.localPosition.z);
				else if (item.localPosition.x < 655)
					item.localPosition = new Vector3(item.localPosition.x + 115, item.localPosition.y, item.localPosition.z);
				else
					item.localPosition = new Vector3(-655, item.localPosition.y, item.localPosition.z);

				//Lorsqu'on arrive en position 0, on change la taille du texte
				if (item.localPosition.x == 0)
					item.GetComponent<TextMeshProUGUI>().fontSize = 140;
				else
					item.GetComponent<TextMeshProUGUI>().fontSize = 80;
			}
			_value--;
		}

		return _value;
	}

	void SetBallType() {

		if (p.GetAxis("Hor") < 0)
			_ballIndex++;
		if (p.GetAxis("Hor") > 0)
			_ballIndex--;

		_ballIndex = (int)Mathf.Repeat(_ballIndex, 3f);

		Debug.Log(_ballIndex);

		ballSprite.sprite = ballSprites[_ballIndex];
		ballTypeDisplay.text = ballTypes[_ballIndex];
		ballTypeDescDisplay.text = ballTypeDesc[_ballIndex];

		if (_ballIndex == 0)
			_ball = GameSettings.BallType.Foot;
		if (_ballIndex == 1)
			_ball = GameSettings.BallType.Basket;
		if (_ballIndex == 2)
			_ball = GameSettings.BallType.Bowling;
	}
	#endregion

	#region Public Methods
	public void LoadSettings() {
		Debug.Log("Game Type : " + gs.gameType);
		Debug.Log("Value : " + gs.value);
		Debug.Log("Ball Type : " + gs.ballType);
		Debug.Log("Events Allowed : " + gs.events);
		Debug.Log("Objects Allowed : " + gs.objects);
		//On définit le type de partie dans le ScoreManager
		ScoreManager.scoreInst.gt = gs.gameType;
		//On définit le score max / le temps de la partie dans le ScoreManager
		ScoreManager.scoreInst.maxValue = gs.value;
		//On active la bonne balle et on définit ses leurres dans l'EventMaster
		switch (_ball) {
			case GameSettings.BallType.Foot:
				gameBall[0].SetActive(true);
				gameBall[1].SetActive(false);
				gameBall[2].SetActive(false);
				EventMaster.eMInst.balls = tempFootBalls;
				ReputationManager.repInst.goldenBall = goldenBall[0];
				FindObjectOfType<BallIndicator>().ball = gameBall[0];
				break;
			case GameSettings.BallType.Basket:
				gameBall[0].SetActive(false);
				gameBall[1].SetActive(true);
				gameBall[2].SetActive(false);
				EventMaster.eMInst.balls = tempBasketBalls;
				ReputationManager.repInst.goldenBall = goldenBall[1];
				FindObjectOfType<BallIndicator>().ball = gameBall[1];
				break;
			case GameSettings.BallType.Bowling:
				gameBall[0].SetActive(false);
				gameBall[1].SetActive(false);
				gameBall[2].SetActive(true);
				EventMaster.eMInst.balls = tempBowlingBalls;
				ReputationManager.repInst.goldenBall = goldenBall[2];
				FindObjectOfType<BallIndicator>().ball = gameBall[2];
				break;
		}
		//On lance le countdown
		ScoreManager.scoreInst.StartCountDown();
		gameObject.SetActive(false);
	}
	#endregion
}
