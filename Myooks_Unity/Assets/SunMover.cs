using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMover : MonoBehaviour {

    private Transform tf;
    public float sunSpeed;

	// Use this for initialization
	void Start () {
        tf = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        tf.position = Quaternion.Euler(0.0f,sunSpeed, 0.0f) * tf.position;
	}
}
