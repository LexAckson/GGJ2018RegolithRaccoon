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
	private GameObject _nutrientPrefab;
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
			makeNutrient();
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
				//Nutrient toAbsorb = _nutrients.Dequeue();
				_leaves++;
				//absorbNutrient(toAbsorb.gameObject);
			}
		}
	}

	private void absorbNutrient(GameObject nutrient)
	{
		Vector3 scale = nutrient.transform.localScale;
		StartCoroutine(Utility.scaler(nutrient, scale.x, 0, Constants.NUTRIENT_ABSORB_TIME, 
					() => Destroy(nutrient)));
		
	}

	void makeNutrient()
	{
		Nutrient nutrient = Instantiate(_nutrientPrefab, transform).GetComponent<Nutrient>();
		float startScale = nutrient.gameObject.transform.localScale.x;
		StartCoroutine(Utility.scaler(nutrient.gameObject, startScale, Constants.NUTRIENT_SIZE, Constants.NUTRIENT_MAKE_TIMER,
			() => _resources[typeof(Nutrient)].Enqueue(nutrient)));
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
