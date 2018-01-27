using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMover : MonoBehaviour {

    [SerializeField]
    private Transform tf;
    //private Transform ground;

    public float sunSpeed;

	// Use this for initialization
	void Start () {
        tf = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //tf.RotateAround(ground.position, Vector3.up, sunSpeed);
        tf.Rotate(Vector3.up, sunSpeed); 
        tf.position = Quaternion.Euler(0.0f,sunSpeed, 0.0f) * tf.position;
	}
}
