#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class ShooterStreaker : MonoBehaviour
{
	#region Private Variables
	private int life = 3;

	[SerializeField]
	private float _speed;

	private bool _canMove = true;

	private Vector3 _dir;

	private GameObject _target;
	#endregion

	#region Methods
	void Start()
    {
		_target = GameObject.FindGameObjectWithTag("Ballon");
    }

    void Update()
    {
		if (_target != null) {
			if (_canMove) {
				Vector3 targetPos = new Vector3(_target.transform.position.x, 0, _target.transform.position.z);
				_dir = _dir.CalculateDir(transform.position, targetPos, true);
				Debug.DrawRay(transform.position, _dir, Color.red);
				transform.Translate(Vector3.forward * _speed * Time.deltaTime);
				Quaternion targetRot = Quaternion.LookRotation(_dir);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _speed * Time.deltaTime);
			}
		}
    }

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject == _target)
			_target.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);
	}

	public void StunAndDisappear() {
		life--;
		if (life == 0)
			StartCoroutine(EventEnd());
		else
			StartCoroutine(EventPause());
	}

	void OnEnable() {
		_canMove = true;
	}

	void OnDisable() {
		life = 3;
		EventMaster.eMInst.WaitForNewEvent();
	}
	#endregion

	#region Coroutines
	IEnumerator EventPause() {
		_canMove = false;

		yield return new WaitForSeconds(3f);

		_canMove = true;
	}

	IEnumerator EventEnd() {
		_canMove = false;

		yield return new WaitForSeconds(3f);

		gameObject.SetActive(false);
	}
	#endregion
}
