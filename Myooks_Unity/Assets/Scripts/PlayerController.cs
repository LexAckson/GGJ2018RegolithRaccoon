using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Camera _toRotate;
	private GameObject _dude;
	[SerializeField]
	private GameObject _ground;
	[SerializeField]
	private GameObject _myceliumPrefab;
	[SerializeField]
	private AudioSource _walkSound;
	[SerializeField]
	private AudioSource _playerFx;
	[SerializeField]
	private AudioClip _startPin;
	[SerializeField]
	private AudioClip _endPin;
	private Animator _anim;

	private bool _isTouchingTree;
	private GameObject _currentTree;
    private Queue<Mycelium> _myceliumQueue = new Queue<Mycelium>();
	private Mycelium _currentThread;
	private List<Mycelium> _threadList;
	private bool _isMycelliumMode;
	private int _numPins = Constants.NEEDLE_COUNT;
	private bool _pinDrop;
	private bool _isFacingClockwise = false;

    private void Start()
    {
		_anim = GetComponent<Animator>();
		_toRotate = Camera.main;
        _dude = gameObject;
		_threadList = new List<Mycelium>();
    }

	private void playWalk(bool play)
	{
		if(play && !_walkSound.isPlaying)
			_walkSound.Play();
		else if(!play)
			_walkSound.Pause();
	}

    void Update () {
		if (Input.GetKey("escape"))
            Application.Quit();
		if(Input.GetKey(Constants.RESTART_KEY))
			UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.TITLE_SCREEN);
		if(!_pinDrop)
		{
			if(Input.GetAxis(Constants.HORIZONTAL_AXIS) < 0)
			{
				_dude.transform.RotateAround(_ground.transform.position, Vector3.up, -Constants.MOVE_SPEED * Time.deltaTime * (3f + Constants.GROUND_RADIUS - _dude.transform.position.magnitude) );
				_toRotate.transform.RotateAround(_ground.transform.position, Vector3.up, -Constants.MOVE_SPEED * Time.deltaTime * (3f + Constants.GROUND_RADIUS - _dude.transform.position.magnitude) );
				if(_isFacingClockwise)
					changeDirection();
			}
			if(Input.GetAxis(Constants.HORIZONTAL_AXIS) > 0)
			{
				_dude.transform.RotateAround(_ground.transform.position, Vector3.up, Constants.MOVE_SPEED * Time.deltaTime * (3f + Constants.GROUND_RADIUS - _dude.transform.position.magnitude) );
				_toRotate.transform.RotateAround(_ground.transform.position, Vector3.up, Constants.MOVE_SPEED * Time.deltaTime * (3f + Constants.GROUND_RADIUS - _dude.transform.position.magnitude) );
				if(!_isFacingClockwise)
					changeDirection();
			}
			Vector3 toCenter = (_dude.transform.position - _ground.transform.position).normalized;

			if (Input.GetAxis(Constants.VERTICAL_AXIS) < 0)
				_dude.transform.position = Vector3.MoveTowards(_dude.transform.position, Vector3.zero, Constants.MOVE_SPEED * 0.5f * Time.deltaTime);
			if (Input.GetAxis(Constants.VERTICAL_AXIS) > 0)
				_dude.transform.position = Vector3.MoveTowards(_dude.transform.position, Vector3.zero, -Constants.MOVE_SPEED * 0.5f * Time.deltaTime);

			if(Input.GetAxis(Constants.VERTICAL_AXIS) == 0 && Input.GetAxis(Constants.HORIZONTAL_AXIS) == 0)
			{
				playWalk(false);
				_anim.SetBool("isMoving", false);
			}
			else
			{
				playWalk(true);
				_anim.SetBool("isMoving", true);
			}
		}

        //don't go to far to the center
        if (_dude.transform.position.magnitude < 2f)
            _dude.transform.position = _dude.transform.position.normalized * 2f;
        //don't go off the planet
        if (_dude.transform.position.magnitude > Constants.GROUND_RADIUS)
            _dude.transform.position = _dude.transform.position.normalized * Constants.GROUND_RADIUS;


        //Droppin lines
        if (_isTouchingTree && Input.GetKeyDown(Constants.PIN_DROP))
		{
			_pinDrop = true;
			_anim.SetBool("isDropPin", _pinDrop);
			checkMycelium();
			if(!_isMycelliumMode)
			{
				_playerFx.clip = _startPin;
				_playerFx.Play();
				if ( _numPins == 0 )
            	{
					//mark the first mycellum in the queue for death and remove it
					_threadList[0].MarkForDeath();
					_threadList.RemoveAt(0);
            	}
				_currentThread = Instantiate(_myceliumPrefab, _dude.transform).GetComponent<Mycelium>();
				_currentThread.Init(_currentTree, _dude);
				_isMycelliumMode = true;
				_threadList.Add(_currentThread);
			} 
			
			//don't attach a tree to itself
			else if (_currentThread.startObject != _currentTree)
			{
				_playerFx.clip = _endPin;
					_playerFx.Play();

				_currentThread.PinTheEnd(_currentTree);
				_currentThread = null;
				_numPins = Mathf.Max(_numPins - 1, 0);
				_isMycelliumMode = false;
			}

            
        }
	}

	private void checkMycelium()
	{
		int oldSize = _threadList.Count;
		_threadList.RemoveAll(item => item==null);
		_numPins += oldSize - _threadList.Count;
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