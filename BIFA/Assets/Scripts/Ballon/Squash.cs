using System.Collections;
using UnityEngine;

public class Squash : MonoBehaviour {
	[SerializeField]
	private float _speed = 5f, _sqStr = .25f, _scaleLim, _minYVel, _minLatVel;

	private bool _isSquashy = false;

	[SerializeField]
	private GameObject _mesh, _direction, _scale;

	private Vector3 _velocity;

	private Rigidbody _rb;

	private void Start() {
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		_mesh.transform.rotation = transform.rotation;
		if (!_isSquashy)
			_scale.transform.localScale = Vector3.SmoothDamp(_scale.transform.localScale, Vector3.one, ref _velocity, Time.fixedDeltaTime * _speed);
	}

	private void OnCollisionEnter(Collision collision) {
		if (_rb.velocity.magnitude > 0.5784079f) {
			if (collision.gameObject.layer == LayerMask.NameToLayer("Sol") && _rb.velocity.y >= _minYVel)
				SquashAndStretch(collision);
			if (collision.gameObject.CompareTag("Limites")) {
				bool canSquash = ((_rb.velocity.x * _rb.velocity.x) + (_rb.velocity.z * _rb.velocity.z)) >= (_minLatVel * _minLatVel);
				if (canSquash)
					SquashAndStretch(collision);
			}
		}
	}

	void SquashAndStretch(Collision col) {
        _direction.transform.forward = -col.contacts[0].normal;
        float val = _rb.velocity.magnitude * _sqStr;
        val = Mathf.Clamp(val, _scaleLim, 1f);
		Vector3 newScale = new Vector3(_scale.transform.localScale.x + val / 2, _scale.transform.localScale.y + val / 2, _scale.transform.localScale.z - val);
		_scale.transform.localScale = newScale;
    }	

	public GameObject Mesh { get => _mesh; }
}
