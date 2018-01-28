using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour {

	public bugColor _color;
	public GreenTree _targetTree;
	public float _landingTimer;
	public bool _isAttakced;
	public float _attackTimer;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void die()
	{

	}

	public void setColor(bugColor color)
	{
		_color = color;
	}

	public void setTargetTree(GreenTree tree)
	{
		_targetTree = tree;
	}
}
