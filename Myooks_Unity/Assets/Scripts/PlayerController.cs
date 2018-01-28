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
	private Animator _anim;

	private bool _isTouchingTree;
	private GameObject _currentTree;
    private Queue<Mycelium> _myceliumQueue = new Queue<Mycelium>();
	private Mycelium _currentThread;
	private bool _isMycelliumMode;
	private int _numPins = Constants.NEEDLE_COUNT;
	private bool _pinDrop;
	private bool _isFacingClockwise = false;

    private void Start()
    {
		_anim = GetComponent<Animator>();
        _dude = gameObject;
    }

    void LateUpdate () {
		if (Input.GetKey("escape"))
            Application.Quit();
		if(Input.GetKey(Constants.RESTART_KEY))
			UnityEngine.SceneManagement.SceneManager.LoadScene("main");
		if(!_pinDrop)
		{
			if(Input.GetAxis(Constants.HORIZONTAL_AXIS) < 0)
			{
				_dude.transform.RotateAround(_ground.transform.position, Vector3.up, -Constants.MOVE_SPEED * Time.deltaTime * (1f + Constants.GROUND_RADIUS - _dude.transform.position.magnitude) );
				if(_isFacingClockwise)
					changeDirection();
			}
			if(Input.GetAxis(Constants.HORIZONTAL_AXIS) > 0)
			{
				_dude.transform.RotateAround(_ground.transform.position, Vector3.up, Constants.MOVE_SPEED * Time.deltaTime * (1f + Constants.GROUND_RADIUS - _dude.transform.position.magnitude) );
				if(!_isFacingClockwise)
					changeDirection();
			}
			Vector3 toCenter = (_dude.transform.position - _ground.transform.position).normalized;

			if (Input.GetAxis(Constants.VERTICAL_AXIS) < 0)
				_dude.transform.position = Vector3.MoveTowards(_dude.transform.position, Vector3.zero, Constants.MOVE_SPEED * 0.2f * Time.deltaTime);
			if (Input.GetAxis(Constants.VERTICAL_AXIS) > 0)
				_dude.transform.position = Vector3.MoveTowards(_dude.transform.position, Vector3.zero, -Constants.MOVE_SPEED * 0.2f * Time.deltaTime);

			if(Input.GetAxis(Constants.VERTICAL_AXIS) == 0 && Input.GetAxis(Constants.HORIZONTAL_AXIS) == 0)
				_anim.SetBool("isMoving", false);
			else
				_anim.SetBool("isMoving", true);
		}

        //don't go to far to the center
        if (_dude.transform.position.magnitude < 1f)
            _dude.transform.position = _dude.transform.position.normalized;
        //don't go off the planet
        if (_dude.transform.position.magnitude > Constants.GROUND_RADIUS)
            _dude.transform.position = _dude.transform.position.normalized * Constants.GROUND_RADIUS;


        //Droppin lines
        if (_isTouchingTree && Input.GetKeyDown(Constants.PIN_DROP))
		{
			_pinDrop = true;
			_anim.SetBool("isDropPin", _pinDrop);
			if(!_isMycelliumMode)
			{
				_currentThread = Instantiate(_myceliumPrefab, _dude.transform).GetComponent<Mycelium>();
				_currentThread.Init(_currentTree, _dude);
				_isMycelliumMode = true;
                _myceliumQueue.Enqueue(_currentThread);
			} //don't attach a tree to itself
			else if (_currentThread.startObject != _currentTree)
			{
				_currentThread.endObject = _currentTree;
				_currentThread = null;
				_numPins = Mathf.Max(_numPins - 1, 0);
				_isMycelliumMode = false;
			}

            if ( _numPins == 0 )
            {
                //mark the first mycellum in the queue for death and remove it
                //Mycelium myceliumToDie = _myceliumQueue.Dequeue();
                //myceliumToDie.MarkForDeath();
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
        //can be a bug or a tree, save these to possibly drop pins
        if (other.gameObject.CompareTag(Tags.TREE) || (other.gameObject.CompareTag(Tags.BUG) && 
														other.gameObject.GetComponent<Bug>()._isDead))
		{
			_isTouchingTree = true;
			_currentTree = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
        //can be a bug or a tree, save these to possibly drop pins
        if (_isTouchingTree && 
			(other.gameObject.CompareTag(Tags.TREE) || (other.gameObject.CompareTag(Tags.BUG) && 
														other.gameObject.GetComponent<Bug>()._isDead) ))
		{
			_isTouchingTree = false;
			_currentTree = null;
		}
	}

	void dropPinEnd()
	{
		_pinDrop = false;
		_anim.SetBool("isDropPin", _pinDrop);
	}

	void changeDirection()
	{
		_isFacingClockwise = !_isFacingClockwise;
		transform.Rotate(Vector3.up, 180f, Space.Self);
	}
}
