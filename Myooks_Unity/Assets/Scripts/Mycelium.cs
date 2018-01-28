﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Mycelium : MonoBehaviour {

    public GameObject myceliumParticle, startObject, endObject;
    public Color myceliumColor;
    private LineRenderer lineRenderer;
    private int numberOfParticles = 0; //Total number of points in mycelium.
    private float startWidth = 0.1F;
    public List<GameObject> myceliumDots;
    public string mycelium_layer;
    private bool alive = true;

    //destroy bugs events
    public delegate void DestroyBugAction(Bug bugToDestroy);
    public static event DestroyBugAction OnDestroyBug;

    public delegate void DestroyBugColorAction(bugColor bugColorToDestory);
    public static event DestroyBugColorAction OnDestroyBugColor;


    public void Init(GameObject start, GameObject end)
    {
        startObject = start;
        endObject = end;
    }

    // Use this for initialization
    void Start() {
        //setup our line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = myceliumColor;
        lineRenderer.endColor = myceliumColor;
        lineRenderer.startWidth = startWidth;
        //one more line position so we can connect the ends
        lineRenderer.positionCount = numberOfParticles + 2;


        ////spawn our surf colliders
        ////make the original surf collider so we can link back the surfaceTensioner
        //myceliumDots.Add(Instantiate(myceliumParticle, startObject.transform.position, Quaternion.identity));
        //myceliumDots[0].layer = LayerMask.NameToLayer(mycelium_layer);
        //myceliumDots[0].transform.parent = gameObject.transform;

        //for (int i = 1; i < numberOfParticles; i++)
        //{
        //    //we want to place our piece 1/(numOfPart + 1) of the way from start to end
        //    Vector3 pos = i * (startObject.transform.position - endObject.transform.position) / ((numberOfParticles + 1) * 1.0f);
        //    myceliumDots.Add(Instantiate(myceliumParticle, pos, Quaternion.identity));
        //    myceliumDots[i].GetComponent<Thread>().surfaceTensioner.connectedBody = myceliumDots[i - 1].GetComponent<Rigidbody>();
        //    myceliumDots[i].layer = LayerMask.NameToLayer(mycelium_layer);
        //    myceliumDots[i].transform.parent = gameObject.transform;
        //}
        ////link the first and last surfaceTensioners to the start/end objects
        //myceliumDots[0].GetComponent<Thread>().surfaceTensioner.connectedBody = startObject.GetComponent<Rigidbody>();
        //endObject.GetComponent<Thread>().surfaceTensioner.connectedBody = myceliumDots[numberOfParticles].GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            //draw the line
            DrawMycelium();
            //only balance if the player is done setting the line
            if (!endObject.CompareTag("Player"))
            {
                //balance resources
                BalanceResources();

                //handle bug link interactions
                BalanceBugs();
            }
        }
        else {
            //maybe die?
            CleanUpYourOwnCorpse();
        }
    }

    private void DrawMycelium()
    {
        //start the line
        lineRenderer.SetPosition(0, startObject.GetComponent<Rigidbody>().position);
        //line position 1 is our first dot
        for (int i = 1; i < numberOfParticles + 1; i++)
        {
            //draw our surface line
            lineRenderer.SetPosition(i, myceliumDots[i].GetComponent<Rigidbody>().position);
        }
        //connect the last dot to the end
        lineRenderer.SetPosition(numberOfParticles + 1, endObject.GetComponent<Rigidbody>().position);
    }

    public void GrowMycelium()
    {
        //add a link
    }

    public void PinTheEnd(GameObject end)
    {
        endObject = end;
        //TODO fixup the links
    }
    //balances resources between the two trees
    private void BalanceResources()
    {
        //Only trade if the both are a greenTree
        GreenTree gtStart = startObject.GetComponent<GreenTree>();
        GreenTree gtEnd = endObject.GetComponent<GreenTree>();

        if (gtEnd && gtStart)
        {
            Dictionary<Type, int> gtStartResourceDict = gtStart.getResourceNum();
            Dictionary<Type, int> gtEndResourceDict = gtEnd.getResourceNum();
            int nutrientDiff = gtStartResourceDict[typeof(Nutrient)] - gtEndResourceDict[typeof(Nutrient)];
            if (nutrientDiff >= 2)
            {
                Nutrient nut = gtStart.TakeResource<Nutrient>();
                gtEnd.GiveResource(nut);
            } else if (nutrientDiff <= -2)
            {
                Nutrient nut = gtEnd.TakeResource<Nutrient>();
                gtStart.GiveResource(nut);
            }

        }

    }

    //join 2 bugs for bomb
    //matched bug + tree for leaf
    //not matched bug + tree for defense
    private void BalanceBugs()
    {
        Bug startBug = startObject.GetComponent<Bug>();
        Bug endBug = endObject.GetComponent<Bug>();
        if (startBug || endBug) {
            //whatever happens from this point on,
            //we will be marking this mycelium for death
            MarkForDeath();
            //join 2 bugs for bomb
            if (startBug && endBug && startBug._color == endBug._color)
            {
                //TODO Bug Bomb!
                Debug.Log("Bug BOMB!");
                OnDestroyBugColor(startBug._color);
            }
            //check for trees
            GreenTree myTree = startObject.GetComponent<GreenTree>() != null ? startObject.GetComponent<GreenTree>() : endObject.GetComponent<GreenTree>();
            Bug myBug = startBug != null ? startBug : endBug;
            if (myTree != null)
            {
                //matched bug + tree for leaf
                if (myTree._color == myBug._color)
                {
                    //TODO tell tree to grow leaf
                    Debug.Log("Telling a tree to grow a leaf");
                } //not matched bug + tree for defense
                else {
                    //TODO defend tree
                    Debug.Log("Defend a tree");
                }
                //we will be destroying the bug
                OnDestroyBug(myBug);
            }

        }
    }

    //this will make it destroy iself after all pending things are done
    public void MarkForDeath()
    {
        alive = false;
    }

    private void CleanUpYourOwnCorpse()
    {
        //TODO maybe some fade away?

        //destroy all the bits
        foreach (GameObject go in myceliumDots)
        {
            Destroy(go);
        }
        Destroy(lineRenderer);
        Destroy(this);
    }

}
