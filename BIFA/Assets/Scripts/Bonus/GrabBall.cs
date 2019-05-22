using UnityEngine;
using System.Diagnostics;

public class GrabBall : MonoBehaviour
{
    [SerializeField]
    private float _hookSpeed = 50, minDistToBall = .5f, minDistToPlayer = 1f;

    private bool _launched = false;

	[SerializeField]
    private Transform _startPos, _ballPos;

	[SerializeField]
	private GamePlayer _player;

	private Stopwatch _lTimer = new Stopwatch();

	public float lifetime = 5f;

	void OnEnable() {
        _ballPos = GameObject.FindGameObjectWithTag("Ballon").transform;
		_lTimer.Start();
    }

    void Update() {
		if (_player.UseBonus) {
			_launched = true;
			_lTimer.Stop();
		}

		if (_lTimer.Elapsed.Seconds >= lifetime) {
			_lTimer.Reset();
			gameObject.SetActive(false);
		}

		//On regarde vers la balle
		transform.LookAt(_ballPos);

        //Si le grappin est lancé
        if (_launched) {
            //Si la balle est loin du grappin
            if ((_ballPos.position - transform.position).sqrMagnitude > (minDistToBall * minDistToBall))
                //On va vers la balle
                transform.position = Vector3.MoveTowards(transform.position, _ballPos.position, Time.deltaTime * _hookSpeed);
            //Si le grappin est proche de la balle
            else if ((_ballPos.position - transform.position).sqrMagnitude <= (minDistToBall * minDistToBall)) {
                //On attrappe la balle
                _ballPos.GetComponent<Rigidbody>().useGravity = false;
                _ballPos.GetComponent<Rigidbody>().freezeRotation = true;
                _ballPos.GetComponent<Rigidbody>().velocity = Vector3.zero;
				_ballPos.GetComponent<Collider>().enabled = false;
                _ballPos.parent = transform;
            }

            //Si le grappin est loin du joueur
            if (transform.childCount > 0 && (_startPos.position - transform.position).sqrMagnitude > (minDistToPlayer * minDistToPlayer))
                //On ramène la balle au joueur
                transform.position = Vector3.MoveTowards(transform.position, _startPos.position, Time.deltaTime * _hookSpeed);
            //Si le grappin est proche du joueur
            else if (transform.childCount > 0 && (_startPos.position - transform.position).sqrMagnitude <= (minDistToPlayer * minDistToPlayer)) {
                //On lache la balle
                _ballPos.parent = null;
				_ballPos.GetComponent<Collider>().enabled = true;
                _ballPos.gameObject.GetComponent<Rigidbody>().useGravity = true;
                _ballPos.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
				_launched = false;
                gameObject.SetActive(false);
            }
        }else
			transform.position = _startPos.position;

    }

    void OnDisable() {
        transform.position = _startPos.position;
    }
}
