using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
/*
*
*AIzaSyAbZuzy5_ilRsP6xNHIyP4sBH7ixLBqsOw --> the API key
*search engine id 003496967335787894975:7yarfcsjner
*miel
*
*/
public class GenerateWord : MonoBehaviour
{
    //using google API custom search to get images online based on the word
    private const string URL = "https://www.googleapis.com/customsearch/v1?key=AIzaSyAbZuzy5_ilRsP6xNHIyP4sBH7ixLBqsOw&cx=003496967335787894975:7yarfcsjner&q=";

    public TMP_Text englishWord, frenchWord, word1, word2, word3;
    private List<string> englishVersion, frenchVersion;
    public Material frameTexture;//applied based on the current word
    public Texture[] colors;//will be fetched online
    private string path = "Assets/Words/words.txt";
    private string imgURL;
    private int currentIndex;
    void Start()
    {
        currentIndex = 0;
        englishVersion = new List<string>();
        frenchVersion = new List<string>();


        //based on level then fetch colors/ object / places
        if (PlayerPrefs.GetInt("Level") > 1)
        {
            FillWordArray();
            FetchContent(currentIndex);//will fetch the needed words and textures if level not one.
        }
        else
        {
            for(int i=0;i<colors.Length;i++)
            {
                englishVersion.Add(colors[i].name);
               switch(colors[i].name)
                {
                    
                    case "blue":
                        frenchVersion.Add("bleu");
                        break;
                    case "green":
                        frenchVersion.Add("green");
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
        }

    }
    private void FillWordArray()//specify which words to fetch from list
    {
        StreamReader reader = new StreamReader(path);
        string fileContent = reader.ReadToEnd();
        //Debug.Log(fileContent);
        reader.Close();
        string[] temp = fileContent.Split(';');
        for (int i = 0; i < temp.Length; i++)
        {
            if (i % 2 == 0)
            {
                englishVersion.Add(temp[i]);
            }
            else
            {
                frenchVersion.Add(temp[i]);
            }
        }
        Debug.Log("done");

    }
    private void FetchContent(int theIndex)
    {

        string theURL = URL + frenchVersion[theIndex] + "&image_type=photo";

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
        if (PlayerPrefs.GetInt("Level") > 1)
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
        if (PlayerPrefs.GetInt("Level") > 1)
        {
            FetchContent(currentIndex);
        }
        else
        {
            UpdateDisplay(colors[currentIndex], frenchVersion[currentIndex], englishVersion[currentIndex]);
        }
    }



}
