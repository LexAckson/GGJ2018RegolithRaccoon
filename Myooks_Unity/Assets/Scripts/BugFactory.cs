﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugFactory : MonoBehaviour {

	[SerializeField]
	private static List<GreenTree> _trees;
	[SerializeField]
	private TreeSpriteInfo _bugSprites;

	private float _bugTimer = 0;
	private float _gameTimer = 0;
	public int _bugsPerDrop = 2;
	public GameObject _bugPrefab;
	private static Dictionary<bugColor, List<Bug>> _bugDict;

	void Start () 
	{
		_bugDict = new Dictionary<bugColor, List<Bug>>();
		foreach(bugColor color in Utility.GetValues<bugColor>())
			_bugDict.Add(color, new List<Bug>());
		beginBugDrop();
		Mycelium.OnDestroyBug += killBug;
		//Mycelium.OnDestroyBugColor += killAllBugsOfColor;
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
		_bugDict[newBugColor].Add(bug);
		bug.Initialize(selectTreeForDrop(bug, newBugColor, 0), newBugColor, 
				_bugSprites.getAnimator(newBugColor), _bugSprites.getTreeSprite(newBugColor));
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

	public static void killBug(Bug toKill, bool isBomb = false)
	{
		_bugDict[toKill._color].Remove(toKill);
		toKill.killBug(isBomb);
	}

	public static void killAllBugsOfColor(bugColor color, bool isBomb = false)
	{
        List<Bug> bugsToKill = new List<Bug>();
        foreach (Bug bug in _bugDict[color])
			bugsToKill.Add(bug);
        foreach (Bug bug in bugsToKill)
        {
            killBug(bug, isBomb);
        }
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
