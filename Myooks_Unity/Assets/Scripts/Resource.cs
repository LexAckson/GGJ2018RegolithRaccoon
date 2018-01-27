using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour {

	public string _name;
	void Start () {
		
	}
	
	public abstract void make();

	public abstract void destroy();
}
