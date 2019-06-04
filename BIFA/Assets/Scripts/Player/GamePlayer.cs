#region System & Unity
using System.Collections;
using System.Diagnostics;
using UnityEngine;
#endregion

#region Rewired
using Rewired;
#endregion

[RequireComponent(typeof(Rigidbody), typeof(IceWall), typeof(StunPlayer))]
public class GamePlayer : MonoBehaviour
{

	/*
		Ce script contrôle le comportement du personnage en jeu.
		Quand on utilise le stick analogique gauche, on déplace le personnage.
		Quand on entre en collision avec un joueur, on pousse le joueur entré en collision.
		Quand on appuie sur un bouton, le personnage dashe.
	*/

	#region Private Variables
	[SerializeField]
	private float _v, _h, _targetSpeed = 0f, _maxSpeed = 8f, _wantedSpeed = 0f, _accel = 6f, _pStr = 30f, _currentSpeed, _ejectForce = 2.5f, _dashCooldown = 1f, _releaseTime = 1f;

	private bool _dashed, _useBonus = false, _allowDash = true, _stunned = false, _hasBall = false, _canMove = true;

	[SerializeField]
	private GameObject _triggerObject, _stunEffect;
	private GameObject _ball;

	[SerializeField]
	private GameObject[] _meshes;

	private Vector3 _bump = Vector3.zero, _dir = Vector3.forward;

	private Rigidbody _rb;

	private Animator _anim;

	private TrailRenderer _trail;

	private Stopwatch _releaseTimer = new Stopwatch();

	[SerializeField]
	private PInfos _pInfos;
	#endregion

	#region Public Variables
	public float dashSpeed = 17.5f;

	public bool isDashing = false;

	public Vector3 move;

	public GameObject hook;

	public Animator dashUIAnim;

	public enum MoveMode
	{
		Main,
		Menu
	}

	public MoveMode moveMode = MoveMode.Menu;
	#endregion

	#region Methods
	void OnEnable() {
		ScoreManager.onGameStart += EnableMove;
		ScoreManager.onGameOver += DisableMove;
	}
	void OnDisable() {
		ScoreManager.onGameStart -= EnableMove;
		ScoreManager.onGameOver -= DisableMove;
	}

	void Start() {
		_rb = GetComponent<Rigidbody>();
		_trail = GetComponent<TrailRenderer>();
		_trail.enabled = false;
		GetComponent<IceWall>().enabled = false;
		GetComponent<StunPlayer>().enabled = false;
		_meshes[_pInfos.characterIndex].SetActive(true);
		_anim = _meshes[_pInfos.characterIndex].GetComponent<Animator>();
		_anim.SetBool("Moving", false);
		if (dashUIAnim != null)
			dashUIAnim.SetFloat("DashRecovery", _dashCooldown);
		if (hook != null)
			hook.SetActive(false);
	}

	void FixedUpdate() {
		_anim.SetFloat("Speed", _currentSpeed);
		_rb.velocity = Vector3.zero;
		switch (moveMode) {
			case MoveMode.Main:
				if (!Stunned) {
					//On récupère les inputs
					_h = ReInput.players.GetPlayer(_pInfos.pIndex).GetAxis("Hor");
					_v = ReInput.players.GetPlayer(_pInfos.pIndex).GetAxis("Ver");
					_dashed = ReInput.players.GetPlayer(_pInfos.pIndex).GetButtonDown("AbButton1");
					if (_dashed && _allowDash)
						_anim.SetTrigger("Dashed");
					if (_canMove)
						InGameMove();
					else {
						if (ReInput.players.GetPlayer(_pInfos.pIndex).GetButton("AbButton1"))
							WaitForRelease();
						else
							ReleaseBall();
					}

					InGameDirection();
					_useBonus = ReInput.players.GetPlayer(_pInfos.pIndex).GetButtonDown("AbButton2");
				}
				if (_bump.magnitude != 0) {
					_bump = Vector3.Lerp(_bump, Vector3.zero, 10 * Time.deltaTime);
				}
				//On ajoute le bump
				transform.position += _bump * Time.fixedDeltaTime;
				break;
			case MoveMode.Menu:
				break;
		}
	}

	void InGameMove() {
		//Si le joueur dirige le stick directionnel, on attribue une valeur à _targetSpeed en fonction des inputs
		if (_h != 0 || _v != 0) {
			_targetSpeed = _maxSpeed * (Mathf.Abs(_h) + Mathf.Abs(_v));
			_targetSpeed = Mathf.Clamp(_targetSpeed, 0, _maxSpeed);
			if (isDashing || _currentSpeed > _maxSpeed)
				_accel = 100f;
			else
				_accel = 6f;
			_anim.SetBool("Moving", true);
		}
		else {
			_targetSpeed = 0f;
			_accel = 4f;
			_anim.SetBool("Moving", false);
		}

		if (_dashed && _allowDash)
			StartCoroutine(Dash());

		//On attribue _playerSpeed en fonction de _targetSpeed
		_currentSpeed = Mathf.SmoothDamp(_currentSpeed, _targetSpeed, ref _wantedSpeed, .5f / _accel);
		transform.Translate(Vector3.right * _currentSpeed * Time.fixedDeltaTime);

		_rb.velocity = _rb.angularVelocity = Vector3.zero;
	}

	void InGameDirection() {
		if (!isDashing) {
			if (_h != 0 || _v != 0) {
				_dir = Vector3.Lerp(_dir, new Vector3(-_v, 0f, _h).normalized, 10 * Time.fixedDeltaTime);
				transform.rotation = Quaternion.LookRotation(_dir);
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		//Si on entre en collision avec un autre joueur
		if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			//On applique une force au joueur touché dans la direction opposée à l'endroit où on l'a touché
			float colSpeed = col.gameObject.GetComponent<GamePlayer>().CurrentSpeed;
			_bump = colSpeed != 0 ? (-(col.transform.position - transform.position) * _ejectForce * colSpeed) : (-(col.transform.position - transform.position) * _ejectForce * .1f);
		}

		if ((col.gameObject.CompareTag("Ballon") || col.gameObject.CompareTag("TempBallon")) || col.gameObject.CompareTag("GoldenBall") && !isDashing) {
			float str = _pStr * ((_currentSpeed / _maxSpeed) + .1f);
			col.gameObject.GetComponent<Ballon>().PlayKick();
			col.gameObject.GetComponent<Rigidbody>().velocity = (col.transform.position - transform.position) * str;
		}

		if (col.gameObject.CompareTag("Streaker") && isDashing) {
			if (col.gameObject.GetComponent<NudistStreaker>())
				col.gameObject.GetComponent<NudistStreaker>().StunAndDisappear();
			if (col.gameObject.GetComponent<ShooterStreaker>())
				col.gameObject.GetComponent<ShooterStreaker>().StunAndDisappear();
		}
	}

	void OnTriggerEnter(Collider other) {
		//On parente la balle au joueur
		if (other.CompareTag("Ballon") || other.CompareTag("GoldenBall")) {
			Grab(other);
			_ball = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Ballon") || other.CompareTag("GoldenBall")) {
			if (_ball != null) {
				if (_ball.transform.parent == this)
					_ball.transform.parent = null;
				_ball = null;
				_hasBall = false;
				_canMove = true;
			}
			_triggerObject.SetActive(false);
		}
	}

	void Grab(Collider c) {
		c.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
		c.transform.GetComponent<Rigidbody>().isKinematic = true;
		c.transform.position = new Vector3(transform.position.x, 0, transform.position.z) + transform.right * 1.5f + (transform.up * (c.transform.GetComponent<SphereCollider>().radius / 2f + .2f));
		c.transform.parent = transform;
		_hasBall = true;
	}

	void WaitForRelease() {
		_anim.SetBool("Moving", false);
		//On lance le timer
		_releaseTimer.Start();
		//Si le joueur appuie avant la fin du timer
		if (!ReInput.players.GetPlayer(_pInfos.pIndex).GetButton("AbButton1") || _releaseTimer.Elapsed.Seconds >= (int)_releaseTime) {
			//On relâche la balle
			if (_ball != null)
				ReleaseBall();
			//On reset le timer
			_releaseTimer.Reset();
		}
	}

	void ReleaseBall() {
		if (_ball != null) {
			UnityEngine.Debug.Log("RELEASE BALL");
			//On déparente la balle
			_ball.transform.parent = null;
			//On repasse le rigidbody de la balle en dynamique
			_ball.GetComponent<Rigidbody>().isKinematic = false;
			//On applique une force à la balle en fonction de la direction dans laquelle on regarde et de la force du joueur
			_ball.GetComponent<Rigidbody>().AddForce((_ball.transform.position - transform.position) * _pStr, ForceMode.Impulse);
			//On reset le gameObject _ball
			_ball = null;
			//On reset le booléen _hasball
			_hasBall = false;
			//On autorise le joueur à bouger
			_canMove = true;
		}
	}
	#endregion

	#region Public Methods
	public void EnableMove() {
		moveMode = MoveMode.Main;
	}

	public void DisableMove() {
		moveMode = MoveMode.Menu;
	}
	#endregion

	#region Coroutines
	IEnumerator Dash() {
		//On empêche la possibilité de dasher
		_allowDash = false;
		float temp = _maxSpeed;
		if (dashUIAnim != null)
			dashUIAnim.SetTrigger("Dashed");
		//On active le trail renderer
		_trail.enabled = true;
		//On active le trigger
		_triggerObject.SetActive(true);
		//On set isDashing
		isDashing = true;
		//On augmente la vitesse maximale le temps du dash
		_maxSpeed = dashSpeed;
		//On set les animators
		yield return new WaitForSeconds(.2f);
		//On reset isDashing
		isDashing = false;
		//On désactive le trail renderer
		_trail.enabled = false;
		//Si la balle n'a pas été récupérée, on désactive le trigger
		if (!_hasBall)
			_triggerObject.SetActive(false);
		else {
			_canMove = false;
		}
		//On reset la vitesse maximale
		_maxSpeed = temp;
		yield return new WaitForSeconds(_dashCooldown);
		//On autorise la possibilité de dasher
		_allowDash = true;
		yield return null;
	}
	#endregion

	#region Public Properties
	public bool Stunned { get => _stunned; set => _stunned = value; }
	public bool CanMove { get => _canMove; set => _canMove = value; }
	#endregion

	#region Public Getters
	public float CurrentSpeed { get => _currentSpeed; }
	public bool UseBonus { get => _useBonus; }
	public PInfos PlayerInfos { get => _pInfos; }
	public GameObject StunEffect { get => _stunEffect; }
	#endregion
}
