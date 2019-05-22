#region System & Unity
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
#endregion

#region TextMeshPro
using TMPro;
#endregion

public class ScoreManager : MonoBehaviour
{

	/*
		Ce script gère les scores.
	*/

	#region Private Variables
	private int _score1 = 0;
	private int _score2 = 0;

	private float _blurVal = 2f;

	private bool _deathMatch = false;

	[SerializeField]
	private GameObject _blurEffect;

	[SerializeField]
	private Animator _camEffect;

	private GameObject _ball;

	private GameObject[] _players;

	[SerializeField]
	private TextMeshProUGUI _endTxt, _cdText, _durationDisplay, _pauseTimeDisplay, _gameOverTimeDisplay;

	private Stopwatch _matchDuration;
	#endregion

	#region Public Variables
	//On met le score maximum à 5 par défaut
	public int maxValue = 5;

	public GameObject gameOverScreen;

	public GameSettings.GameType gt;

	public GameSettings gs;
	#endregion

	#region Public Static Variables
	public static ScoreManager scoreInst;
	#endregion

	#region Events
	public delegate void OnGameStart();
	public static event OnGameStart onGameStart;
	public void RaiseOnGameStart() {
		UnityEngine.Debug.Log("GAME START");
		onGameStart?.Invoke();
	}

	public delegate void OnGameOver();
	public static event OnGameOver onGameOver;
	public void RaiseOnGameOver() {
		UnityEngine.Debug.Log("GAME OVER");
		onGameOver?.Invoke();
		StartCoroutine(GameEnd());
	}
	#endregion

	#region Methods
	void OnEnable() {
		onGameStart += SetEventsAndObjects;
		GameManager.onGamePause += StopTimer;
		GameManager.onGameResume += ResumeTimer;
	}

	void OnDisable() {
		onGameStart -= SetEventsAndObjects;
		GameManager.onGamePause -= StopTimer;
		GameManager.onGameResume -= ResumeTimer;
	}

	void Awake() {
		//S'il y a un doublon de l'objet, on détruit le doublon.
		if (scoreInst == null) {
			scoreInst = this;
		}
		else if (scoreInst != this) {
			Destroy(gameObject);
			return;
		}

		//On empêche le ballon de tomber tant qu'on a pas validé le nombre de buts maximum
		_ball = GameObject.FindGameObjectWithTag("Ballon");
		_ball.GetComponent<Rigidbody>().useGravity = false;

		//On empêche les joueurs de se déplacer tant qu'on a pas validé le nombre de buts maximum
		_players = GameObject.FindGameObjectsWithTag("Player");

		//On s'assure que les effets de menu sont actifs dès le début du jeu
		_camEffect.SetBool("Desat", true);
		_blurEffect.GetComponent<Image>().material.SetFloat("_Size", _blurVal);
		//On s'assure que les événements ne sont pas actifs au début du jeu
		Camera.main.GetComponent<EventMaster>().enabled = false;

		//On crée le timer qui chronomètre la durée du match
		_matchDuration = new Stopwatch();
	}

	void Update() {
		switch (gt) {
			case GameSettings.GameType.Score:
				//Si le score d'une des équipes atteint le score maximum
				if (_score1 == maxValue || _score2 == maxValue) {
					//On désactive les événements aléatoires
					Camera.main.GetComponent<EventMaster>().enabled = false;
					//On désactive les bonus
					Camera.main.GetComponent<ReputationManager>().enabled = false;

					//On désactive la balle
					if (_ball != null)
						_ball.SetActive(false);

					//On arrête le timer
					_matchDuration.Stop();

					//On update le texte
					CheckPerfectScore();
					RaiseOnGameOver();
					gameOverScreen.SetActive(true);
				}
				else
					UpdateDuration();
				break;
			case GameSettings.GameType.Time:
				if (_matchDuration.Elapsed.Minutes < maxValue) {
					UpdateDuration();
				}
				else if (_matchDuration.Elapsed.Minutes == maxValue) {
					_matchDuration.Stop();
					//On désactive les événements aléatoires
					Camera.main.GetComponent<EventMaster>().enabled = false;
					//On désactive les bonus
					Camera.main.GetComponent<ReputationManager>().enabled = false;
					CheckScore();
				}
				break;
		}

	}

	void CheckScore() {
		if ((_score1 > _score2 || _score2 > _score1) && !_deathMatch) {
			if (_ball != null)
				_ball.SetActive(false);
			CheckPerfectScore();
			RaiseOnGameOver();
			gameOverScreen.SetActive(true);
		}
		else {
			CheckDeathScore();
		}
	}

	void CheckPerfectScore() {
		//On vérifie si on a un score parfait
		if ((_score1 == maxValue && _score2 == 0) || (_score1 == 0 && _score2 == maxValue)) {
			//On change le texte en fonction de l'équipe gagnante
			if (_score1 > _score2) {
				_endTxt.text = "The blue team crushed the red team!";
				_endTxt.color = new Color(0f, .5f, 1f);
			}
			if (_score2 > _score1) {
				_endTxt.text = "The red team crushed the blue team";
				_endTxt.color = new Color(1f, .5f, 0f);
			}
		}
		else {
			if (_score1 > _score2) {
				//On change le texte
				_endTxt.text = "Blue team wins!";
				_endTxt.color = new Color(0.01568628f, 0.1490196f, 0.7098039f);
			}
			if (_score2 > _score1) {
				//On change le texte
				_endTxt.text = "Red team wins!";
				_endTxt.color = new Color(0.6941177f, 0.007843138f, 0.02352941f);
			}
		}
	}

	void CheckDeathScore() {
		if ((_score1 == 1 && _score2 == 0) || (_score1 == 0 && _score2 == 1)) {
			if (_ball != null)
				_ball.SetActive(false);
			RaiseOnGameOver();
			gameOverScreen.SetActive(true);
			//On change le texte en fonction de l'équipe gagnante
			if (_score1 > _score2) {
				_endTxt.text = "Blue team wins... but was really slow!";
				_endTxt.color = new Color(0f, .5f, 1f);
			}
			if (_score2 > _score1) {
				_endTxt.text = "Red team wins... but was really slow!";
				_endTxt.color = new Color(1f, .5f, 0f);
			}
		}
	}

	void StopTimer() {
		_matchDuration.Stop();
	}

	void ResumeTimer() {
		_matchDuration.Start();
	}

	void UpdateDuration() {

		int tempS = 0;
		int tempM = 0;
		int tempH = 0;

		tempS = _matchDuration.Elapsed.Seconds % 60;
		tempM = _matchDuration.Elapsed.Minutes % 60;
		tempH = _matchDuration.Elapsed.Hours % 24;

		switch (gt) {
			case GameSettings.GameType.Score:
				if (tempH == 0)
					_durationDisplay.text = tempM.ToString("00") + ":" + tempS.ToString("00");
				else
					_durationDisplay.text = tempH.ToString() + ":" + tempM.ToString("00") + ":" + tempS.ToString("00");
				_gameOverTimeDisplay.text = _durationDisplay.text;
				_pauseTimeDisplay.text = _durationDisplay.text;
				break;
			case GameSettings.GameType.Time:
				_gameOverTimeDisplay.text = maxValue.ToString("00") + ":" + 0.ToString("00");
				if (_matchDuration.Elapsed.Seconds == 0)
					_durationDisplay.text = maxValue.ToString("00") + ":" + 0.ToString("00");
				else
					_durationDisplay.text = (maxValue - 1 - tempM).ToString("00") + ":" + (59 - tempS).ToString("00");
				_pauseTimeDisplay.text = _durationDisplay.text;
				break;
		}

	}
	#endregion

	#region Public Methods
	public void UpdateDisplay(TextMeshProUGUI display, int score) {
		//On met à jour le score dans l'affichage
		display.text = score.ToString();
	}

	public void StartCountDown() {
		StartCoroutine(CountDown());
	}

	public void SetEventsAndObjects() {
		GetComponent<EventMaster>().enabled = gs.events;
		GetComponent<ReputationManager>().enabled = gs.objects;
	}
	#endregion

	#region Coroutines
	IEnumerator CountDown() {
		//On enlève les effets de menu
		_camEffect.SetBool("Desat", false);
		_blurVal = Mathf.Lerp(_blurVal, 0, .25f);
		yield return new WaitForSeconds(.25f);
		_blurEffect.GetComponent<Image>().material.SetFloat("_Size", 0f);
		_blurEffect.SetActive(false);

		//On lance le countdown
		_cdText.enabled = true;
		for (int i = 3; i > 0; i--) {
			_cdText.text = i.ToString();
			yield return new WaitForSeconds(.75f);
		}
		//Quand le countdown est terminé
		_cdText.text = "GO !";
		_ball.GetComponent<Rigidbody>().useGravity = true;
		RaiseOnGameStart();
		//On passe le mode de jeu à Game dans le GameManager
		GameManager.gmInst.mode = GameManager.GameMode.Game;
		yield return new WaitForSeconds(.75f);
		_cdText.enabled = false;
		//On s'assure que la durée du match est bien de zéro au début de la partie
		_matchDuration.Reset();
		_matchDuration.Start();
	}

	IEnumerator GameEnd() {
		_camEffect.SetBool("Desat", true);
		_blurEffect.SetActive(true);
		_blurVal = Mathf.Lerp(_blurVal, 2f, .25f);
		yield return new WaitForSeconds(.25f);
		_blurEffect.GetComponent<Image>().material.SetFloat("_Size", 2f);
	}
	#endregion

	#region Public Properties
	public int Score1 { get => _score1; set => _score1 = value; }
	public int Score2 { get => _score2; set => _score2 = value; }
	#endregion
}
