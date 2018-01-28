using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafAnims : MonoBehaviour {

	[SerializeField]
	private leafSprites[] _leafSprites;

	private Dictionary<bugColor, Sprite[]> _sprites;
	void Awake () 
	{
		_sprites = new Dictionary<bugColor, Sprite[]>();
		foreach(leafSprites spritePair in _leafSprites)
			_sprites.Add(spritePair.color, spritePair.sprites);
	}
	
	public Sprite getSprite(bugColor color, int leafNum)
	{
		return _sprites[color][leafNum];
	}
}
