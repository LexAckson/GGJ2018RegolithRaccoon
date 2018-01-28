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
		_bugDict = new Dictionary<bugColor, List<Bug>>();
		foreach(bugColor color in Utility.GetValues<bugColor>())
			_bugDict.Add(color, new List<Bug>());
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
		bugColor newBugColor = getValidColor();
		_bugDict[newBugColor].Add(bug);
		bug.Initialize(selectTreeForDrop(bug, newBugColor, 0), newBugColor);
	}

	private GreenTree selectTreeForDrop(Bug toDrop, bugColor color, int recurseLvl)
	{
		GreenTree selectedTree = Utility.RandomValue<GreenTree>(_trees);
		if(selectedTree._color == color && recurseLvl <= 20)
		{
			return selectTreeForDrop(toDrop, color, ++recurseLvl);
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

	private bugColor getValidColor()
	{
		bugColor newBugColor = Utility.getRandomEnum<bugColor>();
		if(isBugColorValid(newBugColor))
			return newBugColor;
		return getValidColor();
	}

	private bool isBugColorValid(bugColor color)
	{
		foreach(GreenTree tree in _trees)
			if(tree._color == color)
				return true;
		return false;
	}

}
