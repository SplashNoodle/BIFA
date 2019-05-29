using System.Diagnostics;
using UnityEngine;

using Rewired;

public class IceWall : MonoBehaviour
{
	[SerializeField]
	private int _freezeDuration = 2;

	private bool _startFreeze = false, _isFreezing = false;

	private Stopwatch _freezeTimer = new Stopwatch(), _lTimer = new Stopwatch();

	public float lifetime = 5f;

	public GameObject iceWall, vapor;

	void OnEnable() {
		_lTimer.Start();
		vapor.SetActive(true);
		vapor.GetComponent<ParticleSystem>().Play();
	}
	void Update() {
		_startFreeze = ReInput.players.GetPlayer(GetComponent<GamePlayer>().PlayerInfos.pIndex).GetButtonDown("AbButton2");

		if (_startFreeze) {
			if (!_freezeTimer.IsRunning)
				_freezeTimer.Start();
			_lTimer.Reset();
			vapor.SetActive(false);
		}
		else if (_lTimer.Elapsed.Seconds >= lifetime) {
			_lTimer.Reset();
			vapor.SetActive(false);
			enabled = false;
		}

		if (_freezeTimer.IsRunning) {
			if (_freezeTimer.Elapsed.Seconds < _freezeDuration) {
				_isFreezing = true;
			}
			else {
				_isFreezing = false;
				_freezeTimer.Reset();
				_startFreeze = false;
				enabled = false;
			}

			iceWall.GetComponent<Animator>().SetBool("Freeze", _isFreezing);
		}
	}
}
