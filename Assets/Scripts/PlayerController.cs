using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float positionLimit;
    public float moveSpeed ;
    public float speed;
    public Rigidbody rb;
    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 3.0f;
        speed = 20.03f;

    }

    // Update is called once per frame
    void Update()
    {
        if(speed > 40.0f) { 
            speed = 40.03f;
        }
        //For positioning of stack, I used transform but for ramp, I needed rigidbody physics so
        //there is a switch between them for movement of player and picks
        if(StackManager.instance.isRamp) {        // Rigidbody
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
            
            rb.AddForce(Vector3.down * 10000);
            timeLeft -= Time.deltaTime;

        } else {                                //Transform
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0,0,0);
        }

        //HORIZONTAL MOVEMENT
        float moveX = Input.GetAxis("Mouse X");

        //Handled with MouseSwitch since not specified in the Case PDF
        if(Input.GetMouseButton(0)){
            transform.Translate(Vector3.right * moveX * moveSpeed * Time.deltaTime);
        }

        //Keep player on the platform
        if(transform.position.x < -positionLimit) {
           transform.position = new Vector3(-positionLimit,transform.position.y,transform.position.z);
        } else if(transform.position.x > positionLimit) {
           transform.position = new Vector3(positionLimit,transform.position.y,transform.position.z);
        }
        
    }

    public void MoveUp(){
        rb.AddForce(Vector3.up * 100.0f);
    }

    public void MoveDown(){
        rb.AddForce(Vector3.down * 100000);
    }

    public void FreeYConstraint() {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
