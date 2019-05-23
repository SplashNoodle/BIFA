#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

[RequireComponent(typeof(Rigidbody))]
public class Ballon : MonoBehaviour
{

	/*
		Ce script permet de définir le comportement du ballon en jeu.
		Quand un joueur touche le ballon, on applique une force dans la direction joueur-ballon.
		Quand le ballon touche un des bords du terrain, on lui applique une force dans la direction barrière-ballon.
		Si le ballon sort du terrain, il respawne au centre après x secondes.
	*/

	#region Private Variables
	private float _pStr, _upForce, _borderBounce, _maxVel;
    [SerializeField]
    private float maxDist = 100f;

    private bool _touchedGradins = false, _respawning = false;

    private Vector3 _dir = Vector3.zero;

    private Vector3 _currentVel;

    [SerializeField]
    private Transform _spwnPt;

	[SerializeField]
	private GameObject _spawnEffect;

    private Rigidbody _rb;

	private AudioSource _src;
	#endregion

	#region Public Variables
	[Range(0f, 1f)]
	public float defaultVolume = 1f;

	public AudioClip[] kicks;

    public BallInfos bInfos;

	public GlobalSettings settings;
	#endregion
	
	#region Methods
	void Start() {
        _rb = GetComponent<Rigidbody>();
        _pStr = bInfos.playerShootingStrength;
        _upForce = bInfos.groundBouncingValue;
        _borderBounce = bInfos.borderbouncingValue;
        _maxVel = bInfos.maxVelocity;
		_src = GetComponent<AudioSource>();
    }

    void Update() {
        //On vérifie la distance entre la balle et son point de respawn
        float tempDist = Vector3.Distance(transform.position, _spwnPt.position);
        //Si on touche les gradins ou si la distance avec le spawn est supérieure à 15
        if (_touchedGradins || tempDist >= maxDist)
            //On lance la Coroutine de Respawn
            StartCoroutine(Respawn(2f));
		_src.volume = defaultVolume * settings.masterVolume * settings.effectsVolume;
    }

    void FixedUpdate() {
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, Vector3.zero, ref _currentVel, 1);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVel);
    }

    void OnCollisionEnter(Collision col) {
        DetectCollisionType(col.collider.gameObject, col);
    }

    //On détecte si on touche un joueur, les limites ou les gradins
    void DetectCollisionType(GameObject colObj, Collision col) {
        //Si on touche les limites
        if (colObj.tag == "Limites") {
			//On pousse la balle dans la direction opposée à la barrière
			PlayKick();
            _rb.AddForce(_dir.CalculateDir(col.contacts[0].point, transform.position, true) * _borderBounce, ForceMode.Impulse);
        }

        //Si on touche les gradins
        if (colObj.tag == "Gradins") {
            _touchedGradins = true;
        }

        if (colObj.layer == LayerMask.NameToLayer("Sol")) {
			PlayKick();
			_rb.AddForce(Vector3.up * _upForce, ForceMode.Impulse);
        }
    }
	#endregion

	#region Public Methods
	public void PlayKick() {
		_src.clip = kicks[Random.Range(0, kicks.Length)];
		_src.Play();
	}
	#endregion

	#region Public Coroutines
	//Cette coroutine sert à respawner la balle
	//Si la balle est éjectée hors du terrain, on attend plus ou moins longtemps avant le respawn
	public IEnumerator Respawn(float wait) {
		//On s'assure de ne pas lancer la coroutine deux fois d'affilée
		_respawning = true;
        _touchedGradins = false;
		//On désactive la balle
		_spawnEffect.transform.position = transform.position;
		if (transform.parent != null)
			transform.parent = null;
        GetComponent<Squash>().Mesh.GetComponent<MeshRenderer>().enabled = false;
		_spawnEffect.SetActive(true);
        //On attend x secondes
        yield return new WaitForSeconds(wait);
        //On met la balle à la position du spawn
        transform.position = _spwnPt.position;
        //On remet à 0 la velocité de la balle
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
		//On réactive la balle
		GetComponent<Rigidbody>().isKinematic = false;
		_spawnEffect.transform.position = transform.position;
		_spawnEffect.SetActive(true);
		GetComponent<Squash>().Mesh.GetComponent<MeshRenderer>().enabled = true;
		_respawning = false;
        yield return null;
    }

	public IEnumerator RespawnGolden() {
		//On s'assure de ne pas lancer la coroutine deux fois d'affilée
		_respawning = true;
		_touchedGradins = false;
		//On désactive la balle
		_spawnEffect.transform.position = transform.position;
		if (transform.parent != null)
			transform.parent = null;
		GetComponent<Squash>().Mesh.GetComponent<MeshRenderer>().enabled = false;
		_spawnEffect.SetActive(true);
		//On attend x secondes
		yield return new WaitForEndOfFrame();
		//On met la balle à la position du spawn
		transform.position = _spwnPt.position;
		//On remet à 0 la velocité de la balle
		_rb.velocity = Vector3.zero;
		_rb.angularVelocity = Vector3.zero;
		GetComponent<Squash>().Mesh.GetComponent<MeshRenderer>().enabled = true;
		GetComponent<Rigidbody>().isKinematic = false;
		_respawning = false;
		gameObject.SetActive(false);
		yield return null;
	}
	#endregion

	#region Public Getters
	public bool Respawning { get => _respawning; set => _respawning = value; }
	#endregion
}
