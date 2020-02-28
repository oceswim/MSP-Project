using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractions : MonoBehaviour
{
    private bool NextStep1, NextStep2, stop;
    public GameObject ConfirmBase;
    public GameObject text1, text2;
    public ParticleSystem rightAnswer1, rightAnswer2;
    public AudioSource good;
    private Quaternion initialRot;
    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.localRotation;
        initialPos = transform.position;
        NextStep1 = NextStep2 = stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (NextStep1 && NextStep2 && !stop)
        {
            stop = true;
            ConfirmBase.SetActive(true);
            text1.SetActive(false);
            text2.SetActive(true);

        }
    }
    public void ResetPosition()
    {
        transform.position = initialPos;
        transform.rotation = Quaternion.identity;
        transform.localRotation = initialRot;
        Debug.Log(transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("Trigger1"))
        {
            if (!NextStep1)
            {
                Debug.Log("hey1");
                good.Play();
                rightAnswer1.Play();
                NextStep1 = true;
            }
        }
        else if (other.transform.name.Equals("Trigger2"))
         {
            if (!NextStep2)
            {
                rightAnswer2.Play();
                good.Play();
                Debug.Log("hey2");
                NextStep2 = true;
            }
        }
    }
}
