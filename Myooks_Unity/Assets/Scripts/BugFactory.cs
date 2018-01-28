﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugFactory : MonoBehaviour {

	[SerializeField]
	private List<GreenTree> _treesStart;
	private static List<GreenTree> _trees;
	[SerializeField]
	private TreeSpriteInfo _bugSprites;

	private float _bugTimer = 0;
	private float _gameTimer = 0;
	public int _bugsPerDrop = 2;
	public GameObject _bugPrefab;
	private static Dictionary<bugColor, List<Bug>> _staticBugDict;

	void Start () 
	{
		foreach(GreenTree tree in _treesStart)
			addTreeToList(tree);
		beginBugDrop();
		Mycelium.OnDestroyBug += killBug;
<<<<<<< HEAD
=======
		//Mycelium.OnDestroyBugColor += killAllBugsOfColor;
>>>>>>> 89769ca71ffd52b4544b5ad676bb976d35ac16e1
	}
	
	private static void addTreeToList(GreenTree tree)
	{
		if(_trees == null)
			_trees = new List<GreenTree>();
		_trees.Add(tree);
	}

	private static void addToStaticBugList(Bug bug, bugColor color)
	{
		if(_staticBugDict == null)
			_staticBugDict = new Dictionary<bugColor, List<Bug>>();
		if(!_staticBugDict.ContainsKey(color))
			_staticBugDict.Add(color, new List<Bug>());
		_staticBugDict[color].Add(bug);
	}

	void Update () 
	{
		_gameTimer += Time.deltaTime;
		_bugTimer += Time.deltaTime;
		if(_bugTimer >= Constants.BUG_SPAWN_TIME)
		{
			beginBugDrop();
			_bugTimer = 0;
		}
		if(_trees.Count <= 2)
			UnityEngine.SceneManagement.SceneManager.LoadScene("gameOver");
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
<<<<<<< HEAD
		addToStaticBugList(bug, newBugColor);
		printDict();
=======
		_bugDict[newBugColor].Add(bug);
>>>>>>> 89769ca71ffd52b4544b5ad676bb976d35ac16e1
		bug.Initialize(selectTreeForDrop(bug, newBugColor, 0), newBugColor, 
				_bugSprites.getAnimator(newBugColor), _bugSprites.getTreeSprite(newBugColor));
	}

	private static void printDict()
	{
		foreach(bugColor color in _staticBugDict.Keys)
			Debug.Log(color.ToString() + ":" + _staticBugDict[color].Count);
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

	public static void killAllBugsOfColor(bugColor color, bool isBomb = false)
	{
        List<Bug> bugsToKill = _staticBugDict[color];
		List<Bug> tmpList = new List<Bug>(bugsToKill);
        for(int i=0; i < bugsToKill.Count; i++)
		{
			_staticBugDict[color].Remove(bugsToKill[i]);
            tmpList[i].killBug(isBomb);
		}
	}

	private void killBug(Bug bug, bool isBomb)
	{
		bug.killBug(isBomb);
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

	public static void deadTree(GreenTree tree)
	{
		_trees.Remove(tree);
		tree.die();
	}

}
