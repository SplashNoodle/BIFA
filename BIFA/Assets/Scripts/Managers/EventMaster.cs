#region System & Unity
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
#endregion

public class EventMaster : MonoBehaviour
{
	#region Private Variables
	private bool _activeEvent = false, _startTimer = true;

	private Stopwatch _eventTimer = new Stopwatch();
	#endregion

	#region Public Variables
	public float minTimer, maxTimer;

	public GameObject[] balls, streakers, tempBallIndics;
	#endregion

	#region Public Static Variables
	public static EventMaster eMInst;
	#endregion

	#region Events
	public delegate void OnStreakerEvent();
	public static event OnStreakerEvent onStreakerEvent;
	public void RaiseOnStreakerEvent() {
		UnityEngine.Debug.Log("GAME START");
		onStreakerEvent?.Invoke();
	}
	#endregion

	#region Methods
	void OnEnable() {
		ScoreManager.onGameOver += DisableEvents;
	}

	void OnDisable() {
		ScoreManager.onGameOver -= DisableEvents;
	}

	void Awake() {
		if (eMInst == null)
			eMInst = this;
		else if (eMInst != this) {
			Destroy(gameObject);
			return;
		}
	}

	void Update() {
		if (!_activeEvent) {
			if (_startTimer) {
				//On lance le timer
				_eventTimer.Start();
				_startTimer = false;
			}

			float rTimer = Random.Range(minTimer, maxTimer);
			if (_eventTimer.Elapsed.Seconds >= rTimer) {
				ChoseRandomEvent();
				_eventTimer.Reset();
			}
		}
	}

	void ChoseRandomEvent() {
		float rEvent = Random.Range(0f, 1f);
		if (rEvent <= .25f)
			Multiballs();
		else
			SpawnStreaker();
		_activeEvent = true;
	}

	void Multiballs() {
		//On spawne des balles temporaires supplémentaires
		for (int i = 0; i < balls.Length; i++) {
			balls[i].SetActive(true);
			tempBallIndics[i].SetActive(true);
		}
	}

	void SpawnStreaker() {
		//On spawne un streaker aléatoire sur le terrain
		int rStreaker = Random.Range(0, streakers.Length);
		streakers[rStreaker].SetActive(true);
		RaiseOnStreakerEvent();
	}
	#endregion

	#region Public Methods
	public void WaitForNewEvent() {
		_startTimer = true;
		_activeEvent = false;
	}

	public void DisableEvents() {
		enabled = false;
	}
	#endregion
}
