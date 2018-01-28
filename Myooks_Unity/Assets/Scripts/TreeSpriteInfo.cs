using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpriteInfo : MonoBehaviour {

	[SerializeField]
	private leafSprites[] _leafSprites;
	[SerializeField]
	private treeAnims[] _treeAnimators;

	private Dictionary<bugColor, Sprite[]> _sprites;
	private Dictionary<bugColor, Animator> _anims;
	void Awake () 
	{
		_sprites = new Dictionary<bugColor, Sprite[]>();
		_anims = new Dictionary<bugColor, Animator>();
		foreach(leafSprites spritePair in _leafSprites)
			_sprites.Add(spritePair.color, spritePair.sprites);
		foreach(treeAnims tree in _treeAnimators)
			_anims.Add(tree.color, tree.animator);
	}
	
	public Sprite getSprite(bugColor color, int leafNum)
	{
		return _sprites[color][leafNum];
	}

	public Animator getAnimator(bugColor color)
	{
		return _anims[color];
	}
}
