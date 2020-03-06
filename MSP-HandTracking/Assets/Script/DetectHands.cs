using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHands : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.name+ " collision");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name + " trigger");
    }
}
