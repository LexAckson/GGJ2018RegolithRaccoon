using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpriteInfo : MonoBehaviour {

	[SerializeField]
	private leafSprites[] _leafSprites;
	[SerializeField]
	private treeAnims[] _treeAnimators;
	[SerializeField]
	public colorSpritePair[] _treeSprites;

	private Dictionary<bugColor, Sprite[]> _sprites;
	private Dictionary<bugColor, RuntimeAnimatorController> _anims;
	private Dictionary<bugColor, Sprite> _trees;
	private bool _isInit;
	void Awake () 
	{
		_sprites = new Dictionary<bugColor, Sprite[]>();
		_anims = new Dictionary<bugColor, RuntimeAnimatorController>();
		_trees = new Dictionary<bugColor, Sprite>();
		foreach(leafSprites spritePair in _leafSprites)
			_sprites.Add(spritePair.color, spritePair.sprites);
		foreach(treeAnims tree in _treeAnimators)
			_anims.Add(tree.color, tree.animator);
		foreach(colorSpritePair tree in _treeSprites)
			_trees.Add(tree.color, tree.sprite);
		_isInit = true;
	}
	
	public Sprite getSprite(bugColor color, int leafNum)
	{
		return _sprites[color][leafNum];
	}

	public RuntimeAnimatorController getAnimator(bugColor color)
	{
		return _anims[color];
	}

	public Sprite getTreeSprite(bugColor color)
	{
		return _trees[color];
	}

	public bool isInit()
	{
		return _isInit;
	}
}
