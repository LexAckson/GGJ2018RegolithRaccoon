using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GreenTree : MonoBehaviour {

	[SerializeField]
	private GameObject _leafPrefab;
	public int _leaves;
	
	public int _toxins;
	[SerializeField]
	private Dictionary<string, GameObject> _resourcePrefabs;
	public Dictionary<Type , Queue<Resource>> _resources;

	private bool _inSun;
	private float _sunTimer;

	void Start () 
	{
		_resources = new Dictionary<Type , Queue<Resource>>();
		StartCoroutine(LeafRegrowCheck());
	}
	
	void Update () 
	{
		if(_inSun)
		{
			_sunTimer += Time.deltaTime;
		}

		if(_sunTimer >= Constants.NUTRIENT_MAKE_TIMER)
		{
			_sunTimer = 0;
			makeResource<Nutrient>();
		}
	}

	private IEnumerator LeafRegrowCheck()
	{
		while(true)
		{
			yield return new WaitForSeconds(Constants.TREE_NUTRIENT_CHECK_TIMER);
			if(_leaves < Constants.LEAF_COUNT)
			{
				// TODO: do regrow animation
				_leaves++;
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

	public T TakeResource<T>() where T : Resource
	{
		return _resources[typeof(T)].Dequeue() as T;
	}

	public void GiveResource(Resource newResource)
	{
		_resources[newResource.GetType()].Enqueue(newResource);
	}

	private void makeResource<T>() where T : Resource
	{
		T newResource = Instantiate(_resourcePrefabs[typeof(T).ToString()]).GetComponent<T>();
		newResource.make();
		_resources[typeof(T)].Enqueue(newResource);
	}

	private void removeResource<T> () where T : Resource
	{
		T toRemove = (T) _resources[typeof(T)].Dequeue();
		toRemove.destroy();
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
}
