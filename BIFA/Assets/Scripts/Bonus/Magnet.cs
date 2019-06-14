using System.Diagnostics;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private int _duration = 5;

    [SerializeField, Range(.1f, 10)]
    private float _forceDivider = 1f;

    [SerializeField]
    private float _speed = 1f, _amount = .1f, _defaultVolume;

    private float _startX, _startY, _startZ;

    private GameObject _ball;

	private AudioSource _src;

    private Stopwatch _timer = new Stopwatch(), _lTimer = new Stopwatch();

	public float lifetime = 5f;

	public GameObject effect;

	public GamePlayer player;

	void OnEnable() {
        _ball = GameObject.FindGameObjectWithTag("Ballon");
		_src = GetComponent<AudioSource>();
		_defaultVolume = _src.volume;
		_lTimer.Start();
        _startX = transform.position.x;
        _startY = transform.position.y;
        _startZ = transform.position.z;
    }

    void Update() {
		_src.volume = _defaultVolume * SoundManager.sInst.settings.effectsVolume * SoundManager.sInst.settings.effectsVolume;
		if (player.UseBonus) {
			_timer.Start();
			_lTimer.Stop();
		}

		if (_timer.IsRunning) {
			if (!_src.isPlaying)
				_src.Play();
			if (!effect.activeSelf)
				effect.SetActive(true);
			float posX, posY, posZ;
			//On fait trembler l'aimant
			posX = _startX + Mathf.Sin(Time.time * _speed) * Random.Range(-_amount, _amount);
			posY = _startY + Mathf.Sin(Time.time * _speed) * Random.Range(-_amount, _amount);
			posZ = _startZ + Mathf.Sin(Time.time * _speed) * Random.Range(-_amount, _amount);

			transform.position = new Vector3(posX, posY, posZ);

			//On attire la balle pendant la durée définie
			if (_timer.Elapsed.Seconds < _duration) {
				_ball.GetComponent<Rigidbody>().AddForce((transform.position - _ball.transform.position) / _forceDivider, ForceMode.Acceleration);
			}
		}


        //Si on atteint la fin du timer, on désactive l'aimant
        if (_timer.Elapsed.Seconds == _duration || _lTimer.Elapsed.Seconds >= lifetime) {
			_src.Stop();
            _timer.Stop();
			_lTimer.Stop();
            gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        _timer.Reset();
		_lTimer.Reset();
		effect.SetActive(false);
    }
}
