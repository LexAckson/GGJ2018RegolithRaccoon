using System.Collections;
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
			UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.GAME_OVER_SCREEN);
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
		addToStaticBugList(bug, newBugColor);
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

    public IEnumerator Blink(Color _blinkIn, int _blinkCount, float _totalBlinkDuration)
    {
        // We divide the whole duration for the ammount of blinks we will perform
        float fractionalBlinkDuration = _totalBlinkDuration / _blinkCount;

        for (int blinked = 0; blinked < _blinkCount; blinked++)
        {
            // Each blink needs 2 lerps: we give each lerp half of the duration allocated for 1 blink
            float halfFractionalDuration = fractionalBlinkDuration * 0.5f;

            // Lerp to the color
            yield return StartCoroutine(ColorLerpTo(_blinkIn, halfFractionalDuration));

            // Lerp to transparent
            StartCoroutine(ColorLerpTo(Color.clear, halfFractionalDuration));
        }
    }

    public static void killAllBugsOfColor(bugColor color, bool isBomb = false)
	{
        List<Bug> bugsToKill = new List<Bug>(_staticBugDict[color]);
		_staticBugDict[color] = new List<Bug>();
        for(int i=0; i < bugsToKill.Count; i++)
		{
			Bug b = bugsToKill[i];
            b.killBug(isBomb);
		}
        //TODO flash the dang screen in color
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
