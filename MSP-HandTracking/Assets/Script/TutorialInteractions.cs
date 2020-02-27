using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractions : MonoBehaviour
{
    private bool NextStep1, NextStep2, stop;
    public GameObject ConfirmBase;
    public GameObject text1, text2;
    // Start is called before the first frame update
    void Start()
    {
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("Trigger1"))
        {
            if (!NextStep1)
            {
                NextStep1 = true;
            }
        }
        else if (other.transform.name.Equals("Trigger2"))
            {
            if (!NextStep2)
            {
                NextStep2 = true;
            }
        }
    }
}
