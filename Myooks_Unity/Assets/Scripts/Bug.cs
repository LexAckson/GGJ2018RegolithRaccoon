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
	private AudioSource _bugSounds;
	[SerializeField]
	private AudioClip _bugDie;
	[SerializeField]
	private AudioClip _bugMunch;
	[SerializeField]
	private AudioClip _bugExplode;

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
		
			if(_isAttacked)
				_attackTimer+= Time.deltaTime;
			if(_attackTimer >= Constants.BUG_DIE_TIME)
				killBug(false);
		}
	}

	public void killBug(bool isBomb)
	{
		_isLanded = false;
		_isBombKill = isBomb;
		_isDead = true;
		if(_anim != null)
			_anim.SetBool("isDead", true);
		if(isBomb)
			StartCoroutine(deadBug());
	}

	IEnumerator deadBug()
	{
		yield return new WaitForSeconds(.3f);
		Destroy(this.gameObject);
	}

	void die()
	{
		if(_isBombKill)
		{
			Destroy(gameObject);
			_bugSounds.clip = _bugExplode;
			_bugSounds.Play();
		}
		else
			StartCoroutine(fall());
	}

	private IEnumerator fall()
	{
		Vector3 displacement = (Vector3.zero - transform.position) / 2;
		Vector3 start = transform.position;
		Vector3 target = transform.position + displacement + Utility.offset(.1f,1.5f,.1f,1.5f);
		_bugSounds.clip = _bugDie;
		_bugSounds.Play();
		target.y = 0;
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
		Vector3 endPos = _targetTree._leafSprite.gameObject.transform.position + (Vector3.up * .2f) + Utility.offset(.1f,1.5f,.1f,1.5f);
		while(timer / Constants.BUG_DROP_TIME < 1)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(_startPos, endPos, timer / Constants.BUG_DROP_TIME);
			yield return null;
		}
		_isLanded = true;
		_targetTree.bugLanded(this);
		_anim.SetBool("isOnTree", true);
		_bugSounds.clip = _bugMunch;
		_bugSounds.Play();
	}

}
