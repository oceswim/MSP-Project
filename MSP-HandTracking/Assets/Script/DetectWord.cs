using UnityEngine;
using TMPro;
using Leap.Unity.Interaction;

public class DetectWord : MonoBehaviour
{
    private TMP_Text toCompare;
    private Rigidbody colliderRigidbody;
    public GameObject congratsCanvas;
    public static int sizeLevel1, sizeLevel2, sizeLevel3, maxIndex;
    private int completionTracker,counter;
    public AudioSource rightAnswer, wrongAnswer,finished;
    public GameObject nextWordCanvas;
    public ParticleSystem right, wrong;
    private BoxCollider myTrigger;
    private bool wrongWord;
    public static bool activateMyTrigger;
    private int try1;
    private void Awake()
    {
        try1= 0;
        myTrigger = gameObject.GetComponent<BoxCollider>();
        Debug.Log("my trigger is enabled" + myTrigger.enabled);
        maxIndex = 2;
        completionTracker= counter = 0;
        wrongWord = activateMyTrigger = false;
    }
    private void Update()
    {
         if (wrongWord)
        {
            wrongWord = false;
            resetDesk.wordSelectReset = true;
        }
    if(activateMyTrigger)
        {
            ActivateTrigger();
            activateMyTrigger = false;
        }

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
        
        if (PlayerPrefs.HasKey("theWord"))
        {
            if (other.GetComponent<InteractionBehaviour>() != null)
            {
                other.GetComponent<InteractionBehaviour>().enabled = false;
            }
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
                        Debug.Log(try1+": the counter:" + counter + " counter modulo " + maxIndex + ": " + (counter % maxIndex) + " completion tracker = " + completionTracker);
                        try1++;
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
                                Debug.Log("in 2new words the counter:" + counter + " counter modulo "+maxIndex+": " + (counter % maxIndex) + " completion tracker = " + completionTracker);
                                //next 2 words canvas
                                Debug.Log("2 new words learned but not a level completed");
                                nextWordCanvas.SetActive(true);
                                maxIndex += 2;
                                counter = 0;
                            }
                            else if (counter == completionTracker)//all words are guessed
                            {
                                congratsCanvas.SetActive(true);//you completed a category
                                GameManager.instance.SwitchLevels(); //deactivate generate word 
                                counter = 0;
                                maxIndex = 2;
                                switch (PlayerPrefs.GetInt("Level"))
                                {
                                    case 1:
                                        PlayerPrefs.SetInt("Level", 2);
                                        PlayerPrefs.SetInt("MaxLevel", 2);
                                        break;
                                    case 2:
                                        PlayerPrefs.SetInt("MaxLevel", 3);
                                        PlayerPrefs.SetInt("Level", 3);
                                        break;
                                    case 3:
                                        //finished learning basics french word what do you want to do
                                        finished.Play();
                                        break;
                                }
                                GameManager.instance.SwitchLevels();// activate generate wword again to display next category
                                GenerateWord.newCategory = true;


                            }
                        }
                        else if (counter > 0 && counter <= completionTracker)//new words guessed out of 2
                        {
                            Debug.Log("Here");
                            right.Play();
                            //StartCoroutine(ShowThenHideCanvas(correctAnswerCanvas));
                            GenerateWord.nextWord = true;//display next word
                                                         //with static bool and new method in generate a word
                        }

                    }
                    else //wrong answer count
                    {
                        Debug.Log("Not hey");
                        wrong.Play();
                        wrongAnswer.Play();
                        wrongWord = true;
                        ActivateTrigger();  

                    }


                }
            }
        }
        else
        {
            Debug.Log("No prefs");
        }

        
    }
   /* private IEnumerator ShowThenHideCanvas(GameObject toHide)
    {
        toHide.SetActive(true);
        yield return new WaitForSeconds(8);
        toHide.SetActive(false);
    }*/
}
