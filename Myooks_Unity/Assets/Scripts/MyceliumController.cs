using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mycelium : MonoBehaviour {

    public GameObject myceliumParticle;
    public Color myceliumColor;
    public static float r = 3;
    private LineRenderer lineRenderer;
    public int numberOfParticles = 30; //Total number of points in mycelium.
    public float startWidth = 0.1F;
    public List<GameObject> myceliumDots;
    public string mycelium_layer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
