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
	private bool _isBombKill;
	public bool _isDead;
	private Animator _anim;

	public void Initialize(GreenTree targetTree, bugColor color, RuntimeAnimatorController animatorController, Sprite sprite)
	{
		_targetTree = targetTree;
		_anim = GetComponent<Animator>();
		GetComponent<SpriteRenderer>().sprite = sprite;
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animatorController);
		GetComponent<Animator>().runtimeAnimatorController = animatorOverrideController;
		_color = color;
		transform.position = _targetTree.transform.position 
				+ _targetTree.transform.position.normalized * Constants.BUG_START_DIST;
		_startPos = transform.position;
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
                if(_targetTree.getResourceNum(typeof(Leaf)) > 0)
					_targetTree.removeResource<Leaf>();
				else
				{
					BugFactory.deadTree(_targetTree);
				}
				_eatTimer = 0;
			}
		
			_isAttacked = _targetTree._activeColor.Contains(_color);

			if(_isAttacked)
			{
				_attackTimer+= Time.deltaTime;
				if(_attackTimer >= Constants.BUG_DIE_TIME)
				{
					killBug(false);
				}
			}
		}
	}

	public void killBug(bool isBomb)
	{
		_isLanded = false;
		_isBombKill = isBomb;
		_targetTree.killBug(this);
		_isDead = true;
		_anim.SetBool("isDead", true);
	}

	void die()
	{
		if(_isBombKill)
			Destroy(gameObject);
		else
			StartCoroutine(fall());
	}

	private IEnumerator fall()
	{
		Vector3 displacement = 2 * (Vector3.zero - transform.position.normalized);
		Vector3 start = transform.position;
		Vector3 target = transform.position + displacement;
		target.y = -.15f;
		float timer = 0;
		while(timer / (Constants.BUG_DROP_TIME / 2) < 1)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(start, target, timer / (Constants.BUG_DROP_TIME / 2));
		}
		yield return null;
	}

	private IEnumerator land()
	{
		float timer = 0;
		while(timer / Constants.BUG_DROP_TIME < 1)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(_startPos, _targetTree._leafSprite.gameObject.transform.position + (Vector3.up * .02f), timer / Constants.BUG_DROP_TIME);
			yield return null;
		}
		_isLanded = true;
		_anim.SetBool("isOnTree", true);
	}

}
