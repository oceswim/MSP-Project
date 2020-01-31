using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetDesk : MonoBehaviour
{
    private Vector3 initialPosW1,initialPosW2,initialPosW3;
    private Quaternion initialRotXW1;
    private Quaternion initialRotXW2;
    private Quaternion initialRotXW3;
   

    public GameObject word1, word2, word3;

    void Start()
    {
       
        initialRotXW1 = word1.transform.localRotation;


        initialRotXW2 = word2.transform.localRotation;


        initialRotXW3 = word3.transform.localRotation;
      
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
