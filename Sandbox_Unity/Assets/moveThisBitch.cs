using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveThisBitch : MonoBehaviour {

    public float speed = 1.0F;
    public float jumpSpeed = 10.0F;
    public float gravity = 100.0F;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start () {
        Debug.Log("Starting");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?

        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);
        //var x = GetAxis("Horizontal");
        //transform.Translate(moveDirection * Time.deltaTime);
    }
}
