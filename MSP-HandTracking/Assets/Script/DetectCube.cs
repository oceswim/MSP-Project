using UnityEngine;
using TMPro;
using Leap.Unity.Interaction;

public class DetectCube : MonoBehaviour
{
    private Rigidbody colliderRigidbody;
    public GameObject NextButton, CongratsText,resetButton, Text2;

    private int completionTracker, counter;
    public AudioSource rightAnswer;
    public ParticleSystem right;
    private BoxCollider myTrigger;

    private void Awake()
    {
        myTrigger = gameObject.GetComponent<BoxCollider>();
        Debug.Log("my trigger is enabled" + myTrigger.enabled);
        completionTracker = counter = 0;

    }
   
    public void ActivateTrigger()
    {
        if (myTrigger.enabled == true)
        {
            myTrigger.enabled = false;
        }
        else
        {
            myTrigger.enabled = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.name.Equals("PracticeCube"))
        {
            myTrigger.enabled = false;
            Debug.Log("it's hey");
            rightAnswer.Play();//right answer sound
            Destroy(other.transform.gameObject);
            Text2.SetActive(false);
            CongratsText.SetActive(true);
            right.Play();
            resetButton.SetActive(false);
            NextButton.SetActive(true);

        }
    }

}

