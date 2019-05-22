using System.Diagnostics;
using UnityEngine;

public class StunPlayer : MonoBehaviour
{
    [SerializeField]
    private int _stunDuration = 1;

	private bool collided = false;

    private GameObject _otherPlayer;

    private Stopwatch _stunTimer = new Stopwatch(), _lTimer = new Stopwatch();

	public float lifetime = 5f;

	public GameObject lightning, circle;

	void Start() {
		_lTimer.Start();
		circle.SetActive(true);
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Player") && enabled) {
			collided = true;
            _otherPlayer = col.gameObject;
			lightning.transform.position = _otherPlayer.transform.position + Vector3.up * 2;
			lightning.SetActive(true);
            _otherPlayer.GetComponent<GamePlayer>().Stunned = true;
			_otherPlayer.GetComponent<GamePlayer>().StunEffect.SetActive(true);
            _stunTimer.Start();
        }
    }

    void Update() {
		if (collided) {
			_lTimer.Reset();
			if (_stunTimer.Elapsed.Seconds >= _stunDuration) {
				lightning.SetActive(false);
				_otherPlayer.GetComponent<GamePlayer>().Stunned = false;
				_otherPlayer.GetComponent<GamePlayer>().StunEffect.SetActive(false);
				_stunTimer.Reset();
				enabled = false;
			}
		}
		else if (_lTimer.Elapsed.Seconds >= lifetime) {
			_lTimer.Reset();
			enabled = false;
		}
    }
}
