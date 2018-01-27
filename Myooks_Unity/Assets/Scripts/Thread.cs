using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread : MonoBehaviour
{

    public float stiffness = 0.025f;
    public float dampening = 0.025f;
    Rigidbody2D rb;
    public SpringJoint2D surfaceTensioner;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //maybe tell the MyceliumController when we are being pulled on so we can add a new link at this spot?
    }
}
