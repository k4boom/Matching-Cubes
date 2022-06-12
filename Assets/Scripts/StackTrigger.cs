using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackTrigger : MonoBehaviour
{
    //This script is included in the player object and stacked picks
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Pick") {                        
            if(StackManager.instance){
                StackManager.instance.PickUp(other.gameObject);
                }
        } else if(other.tag == "Obstacle") { 
            StackManager.instance.OnObstacleHit(transform);
        } else if(other.tag == "Order Gate") {
            StackManager.instance.OrderPicks();
        } else if(other.tag == "Random Gate") {
            StackManager.instance.ShufflePicks();
        } else if(other.tag == "Speedboost") {
            StackManager.instance.SpeedBoost();
        } else if(other.tag == "Ramp") {
            StackManager.instance.ChangeRampState();
        } else if(other.tag == "AfterJump" && (gameObject.tag == "Player" || transform.parent.tag == "Stacker")) {
            StackManager.instance.EndRampState();
        } else if(other.tag == "Finish") {
            UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();
        } 
    }
}
