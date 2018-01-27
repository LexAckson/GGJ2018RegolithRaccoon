using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerLogic : MonoBehaviour {

    public Vector3 myPos;
    public Transform myPlay;
    public Camera mainCamera;
    public float yAxis = 0.0F;
    public float ySpeed = 2.0F;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = myPlay.position + myPos;
        yAxis -= ySpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(yAxis, 0, 0);

    }
}
