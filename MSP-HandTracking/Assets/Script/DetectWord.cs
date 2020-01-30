using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectWord : MonoBehaviour
{
    private TMP_Text toCompare;
    private Rigidbody colliderRigidbody;
    public GameObject congratsCanvas;
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerPrefs.HasKey("theWord"))
        {
            toCompare = other.transform.Find("Canvas/theWord").GetComponent<TMP_Text>();
            colliderRigidbody = other.transform.GetComponent<Rigidbody>();
            if (toCompare != null)
            {

                other.transform.rotation = transform.rotation;
                colliderRigidbody.freezeRotation = true;
                if (toCompare.text.Equals(PlayerPrefs.GetString("theWord")))
                {
                    //if it's the single or the last word show congrats complete level go to next level

                    //if it's not the last word resets desk with next word.
                    Debug.Log("It's hey");
                }


            }
        }

    }
}