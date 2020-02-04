using UnityEngine;
using TMPro;

public class DetectWord : MonoBehaviour
{
    private TMP_Text toCompare;
    private Rigidbody colliderRigidbody;
    public GameObject congratsCanvas;
    public static int sizeLevel1, sizeLevel2, sizeLevel3, maxIndex;
    private int completionTracker,counter;

    private void Start()
    {
        maxIndex = 2;
        completionTracker= counter = 0;
    }
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
                    counter++;
                    switch(PlayerPrefs.GetInt("Level"))
                    {
                        case 1:
                            completionTracker = sizeLevel1;
                            break;
                        case 2:
                            completionTracker = sizeLevel2;
                            break;
                        case 3:
                            completionTracker = sizeLevel3;
                            break;
                    }

                    if(counter % 2 ==0 && counter <=completionTracker)
                    {
                        if (counter != completionTracker && counter>0)//2 new words guessed
                        {
                           //next 2 words canvas
                            maxIndex += 2;
                            counter = 0;
                        }
                        else if(counter == completionTracker)//all words are guessed
                        {
                            maxIndex = 2;
                            switch(PlayerPrefs.GetInt("Level"))
                            {
                                case 1:

                                    PlayerPrefs.SetInt("Level", 2);
                                    break;
                                case 2:
                                    PlayerPrefs.SetInt("Level", 3);
                                    break;
                                case 3:
                                    //finished learning basics french word what do you want to do
                                    break;
                            }
                            
                            //deactivate generate word and activate it to display next category
                            congratsCanvas.SetActive(true);//you completed a level
                        }
                    }
                    else if(counter >0 && counter <= completionTracker)//new words guessed out of 2
                    {
                        //display next word
                        //with static bool and new method in generate a word
                    }
                    
                }


            }
        }

    }
}
