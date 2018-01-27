using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerLogic : MonoBehaviour {

    public Vector3 myPos;
    public Transform myPlay;
    public Camera mainCamera;
    public float xAxis = 0.0F;
    public float yAxis = 0.0F;
    public float xSpeed = 2.0F;
    public float ySpeed = 2.0F;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = myPlay.position + myPos;

        xAxis += xSpeed * Input.GetAxis("Mouse X");
        yAxis -= ySpeed * Input.GetAxis("Mouse Y");
        //mainCamera.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 1.0F);
        transform.eulerAngles = new Vector3(yAxis, xAxis, 0);
        Debug.Log(Input.GetAxis("Mouse Y"));
        Debug.Log(Input.GetAxis("Mouse X"));
    }
}
