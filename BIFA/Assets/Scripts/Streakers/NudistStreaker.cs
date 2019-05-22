#region System & Unity
using System.Collections;
using UnityEngine;
#endregion

#region PathCreation
using PathCreation;
#endregion

public class NudistStreaker : MonoBehaviour
{
    #region Private Variables
    private int life = 3;

    [SerializeField]
    private float _dstTravelled, _speed = 5f;

    [SerializeField]
    private PathCreator _pathCreator;

    private EndOfPathInstruction _end = EndOfPathInstruction.Loop;

    private bool _canMove = true;
    #endregion

    void OnEnable() {
        transform.position = _pathCreator.path.GetPoint(0f);
        _canMove = true;
    }

    void Update() {
        if (_canMove) {
            _dstTravelled += _speed * Time.deltaTime;
            transform.position = _pathCreator.path.GetPointAtDistance(_dstTravelled, _end);
			transform.rotation = _pathCreator.path.GetRotationAtDistance(_dstTravelled, _end);
			//transform.Rotate(Vector3.up * 30*_speed * Time.deltaTime);
        }
    }

    void OnDisable() {
        life = 3;
		EventMaster.eMInst.WaitForNewEvent();
    }

    public void StunAndDisappear() {
        life--;
        if (life == 0)
            StartCoroutine(EventEnd());
        else
            StartCoroutine(EventPause());
    }

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
}
