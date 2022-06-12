using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = (new Vector3(player.position.x+3.0f, player.position.y + 5.0f, player.position.z - 17.0f));
    }
}
