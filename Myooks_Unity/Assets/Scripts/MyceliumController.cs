using System.Collections;
using System.Collections.Generic;
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

    private void Init(GameObject start, GameObject end)
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
        //    myceliumDots[i].GetComponent<Thread>().surfaceTensioner.connectedBody = myceliumDots[i - 1].GetComponent<Rigidbody2D>();
        //    myceliumDots[i].layer = LayerMask.NameToLayer(mycelium_layer);
        //    myceliumDots[i].transform.parent = gameObject.transform;
        //}
        ////link the first and last surfaceTensioners to the start/end objects
        //myceliumDots[0].GetComponent<Thread>().surfaceTensioner.connectedBody = startObject.GetComponent<Rigidbody2D>();
        //endObject.GetComponent<Thread>().surfaceTensioner.connectedBody = myceliumDots[numberOfParticles].GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //draw the line
        DrawMycelium();

        //maybe die?

        //balance resources
        BalanceResources();
    }

    private void DrawMycelium()
    {
        //start the line
        lineRenderer.SetPosition(0, startObject.GetComponent<Rigidbody2D>().position);
        //line position 1 is our first dot
        for (int i = 1; i < numberOfParticles + 1; i++)
        {
            //draw our surface line
            lineRenderer.SetPosition(i, myceliumDots[i].GetComponent<Rigidbody2D>().position);
        }
        //connect the last dot to the end
        lineRenderer.SetPosition(numberOfParticles + 1, endObject.GetComponent<Rigidbody2D>().position);
    }

    public void GrowMycelium()
    {
        //add a link
    }

    public void PinTheEnd(GameObject end)
    {
        endObject = end;
        //fixup the links
    }
    //balances resources between the two trees
    private void BalanceResources()
    {
        //Only trade if the end is a tree
        GreenTree gtEnd = end.GetComponent<GreenTree>();

        if (gtEnd != null)
        {
            GreenTree gtStart = start.GetComponent<GreenTree>();
            Dictionary<Types, int> gtStartResourceDict = gtStart.getResourceNum();
            Dictionary<Types, int> gtEndResourceDict = gtEnd.getResourceNum();
            int nutrientDiff = gtStartResourceDict["Nutrients"] - gtEndResourceDict["Nutrients"];
            if (nutrientDiff >= 2)
            {
                gtStart.TakeResource("Nutrients");
                gtEnd.GiveResource("Nutrients");
            } else if (nutrientDiff <= -2)
            {
                gtEnd.TakeResource("Nutrients");
                gtStart.GiveResource("Nutrients");
            }
        }

    }

}
