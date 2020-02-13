using UnityEngine;
using TMPro;
using System.Collections;

public class DetectWord : MonoBehaviour
{
    private TMP_Text toCompare;
    private Rigidbody colliderRigidbody;
    public GameObject congratsCanvas;
    public static int sizeLevel1, sizeLevel2, sizeLevel3, maxIndex;
    private int completionTracker,counter;
    public AudioSource rightAnswer, wrongAnswer;
    public GameObject correctAnswerCanvas, nextWordCanvas;
    private BoxCollider myTrigger;
    private bool wrongWord;
    private void Start()
    {
        myTrigger = gameObject.GetComponent<BoxCollider>();
        maxIndex = 2;
        completionTracker= counter = 0;
        wrongWord = false;
    }
    private void Update()
    {
         if (wrongWord)
        {
            wrongWord = false;
            resetDesk.wordSelectReset = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (PlayerPrefs.HasKey("theWord"))
        {
            if (other.transform.tag.Equals("Word"))
            {
                myTrigger.enabled = false;
                toCompare = other.transform.Find("Canvas/theWord").GetComponent<TMP_Text>();
                Debug.Log(toCompare + " compared to the word : " + PlayerPrefs.GetString("theWord"));
                colliderRigidbody = other.transform.GetComponent<Rigidbody>();
                if (toCompare != null)
                {

                    other.transform.rotation = transform.rotation;
                    colliderRigidbody.freezeRotation = true;
                    if (toCompare.text.Equals(PlayerPrefs.GetString("theWord")))
                    {
                        Debug.Log("it's hey");
                        rightAnswer.Play();//right answer sound 
                        counter++;
                        Debug.Log("the counter:" + counter + " counter modulo " + maxIndex + ": " + (counter % maxIndex) + " completion tracker = " + completionTracker);
                        switch (PlayerPrefs.GetInt("Level"))
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

                        if (counter % maxIndex == 0 && counter <= completionTracker)
                        {
                            if (counter != completionTracker && counter > 0)//2 new words guessed
                            {
                                Debug.Log("the counter:" + counter + " counter modulo "+maxIndex+": " + (counter % maxIndex) + " completion tracker = " + completionTracker);
                                //next 2 words canvas
                                Debug.Log("2 new words learned but not a level completed");
                                nextWordCanvas.SetActive(true);
                                maxIndex += 2;
                                counter = 0;
                            }
                            else if (counter == completionTracker)//all words are guessed
                            {
                                maxIndex = 2;
                                switch (PlayerPrefs.GetInt("Level"))
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

                                //deactivate generate word and activate it again to display next category

                                congratsCanvas.SetActive(true);//you completed a level
                            }
                        }
                        else if (counter > 0 && counter <= completionTracker)//new words guessed out of 2
                        {
                            Debug.Log("Here");
                            StartCoroutine(ShowThenHideCanvas(correctAnswerCanvas));
                            GenerateWord.nextWord = true;//display next word
                                                         //with static bool and new method in generate a word
                        }

                    }
                    else //wrong answer count
                    {
                        Debug.Log("Not hey");
                        wrongAnswer.Play();
                        wrongWord = true;

                    }


                }
            }
        }
        else
        {
            Debug.Log("No prefs");
        }

        myTrigger.enabled = true;
    }
    private IEnumerator ShowThenHideCanvas(GameObject toHide)
    {
        toHide.SetActive(true);
        yield return new WaitForSeconds(8);
        toHide.SetActive(false);
    }
}
