using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Mycelium : MonoBehaviour {

    public GameObject myceliumParticle, pinSprite, startObject, endObject;
    private GameObject startPin, endPin;
    public Color myceliumColor;
    private LineRenderer lineRenderer;
    private int numberOfParticles = 15; //Total number of points in mycelium.
    private float startWidth = 0.1F;
    public List<GameObject> myceliumDots;
    public string mycelium_layer;
    private int lifeForce = 255;
    private List<Flow> myFlows;
    private Gradient myGradient;
    private GradientColorKey[] myGCK;
    private GradientAlphaKey[] myGAK;

    //destroy bugs events
    public delegate void DestroyBugAction(Bug bugToDestroy, bool isBomb);
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

        myFlows = new List<Flow>();
        myGradient = new Gradient();
        myGCK = new GradientColorKey[8];
        myGAK = new GradientAlphaKey[2];

        //drop the pin
        startPin = Instantiate(pinSprite, endObject.transform.position, endObject.transform.rotation);
        startPin.GetComponent<SpriteRenderer>().flipX = UnityEngine.Random.value > 0.5f;

        //setup our line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = Color.clear;
        lineRenderer.endColor = Color.clear;
        lineRenderer.startWidth = startWidth;
        //one more line position so we can connect the ends
        lineRenderer.positionCount = numberOfParticles;

        //spawn our mycelium colliders
        Vector3 between = endObject.transform.position - startObject.transform.position;

        for (int i = 0; i < numberOfParticles; i++)
        {
            //we want to place our piece 1/(numOfPart + 1) of the way from start to end
            Vector3 pos = startObject.transform.position + (between *(float)i / (float)(numberOfParticles - 1));
            myceliumDots.Add(Instantiate(myceliumParticle, pos, Quaternion.identity));
            if (i > 0 )
                myceliumDots[i].GetComponent<SpringJoint>().connectedBody = myceliumDots[i - 1].GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (lifeForce == 255)
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
        else if (lifeForce > 0)
        {
            //fade out a bit
            lifeForce = lifeForce - 2;
            //draw the line
            DrawMycelium();
        } else
        { 
            //no bits left behind
            CleanUpYourOwnCorpse();
        }
    }

    private void DrawMycelium()
    {
        //reset the colors cause we can be fading or doing effects
        FillGradientKeys();

        //Make sure the first and last particle are stuck to the start/ end objects
        if (startObject != null)
            myceliumDots[0].transform.position = startObject.transform.position;
        if (endObject != null)
            myceliumDots[numberOfParticles - 1].transform.position = endObject.transform.position;

        //line position 1 is our first dot
        for (int i = 0; i < numberOfParticles; i++)
        {
            //draw our surface line
            lineRenderer.SetPosition(i, myceliumDots[i].transform.position);
            //toggle kinematic occasionally to kill velocity and make it look jittery
            myceliumDots[i].GetComponent<Rigidbody>().isKinematic = UnityEngine.Random.value > 0.99f;
        }

    }

    public void GrowMycelium()
    {
        //add a link
    }

    public void PinTheEnd(GameObject end)
    {
        //drop the pin
        endPin = Instantiate(pinSprite, endObject.transform.position, endObject.transform.rotation);
        endPin.GetComponent<SpriteRenderer>().flipX = UnityEngine.Random.value > 0.5f;
        endObject = end;
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
            int leafDiff = gtStartResourceDict[typeof(Leaf)] - gtEndResourceDict[typeof(Leaf)];
            if (leafDiff > 0 && gtStartResourceDict[typeof(Nutrient)] > 0)
            {
                //take a resource form the start tree
                gtStart.removeResource<Nutrient>();
                Debug.Log("FlowAction: Tree NUTRIENT give start!");
                AddFlow("forward", bugColor.YELLOW, endObject, FlowAction.NUTRIENT);
            } else if (leafDiff < 0 && gtEndResourceDict[typeof(Nutrient)] > 0)
            {
                //take a resource form the end tree
                gtEnd.removeResource<Nutrient>();
                Debug.Log("FlowAction: Tree NUTRIENT give start!");
                AddFlow("backward", bugColor.YELLOW, startObject, FlowAction.NUTRIENT);
            }

            bool hasDefense = false;
            foreach(Flow f in myFlows)
                if(f.fAction == FlowAction.DEFENSE)
                    hasDefense = true;
                    
            if(!hasDefense)
            {
                if(!gtEnd._activeColor.Contains(gtStart._color))
                    AddFlow("forward", gtStart._color, endObject, FlowAction.DEFENSE);
                if(!gtStart._activeColor.Contains(gtEnd._color))
                    AddFlow("backward", gtEnd._color, startObject, FlowAction.DEFENSE);
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
                //TODO Bug Bomb animation
                Debug.Log("FlowAction: Bug BOMB start!");
                AddFlow("forward", startBug._color, endObject, FlowAction.BOMB);
                AddFlow("backward", startBug._color, startObject, FlowAction.BOMB);
            }
            //check for trees
            GreenTree myTree = startObject.GetComponent<GreenTree>() != null ? startObject.GetComponent<GreenTree>() : endObject.GetComponent<GreenTree>();
            Bug myBug = startBug != null ? startBug : endBug;
            if (myTree != null)
            {
                //matched bug + tree for leaf
                if (myTree._color == myBug._color)
                {
                    Debug.Log("FlowAction: Tree LEAF grow start!");
                    if (startBug != null)
                    {
                        AddFlow("forward", myBug._color, endObject, FlowAction.LEAF);
                    }
                    else
                    {
                        AddFlow("backward", myBug._color, startObject, FlowAction.LEAF);
                    }
                } //not matched bug + tree for defense
                else {
                    Debug.Log("FlowAction: Tree DEF start!");
                    if (startBug != null) {
                        AddFlow("forward", myBug._color, endObject, FlowAction.DEFENSE);
                    } else {
                        AddFlow("backward", myBug._color, startObject, FlowAction.DEFENSE);
                    }

                }
                //we will be destroying the bug
                OnDestroyBug(myBug, true);
            }

        }
    }

    //this will make it destroy iself after all fading out
    public void MarkForDeath()
    {
        lifeForce = --lifeForce;
    }

    private void CleanUpYourOwnCorpse()
    {
        //destroy all the bits
        foreach (GameObject go in myceliumDots)
        {
            Destroy(go);
        }
        Destroy(lineRenderer);
        Destroy(gameObject);
        if (startPin != null)
            Destroy(startPin);
        if (endPin != null)
            Destroy(endPin);
    }

    private void FillGradientKeys()
    {
        //set alphas
        myGAK[0].alpha = myceliumColor.a;
        myGAK[0].time = 0f;
        myGAK[1].alpha = myceliumColor.a;
        myGAK[1].time = 1f;
        //set colors
        for (int i = 0; i < myGCK.Length; i++)
        {
            myGCK[i].color = myceliumColor;
            myGCK[i].time = (i * 1f)/myGCK.Length;
        }
        //now overlay the effect colors
        foreach (Flow f in myFlows)
        {
            float t = f.TakeAStep();
            int idx = (int)(Mathf.Clamp(Mathf.Round(t * myGCK.Length), 0, myGCK.Length - 1 ));
            myGCK[idx].color = f.color;
            myGCK[idx].time = t;
        }
        //TODO this is probably leaking memory?
        myFlows.RemoveAll(f => f.isDone);

        //save our changes
        myGradient.SetKeys(myGCK, myGAK);
        lineRenderer.colorGradient = myGradient;
    }

    private void AddFlow(string direction, bugColor c, GameObject target, FlowAction fAction)
    {
        Flow f = new Flow();
        f.Init(direction, c, target, fAction);
        myFlows.Add(f);
    }

}



//This class keeps track of gradient effects moving down the mycelium
class Flow {
    public float currentProgress = 0f;
    //const float DURATION = 2f;
    public float change = 1f;
    public bool isDone = false;
    public Color color;
    //this will dictate the final action
    public FlowAction fAction;
    public bugColor bugCol;
    public GameObject target;


    public void Init(string direction, bugColor c, GameObject t, FlowAction f)
    {
        target = t;
        bugCol = c;
        fAction = f;
        color = fAction == FlowAction.NUTRIENT ?  Color.green : Constants.BUG_COLOR_LOOKUP[c];
        if (direction != "forward")
        {
            change = -1f;
            currentProgress = 1f;
        }
    }

    //update the current step based on elapsed time, and return a float[0f to 1f] for where in the gradient to add a color
    public float TakeAStep() {
        //update progress
        //Time.deltaTime was going bonkers here, so just change at a fixed rate per frame
        currentProgress += 0.004f * change;
        //check to see if we should flag this flow as done
        isDone = change > 0 ? currentProgress > 1f : currentProgress < 0f;
        if(isDone)
            deliverAction();
        return currentProgress;
    }

    //based on color and GameObject type, do stuff
    private void deliverAction()
    {
        switch (fAction) {
            case FlowAction.BOMB:
                Debug.Log("FlowAction: Bug BOMB finish!");
                BugFactory.killAllBugsOfColor(target.GetComponent<Bug>()._color, true);
                break;
            case FlowAction.DEFENSE:
                target.GetComponent<GreenTree>().addColor(bugCol);
                Debug.Log("FlowAction: Tree DEF finish!");
                break;
            case FlowAction.LEAF:
                target.GetComponent<GreenTree>().makeResource<Leaf>();
                Debug.Log("FlowAction: Tree LEAF grow finish!");
                break;
            case FlowAction.NUTRIENT:
                target.GetComponent<GreenTree>().makeResource<Nutrient>();
                Debug.Log("FlowAction: Tree NUTRIENT give finish!");
                break;
            default:
                break;
        }
    }
}
