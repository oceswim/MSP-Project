using Leap.Unity.Interaction;
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
    public static bool wordSelectReset;
    private InteractionBehaviour word1Interaction, word2Interaction, word3Interaction;
    void Start()
    {
        word1Interaction = word1.GetComponent<InteractionBehaviour>();
        word2Interaction = word2.GetComponent<InteractionBehaviour>();
        word3Interaction = word3.GetComponent<InteractionBehaviour>();
       
        initialRotXW1 = word1.transform.localRotation;
        initialRotXW2 = word2.transform.localRotation;
        initialRotXW3 = word3.transform.localRotation;
      
        initialPosW1 = word1.transform.position;
        initialPosW2 = word2.transform.position;
        initialPosW3 = word3.transform.position;
    }

    private void Update()
    {
        if(wordSelectReset)
        {
            wordSelectReset = false;
            ResetPositionAndRotation();

        }
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

        //resetting any interaction behavior turnedoff
        if(word1Interaction.enabled == false)
        {
            word1Interaction.enabled = true;
        }
        else if(word2Interaction.enabled == false)
        {
            word2Interaction.enabled = true;
        }
        else if(word3Interaction.enabled == false)
        {
            word3Interaction.enabled = true;
        }

      
    }
 
    
}
