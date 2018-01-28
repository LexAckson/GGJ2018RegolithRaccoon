using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour {

	public bugColor _color;
	public GreenTree _targetTree;
	public bool _isAttacked;
	public float _attackTimer;
	private float _eatTimer;
	private bool _isLanded;
	private Vector3 _startPos;
	private bool _isInit;

	public void Initialize(GreenTree targetTree, bugColor color)
	{
		_targetTree = targetTree;
		_color = color;
		transform.position = _targetTree.transform.position 
				+ _targetTree.transform.position.normalized * Constants.BUG_START_DIST;
		_startPos = transform.position;
		_isInit = true;
		StartCoroutine(land());
	}
	// Update is called once per frame
	void Update () 
	{
		if(_isLanded)
		{
			_eatTimer += Time.deltaTime;
			if(_eatTimer >= Constants.BUG_EAT_TIME)
			{
                //TODO instead, kill the tree if no leaves
				_targetTree.removeResource<Leaf>();
				_eatTimer = 0;
			}
		}
		if(_isAttacked)
		{
			_attackTimer+= Time.deltaTime;
			if(_attackTimer >= Constants.BUG_DIE_TIME)
			{
				_isLanded = false;
				_targetTree.killBug(this);
				die();
			}
		}
	}

	public void die()
	{
		// TODO:
		// animation for dead bug
		Destroy(gameObject);
	}

	private IEnumerator land()
	{
		float timer = 0;
		while(timer / Constants.BUG_DROP_TIME < 1)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(_startPos, _targetTree.transform.position, timer / Constants.BUG_DROP_TIME);
			yield return null;
		}
		_isLanded = true;
	}

}
