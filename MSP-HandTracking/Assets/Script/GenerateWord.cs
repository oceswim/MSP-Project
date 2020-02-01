using UnityEngine;
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

    public TMP_Text englishWord, frenchWord, word1, word2, word3;
    private List<string> englishVersion, frenchVersion;
    public Material frameTexture;//applied based on the current word
    public Texture[] colors;
    private string pathTown = "Assets/Ressources/Words/inTown.txt";
    private string pathAnimals = "Assets/Ressources/Words/animals.txt";
    private string imgURL;
    private int currentIndex;
    public AudioSource[] theColorsVoiceF, theColorsVoiceE;
    public AudioSource[] theAnimalsVoiceF, theAnimalsVoiceE;
    public AudioSource[] theInTownVoiceF, theInTownVoiceE;
    public AudioSource[] teacher;
    void Start()
    {
        currentIndex = 0;
        englishVersion = new List<string>();
        frenchVersion = new List<string>();


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

                    case "blue":
                        frenchVersion.Add("bleu");
                        Debug.Log(frenchVersion[i]);
                        break;
                    case "green":
                        frenchVersion.Add("vert");
                        Debug.Log(frenchVersion[i]);
                        break;
                    case "orange":
                        frenchVersion.Add("orange");
                        Debug.Log(frenchVersion[i]);
                        break;
                    case "purple":
                        frenchVersion.Add("violet");
                        Debug.Log(frenchVersion[i]);
                        break;
                    case "red":
                        frenchVersion.Add("rouge");
                        Debug.Log(frenchVersion[i]);
                        break;
                    case "white":
                        frenchVersion.Add("blanc");
                        Debug.Log(frenchVersion[i]);
                        break;
                    case "yellow":
                        frenchVersion.Add("jaune");
                        Debug.Log(frenchVersion[i]);
                        break;
                 
                }
               
            }
            UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
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
        Debug.Log("done");

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
                        StartCoroutine(GetTexture(imgURL));
                        break;
                    }
                }

            }
        }
    }


    private IEnumerator GetTexture(string img)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(img);
        yield return www.SendWebRequest();

        Texture myTexture = DownloadHandlerTexture.GetContent(www);
       
        UpdateDisplay(myTexture, frenchVersion[currentIndex], englishVersion[currentIndex]);


    }
    private void UpdateDisplay(Texture theTexture, string french,string english)
    {
        frameTexture.mainTexture = theTexture;
        frenchWord.text = french;
        englishWord.text = english;
        PlayerPrefs.SetString("theWord", french);

        


    }
    public void SpeakTeacher()
    {
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
    public void NextImage()
    {
        if (currentIndex == (frenchVersion.Count - 1))
        {
            currentIndex = 0;
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
            currentIndex = (frenchVersion.Count - 1);
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



}
