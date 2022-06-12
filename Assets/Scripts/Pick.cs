using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float speed;
    public GameObject cylinder;
    private float timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 3.0f;
        speed = cylinder.GetComponent<PlayerController>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        //For positioning of stack, I used transform but for ramp, I needed rigidbody physics so
        //there is a switch between them for movement of player and picks
        if(StackManager.instance.isRamp) {
            if(rb) {
                if(timeLeft>1.2f) {
                    rb.velocity = Vector3.up * 10 + Vector3.forward * speed;
                    transform.rotation = Quaternion.Euler(-30,0,0);
                } else if(timeLeft>0.8f && timeLeft<1.2f) {
                    rb.velocity = Vector3.forward * speed;
                    transform.rotation = Quaternion.Euler(0,0,0);

                } else if(timeLeft<0.8f) {
                    rb.velocity = Vector3.down * 10 + Vector3.forward * speed;
                    transform.rotation = Quaternion.Euler(30,0,0);

                } 
                timeLeft -= Time.deltaTime;
                rb.AddForce(Vector3.down * 10000);

            }
            
        } else {    //Regulator handles movement, only need to change rotation here
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    //Get Into Ramp (Rigidbody) State
    public void ChangeRampState(){
        if(rb){
            rb.isKinematic = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }

    //Get Into Normal (Transform) State
    public void ChangeNormalState(){
        if(rb){
            rb.isKinematic = true;
            gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }
}
