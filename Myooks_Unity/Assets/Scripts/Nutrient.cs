using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutrient : Resource {

	// Use this for initialization
	void Start () {
		name = "nutrient";
		gameObject.tag = Tags.NUTRIENT;
	}
	
	public override void make()
	{
		float startScale = transform.localScale.x;
		StartCoroutine(Utility.scaler(gameObject, startScale, Constants.NUTRIENT_SIZE, Constants.NUTRIENT_MAKE_TIMER,
			null));
	}

	public override void destroy()
	{
		float startScale = transform.localScale.x;
		StartCoroutine(Utility.scaler(gameObject, startScale, 0, Constants.NUTRIENT_ABSORB_TIME, 
					() => Destroy(gameObject)));
	}
}
