using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class StackManager : MonoBehaviour
{
    public static StackManager instance;  //Singleton to use StackManager from everywhere
    [SerializeField] private Transform parent;
    [SerializeField] private Transform cylinder;
    [SerializeField] private Transform trailer;
    private List<Transform> picks;  // Picks List to handle positioning and gates
    private Vector3 firstPosition;
    private int comboCounter; // fever mode tracker
    private Rigidbody rb; //Player Object (Cylinder)
    private Transform prev;
    public bool isRamp;
    private bool rampStateEnded;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start()
    {
        rampStateEnded = true;
        isRamp = false;
        rb = cylinder.gameObject.GetComponent<PlayerController>().rb;
        comboCounter = 0;
        picks = new List<Transform>();
        firstPosition = new Vector3(cylinder.position.x, cylinder.position.y, cylinder.position.z);
    }

    
    void Update()
    {
        if(!isRamp) {
            rb.isKinematic = true;   //Transform Mode
            Regulator();
        } else {
            rb.isKinematic = false;  //Rigidbody Mode
            cylinder.gameObject.GetComponent<PlayerController>().FreeYConstraint();
            foreach (Transform child in picks)
            {
                child.gameObject.GetComponent<Pick>().ChangeRampState();
            }
            RampState();
        }
    }

    //Turn back to Normal State and Enable Trailer
    public void EndRampState() {
        rampStateEnded = true;
        trailer.GetComponent<TrailController>().isTrailer = true;
    }

    
    async void RampState() {
        await Task.Delay(2000);
        if(rampStateEnded) {
            isRamp = false;
            foreach (Transform child in picks)
                {
                    child.gameObject.GetComponent<Pick>().ChangeNormalState();
                }
        }
    }

    //Switch to Ramp State and Disable Trailer
    public void ChangeRampState() {
        isRamp = true;
        rampStateEnded = false;
        trailer.GetComponent<TrailController>().isTrailer = false;
    }

    //Matching Checker Function
    void CheckChildren() {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in picks)
        {
            children.Add(child);
        }
        bool again = false;
        for(int i = 0; i < children.Count - 2; i++) {
            if (children[i].isSameMaterial2(children[i+1], children[i+2])) {
                //await Task.Delay(500); // Delay to not disappear instantly matching blocks after Order Gate : Comes in with bugs
                comboCounter++;
                Destroy(children[i].gameObject);
                Destroy(children[i+1].gameObject);
                Destroy(children[i+2].gameObject);
                picks.Remove(children[i]);
                picks.Remove(children[i+1]);
                picks.Remove(children[i+2]);
                again = true;
                break;
            };
        }
        if(again) {
            CheckChildren();
        }
        if(comboCounter == 3) {
            SpeedBoost(); //fever mode
            comboCounter = 0;
        }
    }

    //Add Pick to the List and Invoke Regulator to position immediately
    public void PickUp(GameObject pickedObj){
        pickedObj.tag = "Picked";
        pickedObj.transform.parent = parent;
        picks.Add(pickedObj.transform);
        Regulator();
    }


    // Regulator handles all positioning constantly with Transform
    private void Regulator(){

        //Position of cylinder
        //set y value based on the # of children objects
        /**
        *hold the first position of cylinder
        *make calculation by referencing it
        *reference + localScale.y + 0.1f:
        */
        Vector3 newPos = new Vector3(cylinder.position.x, firstPosition.y, cylinder.position.z);
        foreach (Transform child in picks)
        {
            newPos.y += child.localScale.y + 0.1f;
        }       
        cylinder.position = newPos;      

        //Position of children
        /**
        *For each child
        *   cylinder-0.1f-pick-0.1f-pick-...
        */
        prev = cylinder;
        for(int i = 0; i < picks.Count; i++)
        {
            if(i==0){
                picks[i].position = new Vector3(prev.position.x, prev.position.y-1.2f, prev.position.z); 
            } else {
                picks[i].position = new Vector3(prev.position.x, prev.position.y-prev.localScale.y -0.1f, prev.position.z); 
            }
            prev = picks[i];
        }

        //Position of trailer object
        if(picks.Count>0) {
            trailer.position = new Vector3(prev.position.x, prev.position.y-0.2f, prev.position.z);             //relocate the trailer object under last pick
            trailer.GetComponent<TrailRenderer>().material = prev.GetComponent<MeshRenderer>().material;        //change the color of the trail
        }
        //Check picks for trailer
        if(picks.Count == 0){
            trailer.GetComponent<TrailController>().isTrailer = false;
        } else {
            trailer.GetComponent<TrailController>().isTrailer = true;
        }

        CheckChildren();        //check for the 3-conjugate combo
    }

    
    //Order Gate
    public void OrderPicks() {
            picks.Sort((x, y) => string.Compare(x.GetComponent<MeshRenderer>().sharedMaterial.name, y.GetComponent<MeshRenderer>().sharedMaterial.name));
        
    }
    
    //Random Gate
    public void ShufflePicks() {
            picks = picks.Fisher_Yates_CardDeck_Shuffle();
        
    }

    //Since we are in Transform Mode, I handle the obstacle hit a bit bad
    //May be done better in a more available timeline :/
    async public void OnObstacleHit(Transform pickToDestroy) {
        if(picks.Count == 0) {
            UnityEditor.EditorApplication.isPlaying = false;
        } else {
            pickToDestroy.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; 
            pickToDestroy.parent = null;
            await Task.Delay(200);
            picks.Remove(pickToDestroy);
        }      
    }

    //Used in Fever Mode and Speed Boost
    public void SpeedBoost() {
        cylinder.gameObject.GetComponent<PlayerController>().speed *= 2;
    }
    
}

