using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetDesk : MonoBehaviour
{
    private Vector3 initialPosW1,initialPosW2,initialPosW3;
    private Quaternion initialRotXW1;//,initialRotYW1,initialRotZW1;//word1 rotation
    private Quaternion initialRotXW2;//,initialRotYW2,initialRotZW2;//word2 rotation
    private Quaternion initialRotXW3;//,initialRotYW3,initialRotZW3;//word3 rotation
   
    private Quaternion W1Rot, W2Rot, W3Rot;
    public GameObject word1, word2, word3;
    // Start is called before the first frame update
    void Start()
    {
       
        initialRotXW1 = word1.transform.localRotation;
        //initialRotYW1 = word1.transform.localRotation.y;
        //initialRotZW1 = word1.transform.localRotation.z;

        initialRotXW2 = word2.transform.localRotation;
       // initialRotYW2 = word2.transform.localRotation.y;
       // initialRotZW2 = word2.transform.localRotation.z;

        initialRotXW3 = word3.transform.localRotation;
        // initialRotYW3 = word3.transform.localRotation.y;
        // initialRotZW3 = word3.transform.localRotation.z;
        Debug.Log("Word1 " + initialRotXW1 + " word2 " + initialRotXW2 + " word3" + initialRotXW3);
        initialPosW1 = word1.transform.position;
        initialPosW2 = word2.transform.position;
        initialPosW3 = word3.transform.position;
    }

    public void ResetPositionAndRotation()
    {
        word1.transform.position = initialPosW1;
        word2.transform.position = initialPosW2;
        word3.transform.position = initialPosW3;

        word1.transform.rotation = Quaternion.identity;
        word2.transform.rotation = Quaternion.identity;
        word3.transform.rotation = Quaternion.identity;

        word1.transform.localRotation = initialRotXW1;
       word2.transform.localRotation=initialRotXW2;
       word3.transform.localRotation=initialRotXW3;
    }
 
    
}
