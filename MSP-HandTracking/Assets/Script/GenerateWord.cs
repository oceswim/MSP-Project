using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class GenerateWord : MonoBehaviour
{

    public GameObject loadingCanvas;
    public TMP_Text frenchWord, word1, word2, word3;
    public TMP_Text[] englishWord, wordCount,wordTotal;
    private List<string> englishVersion, frenchVersion;
    public Material frameTexture;//applied based on the current word
    public Texture[] colors,animals,objects;
    private string pathTown = "Assets/Ressources/Words/inTown.txt";
    private string pathAnimals = "Assets/Ressources/Words/animals.txt";

    private int currentIndex, practiceIndex, speakIndex;
    public AudioSource[] theColorsVoiceF, theColorsVoiceE;
    public AudioSource[] theAnimalsVoiceF, theAnimalsVoiceE;
    public AudioSource[] theInTownVoiceF, theInTownVoiceE;
    public AudioSource[] teacher;
    public AudioSource[] newCategories;
    public static bool nextWord,speak,newCategory, SetUp;
    public Button next, previous, practice, listen,pauseButton;
    private bool isSpeaking,allButtonsActive;
    public GameObject practiceInteractable;
   
    void Start()
    {
        SetUp = false;
        SetEverythingUp();

    }
    private void SetEverythingUp()
    {
       
            Debug.Log("HHHEEEEEEEEEEEEERE");
           
            practiceIndex = speakIndex = 0;
            currentIndex = 0;
            englishVersion = new List<string>();
            frenchVersion = new List<string>();

            previous.interactable = practice.interactable = allButtonsActive = false;

            //based on level then fetch colors/ object / places
            if (PlayerPrefs.GetInt("Level") > 1)
            {
                switch (PlayerPrefs.GetInt("Level"))
                {
                    case 2:
                        Debug.Log("animals");
                        FillWordArray(pathAnimals);
                        UpdateDisplay(animals[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                        break;
                    case 3:
                        Debug.Log("IN TTOWN");
                        FillWordArray(pathTown);
                        UpdateDisplay(objects[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                        break;
                }

                // FetchContent(currentIndex);//will fetch the needed words and textures if level not one.
            }
            else
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    englishVersion.Add(colors[i].name);
                    //Debug.Log(englishVersion[i]);
                    switch (englishVersion[i])
                    {
                        case "black":
                            frenchVersion.Add("noir");
                            break;
                        case "blue":
                            frenchVersion.Add("bleu");

                            break;
                        case "green":
                            frenchVersion.Add("vert");

                            break;
                        case "orange":
                            frenchVersion.Add("orange");

                            break;
                        case "purple":
                            frenchVersion.Add("violet");

                            break;
                        case "red":
                            frenchVersion.Add("rouge");

                            break;
                        case "white":
                            frenchVersion.Add("blanc");

                            break;
                        case "yellow":
                            frenchVersion.Add("jaune");
                            break;


                    }

                }
                DetectWord.sizeLevel1 = frenchVersion.Count;
                UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
            }
            ResetWordCounterDisplay();
        
    }
    private void Update()
    {
        if (SetUp)
        {
            SetUp = false;
            SetEverythingUp();
        }
        if (nextWord)
        {
            nextWord = false;
            NextWordPractice();

            resetDesk.wordSelectReset = true;
            DetectWord.activateMyTrigger = true;
        }
     
        if (speak)
        {
            speak = false;
            SpeakTeacher();
        }
        if(isSpeaking && speakIndex ==0)
        { 
            speakIndex = 1;
            pauseButton.interactable = false;
            next.interactable =false ;
            if (allButtonsActive)
            {
                previous.interactable = false;
                practice.interactable = false;

            }
            listen.interactable=false;
        }
        else if(!isSpeaking && speakIndex==1)
        {
            speakIndex = 0;
            pauseButton.interactable = true;
            next.interactable = true;
            if (allButtonsActive)
            {
                previous.interactable = true;
                practice.interactable = true;
                StartCoroutine(LimitedActivation());
            }
            listen.interactable = true;
        }
       
     
    }
    private IEnumerator LimitedActivation()
    {
        practiceInteractable.SetActive(true);
        yield return new WaitForSeconds(3);
        practiceInteractable.SetActive(false);
    }
    private void FillWordArray(string path)//specify which words to fetch from list
    {
        StreamReader reader = new StreamReader(path);
        string fileContent = reader.ReadToEnd();
        
        reader.Close();
        string[] temp = fileContent.Split(';');
        for (int i = 0; i < temp.Length; i++)
        {
            if (i % 2 == 0)
            {
                frenchVersion.Add(temp[i]);
                
            }
            else
            {
                englishVersion.Add(temp[i]);
               
            }
        }
        Debug.Log(frenchVersion.Count + " FRENCH");
        if (PlayerPrefs.GetInt("Level")==2)
        {

            DetectWord.sizeLevel2 = frenchVersion.Count;
        }
        else if(PlayerPrefs.GetInt("Level")==3)
        {
            DetectWord.sizeLevel3 = frenchVersion.Count;
        }

    }
   

    public void StartPractice()
    {
        ResetWordCounterDisplay();
        currentIndex = practiceIndex= 0;
        StartCoroutine(NextWordProcess(currentIndex));
    }
    private void UpdateDisplay(Texture theTexture, string french,string english)
    {
        frameTexture.mainTexture = theTexture;
        frenchWord.text = french;
        foreach (TMP_Text s in englishWord)
        {
            s.text = english;
        }
        
        PlayerPrefs.SetString("theWord", french);
        Debug.Log("prefs set to " + PlayerPrefs.GetString("theWord"));
    }
    public void SpeakTeacher()//called when pressing next and previous buttons
    {
        isSpeaking = true;
        StartCoroutine(TeacherSpeaks());
    }

    private IEnumerator TeacherSpeaks()
    {
        if(newCategory)//if switching to new category then we say it to the player
        {
            switch (PlayerPrefs.GetInt("Level"))
            {
                case 2:
                    newCategories[0].Play();
                    yield return new WaitForSeconds(7f); 
                    break;
                case 3:
                    newCategories[1].Play();
                    yield return new WaitForSeconds(7f);
                    break;
            }
            newCategory = false;
        }
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 1:
                teacher[0].Play();
                yield return new WaitForSeconds(.7f);
                theColorsVoiceE[currentIndex].Play();
                yield return new WaitForSeconds(1f);
                teacher[1].Play();
                yield return new WaitForSeconds(1);
                theColorsVoiceF[currentIndex].Play();
                
                break;
            case 2:
                teacher[0].Play();
                yield return new WaitForSeconds(.7f);
                theAnimalsVoiceE[currentIndex].Play();
                yield return new WaitForSeconds(1f);
                teacher[1].Play();
                yield return new WaitForSeconds(1);
                theAnimalsVoiceF[currentIndex].Play();
                break;
            case 3:
                teacher[0].Play();
                yield return new WaitForSeconds(.7f);
                theInTownVoiceE[currentIndex].Play();
                yield return new WaitForSeconds(1f);
                teacher[1].Play();
                yield return new WaitForSeconds(1);
                theInTownVoiceF[currentIndex].Play();
                break;
        }
        isSpeaking = false;
    }
    public void GenerateRandomWords()//to be called when pressing practice
    {
        int ind = Random.Range(0, 3);//chooses which cube will have the correct word
        int rand1 = Random.Range(0, frenchVersion.Count);
        int rand2 = Random.Range(0, frenchVersion.Count);
        if (rand1 == currentIndex || rand1 == rand2)
        {
            rand1 = Random.Range(0, frenchVersion.Count);
        }
        else if (rand2 == currentIndex)
        {
            rand2 = Random.Range(0, frenchVersion.Count);
        }
       

        switch (ind)
        {
            case 0:
                word1.text = frenchWord.text;
                word2.text = frenchVersion[rand1];
                word3.text = frenchVersion[rand2];
                break;
            case 1:
                word1.text = frenchVersion[rand1];
                word2.text = frenchWord.text;
                word3.text = frenchVersion[rand2];
                break;
            case 2:
                word1.text = frenchVersion[rand1];
                word2.text = frenchVersion[rand2];
                word3.text = frenchWord.text;
                break;
        }
        if(loadingCanvas.activeInHierarchy)
        {
            loadingCanvas.SetActive(false);
        }
    }
    private void NextWordPractice()//we update the current word and display new info on the practice table.
    {
        practiceIndex++;
        UpdateCounterWords(practiceIndex);
        StartCoroutine(NextWordProcess(practiceIndex));
        
    }
    private IEnumerator NextWordProcess(int index)
    {
      
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 1:
                UpdateDisplay(colors[index], frenchVersion[index], englishVersion[index]);
                break;
            case 2:
                UpdateDisplay(animals[index], frenchVersion[index], englishVersion[index]);
                break;
            case 3:
                UpdateDisplay(objects[index], frenchVersion[index], englishVersion[index]);
                break;
        }
     
            yield return new WaitForSeconds(1);//alows to wait for new word to be set
            Debug.Log(PlayerPrefs.GetInt("Level") + " generate new words " + index);
            GenerateRandomWords();
        
    }
    public void NextImage()
    {
        
        if (currentIndex == (DetectWord.maxIndex - 1))
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
            Debug.Log("Current index next img:" + currentIndex);
            if (currentIndex == (DetectWord.maxIndex - 1))
            {
                if (!allButtonsActive)
                {
                    allButtonsActive = true;
                }
            }
        }
        for (int i = 0; i < wordCount.Length; i++)
        {
            wordCount[i].text = (currentIndex + 1).ToString();
            wordTotal[i].text = (DetectWord.maxIndex).ToString();
        }
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 1:
                UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                break;
            case 2:
                UpdateDisplay(animals[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                break;
            case 3:
                UpdateDisplay(objects[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                break;
        }
 

    }
    public void PreviousImage()
    {
        if (currentIndex == 0)
        {
            currentIndex = (DetectWord.maxIndex - 1);
        }
        else
        {
            currentIndex--;
        }
        for (int i = 0; i < wordCount.Length; i++)
        {
            wordCount[i].text = (currentIndex + 1).ToString();
            wordTotal[i].text = (DetectWord.maxIndex).ToString();
        }
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 1:
                UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                break;
            case 2:
                UpdateDisplay(animals[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                break;
            case 3:
                UpdateDisplay(objects[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                break;
        }
    }

    public void ToBeginning()
    {
        currentIndex = 0;
        allButtonsActive = false;
        previous.interactable = false;
        practice.interactable = false;
        ResetWordCounterDisplay();
        switch(PlayerPrefs.GetInt("Level"))
        {
            case 1:
                UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                SpeakTeacher();
                break;
            case 2:
                UpdateDisplay(animals[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                SpeakTeacher();
                break;
            case 3:
                UpdateDisplay(objects[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                SpeakTeacher();
                break;
        }
        
    }
    private void UpdateCounterWords(int index)
    {
        Debug.Log("wordcount = " + index); 
         wordCount[1].text = (index+1).ToString(); 
    }
    private void ResetWordCounterDisplay()
    {
        for (int i = 0; i < wordCount.Length; i++)
        {
            wordCount[i].text = "1";
            wordTotal[i].text = (DetectWord.maxIndex).ToString();
        }
    }


}
