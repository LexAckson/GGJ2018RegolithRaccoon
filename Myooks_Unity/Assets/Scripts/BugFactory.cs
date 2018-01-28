using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugFactory : MonoBehaviour {

	[SerializeField]
	private List<GreenTree> _trees;

	private float _bugTimer = 0;
	public int _bugsPerDrop = 2;
	public GameObject _bugPrefab;
	private Dictionary<bugColor, List<Bug>> _bugDict;

	void Start () 
	{
		beginBugDrop();
		Mycelium.OnDestroyBug += killBug;
		Mycelium.OnDestroyBugColor += killAllBugsOfColor;
	}
	
	void Update () 
	{
		_bugTimer += Time.deltaTime;
		if(_bugTimer >= Constants.BUG_SPAWN_TIME)
		{
			beginBugDrop();
			_bugTimer = 0;
		}
	}

	private void beginBugDrop()
	{
		for(int i = 0; i < _bugsPerDrop; i++)
			bugDrop();
	}
	private void bugDrop()
	{
		Bug bug = Instantiate(_bugPrefab).GetComponent<Bug>();
		bugColor newBugColor = Utility.getRandomEnum<bugColor>();
		bug.setColor(newBugColor);
		_bugDict[newBugColor].Add(bug);
		bug.setTargetTree(selectTreeForDrop(bug));
	}

	private GreenTree selectTreeForDrop(Bug toDrop)
	{
		GreenTree selectedTree = Utility.RandomValue<GreenTree>(_trees);
		if(selectedTree._color == toDrop._color)
		{
			return selectTreeForDrop(toDrop);
		}

		return selectedTree;
	}

	public void killBug(Bug toKill)
	{
		_bugDict[toKill._color].Remove(toKill);
		toKill.die();
	}

	public void killAllBugsOfColor(bugColor color)
	{
		foreach(Bug bug in _bugDict[color])
			killBug(bug);
	}

}
