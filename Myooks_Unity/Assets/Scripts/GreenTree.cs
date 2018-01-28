using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GreenTree : MonoBehaviour {

	[SerializeField]
	private TreeSpriteInfo _allTreeSpriteInfo;
	[SerializeField]
	public SpriteRenderer _leafSprite;
	public int _toxins;
	[SerializeField]
	private List<ResourcePrefab> _resourcePrefabsList;
    private Dictionary<string, GameObject> _resourcePrefabs;
    public Dictionary<Type , Queue<Resource>> _resources;
	public List<Bug> _bugs;

	private AudioSource _treeSounds;
	[SerializeField]
	private AudioClip _treeDie;
	[SerializeField]
	private AudioClip _leafDie;
	public bugColor _color;
	public List<bugColor> _activeColor;

	private bool _inSun;
	private float _sunTimer;
	private bool _isDead;

	void Start () 
	{
		_activeColor = new List<bugColor>();
		_resources = new Dictionary<Type , Queue<Resource>>();
        _resourcePrefabs = new Dictionary<string, GameObject>();
        foreach(ResourcePrefab r in _resourcePrefabsList) {
            _resources[Type.GetType(r.name)] = new Queue<Resource>();
            _resourcePrefabs.Add(r.name, r.stuff);
        }
		for(int i = 0; i < Constants.LEAF_COUNT; i++)
		{
			makeResource<Leaf>();
		}

		updateLeafSprite();
		GetComponent<SpriteRenderer>().sprite = _allTreeSpriteInfo.getTreeSprite(_color);
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(_allTreeSpriteInfo.getAnimator(_color));
		GetComponent<Animator>().runtimeAnimatorController = animatorOverrideController;
		_bugs = new List<Bug>();
		_activeColor.Add(_color);
		StartCoroutine(LeafRegrowCheck());
		
	}
	
	void Update () 
	{
		if(_isDead)
			return;

		if(_inSun)
		{
			_sunTimer += Time.deltaTime;
		}


		if(_sunTimer >= Constants.NUTRIENT_MAKE_TIMER)
		{
			_sunTimer = 0;
			makeResource<Nutrient>();
		}
		updateLeafSprite();
		_bugs.RemoveAll(item => item==null);
		
	}

	private IEnumerator LeafRegrowCheck()
	{
		while(true)
		{
			yield return new WaitForSeconds(Constants.TREE_NUTRIENT_CHECK_TIMER);
			if(_resources[typeof(Leaf)].Count < Constants.LEAF_COUNT && _inSun && getResourceNum(typeof(Nutrient)) > 0)
			{
				makeResource<Leaf>();
				removeResource<Nutrient>();
			}
		}
	}

	public Dictionary<Type, int> getResourceNum()
	{
		Dictionary<Type, int> countDict = new Dictionary<Type, int>();
		foreach(Type t in _resources.Keys)
			countDict.Add(t, _resources[t].Count);
		return countDict;
	}

	public int getResourceNum(Type t)
	{
		return _resources[t].Count;
	}

	public void makeResource<T>() where T : Resource
	{
		if(_isDead)
			return;
		T newResource = Instantiate(_resourcePrefabs[typeof(T).ToString()], gameObject.transform).GetComponent<T>();
		newResource.transform.position += Utility.offset(.1f,1.5f,.1f,1.5f);
		newResource.make();
		_resources[typeof(T)].Enqueue(newResource);
	}

	public void removeResource<T> () where T : Resource
	{
		T toRemove = (T) _resources[typeof(T)].Dequeue();
		toRemove.destroy();
		if(typeof(T) == typeof(Leaf))
		{
			_treeSounds.clip = _leafDie;
			_treeSounds.Play();
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag(Tags.SUN))
		{
			_inSun = true;
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if(other.gameObject.CompareTag(Tags.SUN))
		{
			_inSun = false;
		}
	}
	public void addColor(bugColor color)
	{
		_activeColor.Add(color);
		updateAttacks();
	}

	public void removeColor(bugColor color)
	{
		_activeColor.Remove(color);
		updateAttacks();
	}

	

	public void bugLanded(Bug bug)
	{
		_bugs.Add(bug);
		updateAttacks();
	}


	private void updateAttacks()
	{
		foreach(Bug bug in _bugs)
		{
			bug._isAttacked = _activeColor.Contains(bug._color);
		}
	}


	private void updateLeafSprite()
	{
		_leafSprite.sprite = _allTreeSpriteInfo.getSprite(_color, _resources[typeof(Leaf)].Count);
	}

	public void die()
	{
		foreach(Bug bug in _bugs)
			bug.killBug(false);
		_isDead = true;
		GetComponent<Animator>().SetBool("isDead", true);
		_treeSounds.clip = _treeDie;
		_treeSounds.Play();
		StopCoroutine(LeafRegrowCheck());
		for(int i = 0; i < getResourceNum(typeof(Nutrient)); i++)
			removeResource<Nutrient>();
	}
}

