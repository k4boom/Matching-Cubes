using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public TrailRenderer tr;
    private float initialWidth;
    public bool isTrailer;
    // Start is called before the first frame update
    void Start()
    {
        isTrailer = true;
        initialWidth = tr.startWidth;
    }

    //Since Trail is mostly used, handled it more basic
    void Update() {
        if(isTrailer) {
            tr.startWidth =initialWidth * 1.2f;
        } else {
            tr.startWidth =initialWidth * 0; //if no pick, do not leave trail
        }
    }
}
