﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class GenerateWord : MonoBehaviour
{
    //using google API custom search to get images online based on the word
    private const string URL = "https://pixabay.com/api/?key=15096738-95fe6f1ad98a8ac365a73331d&q=";

    public TMP_Text frenchWord, word1, word2, word3;
    public TMP_Text[] englishWord;
    private List<string> englishVersion, frenchVersion;
    public Material frameTexture;//applied based on the current word
    public Texture[] colors;
    private string pathTown = "Assets/Ressources/Words/inTown.txt";
    private string pathAnimals = "Assets/Ressources/Words/animals.txt";
    private string imgURL;
    private int currentIndex, practiceIndex,speakIndex;
    public AudioSource[] theColorsVoiceF, theColorsVoiceE;
    public AudioSource[] theAnimalsVoiceF, theAnimalsVoiceE;
    public AudioSource[] theInTownVoiceF, theInTownVoiceE;
    public AudioSource[] teacher;
    public static bool nextWord,speak;
    public Button next, previous, practice, listen;
    private bool isSpeaking,allButtonsActive;
    void Start()
    {
        practiceIndex = speakIndex = 0;
        currentIndex = 0;
        englishVersion = new List<string>();
        frenchVersion = new List<string>();

        previous.interactable = practice.interactable = allButtonsActive= false;

        //based on level then fetch colors/ object / places
        if (PlayerPrefs.GetInt("Level") > 1)
        {
            switch(PlayerPrefs.GetInt("Level"))
            {
                case 2:
                    Debug.Log("animals");
                    FillWordArray(pathAnimals) ;
                    break;
                case 3:
                    Debug.Log("IN TTOWN");
                    FillWordArray(pathTown);
                    break;
            }
           
            FetchContent(currentIndex);//will fetch the needed words and textures if level not one.
        }
        else
        {
            for (int i = 0; i < colors.Length; i++)
            {
                englishVersion.Add(colors[i].name);
                Debug.Log(englishVersion[i]);
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

    }
    private void Update()
    {
        if(nextWord)
        {
            nextWord = false;
            NextWordPractice();
            resetDesk.wordSelectReset = true;
        }
        if(speak)
        {
            speak = false;
            SpeakTeacher();
        }
        if(isSpeaking && speakIndex ==0)
        {
            speakIndex = 1;
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
            next.interactable = true;
            if (allButtonsActive)
            {
                previous.interactable = true;
                practice.interactable = true;
            }
            listen.interactable = true;
        }
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
        if(PlayerPrefs.GetInt("Level")==2)
        {
            DetectWord.sizeLevel2 = frenchVersion.Count;
        }
        else if(PlayerPrefs.GetInt("Level")==3)
        {
            DetectWord.sizeLevel3 = frenchVersion.Count;
        }

    }
   
    private void FetchContent(int theIndex)
    {

        string theURL = URL + frenchVersion[theIndex] + "&image_type=vectors";

        StartCoroutine(GetRequest(theURL, theIndex));
    }
    private IEnumerator GetRequest(string uri, int i)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {

            }
            else if (!webRequest.isHttpError)
            {
                string[] pageContent = webRequest.downloadHandler.text.Split('"');
                for (int x = 0; x < pageContent.Length; x++)
                {

                    if (pageContent[x].Equals("largeImageURL"))
                    {

                        imgURL = pageContent[x + 2];
                        Debug.Log(imgURL + " " + frenchVersion[i]);
                        StartCoroutine(GetTexture(imgURL,i));
                        break;
                    }
                }

            }
        }
    }


    private IEnumerator GetTexture(string img, int theIndex)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(img);
        yield return www.SendWebRequest();

        Texture myTexture = DownloadHandlerTexture.GetContent(www);
       
        UpdateDisplay(myTexture, frenchVersion[theIndex], englishVersion[theIndex]);


    }
    public void StartPractice()
    {
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
    }
    private void NextWordPractice()//we update the current word and display new info on the practice table.
    {
        practiceIndex++;
        StartCoroutine(NextWordProcess(practiceIndex));
        
    }
    private IEnumerator NextWordProcess(int index)
    {
        if (PlayerPrefs.GetInt("Level") > 1)
        {
            FetchContent(index);
        }
        else
        {
            UpdateDisplay(colors[index], frenchVersion[index], englishVersion[index]);
        }
        yield return new WaitForSeconds(1);//alows to wait for new word to be set
        GenerateRandomWords();
    }
    public void NextImage()
    {
        if (currentIndex == (DetectWord.maxIndex - 1))
        {
            currentIndex = 0;
            if(!allButtonsActive)
            {
                allButtonsActive = true;
            }
        }
        else
        {
            currentIndex++;
        }
        if (PlayerPrefs.GetInt("Level") > 1)//if not colors level
        {
            FetchContent(currentIndex);
        }
        else
        {
            UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
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
        if (PlayerPrefs.GetInt("Level") > 1)//if not colors level
        {
            FetchContent(currentIndex);
        }
        else
        {
            UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
        }
    }

    public void ToBeginning()
    {
        currentIndex = 0;
        allButtonsActive = false;
        previous.interactable = false;
        practice.interactable = false;
        switch(PlayerPrefs.GetInt("Level"))
        {
            case 1:
                UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
                SpeakTeacher();
                break;
            case 2:
                FetchContent(currentIndex);
                SpeakTeacher();
                break;
            case 3:
                FetchContent(currentIndex);
                SpeakTeacher();
                break;
        }
        
    }

}
