using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private GameObject _dude;
	[SerializeField]
	private GameObject _ground;
	[SerializeField]
	private GameObject _myceliumPrefab;

	private bool _isTouchingTree;
	private GameObject _currentTree;
	private Mycelium _currentThread;
	private bool _isMycelliumMode;
	private int _numPins = Constants.NEEDLE_COUNT;

    private void Start()
    {
        _dude = gameObject;
    }

    void Update () {
		if(Input.GetAxis(Constants.HORIZONTAL_AXIS) < 0)
			_dude.transform.RotateAround(_ground.transform.position, Vector3.up, -Constants.MOVE_SPEED);
	
		if(Input.GetAxis(Constants.HORIZONTAL_AXIS) > 0)
			_dude.transform.RotateAround(_ground.transform.position, Vector3.up, Constants.MOVE_SPEED);

        if (isPointingIn(new Vector3(Input.GetAxis(Constants.HORIZONTAL_AXIS), 0, Input.GetAxis(Constants.VERTICAL_AXIS))))
        {
            _dude.transform.Translate((_dude.transform.position - _ground.transform.position).normalized * Constants.MOVE_SPEED);
        }

        if (_isTouchingTree && _numPins > 0 && Input.GetKeyDown(Constants.PIN_DROP))
		{
			if(!_isMycelliumMode)
			{
				_currentThread = Instantiate(_myceliumPrefab, _currentTree.transform).GetComponent<Mycelium>();
				_currentThread.Init(_currentTree, _dude);
				_isMycelliumMode = true;
			}

			else
			{
				_currentThread.endObject = _currentTree;
				_currentThread = null;
				_numPins = Mathf.Max(_numPins - 1, 0);
				_isMycelliumMode = false;
			}
		}
	}

	public bool isPointingIn(Vector3 inputAxis)
	{
		// Vector3 toCenter = _dude.transform.position - _ground.transform.position;
		// float angleDiff = Vector3.Dot(toCenter,inputAxis);

		// return angleDiff < -.85f;
		return false;

	}



	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag(Tags.TREE))
		{
			_isTouchingTree = true;
			_currentTree = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag(Tags.TREE))
		{
			_isTouchingTree = false;
			_currentTree = null;
		}
	}
}
