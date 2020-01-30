using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerateWord : MonoBehaviour
{
    public TMP_Text englishWord, frenchWord,word1,word2,word3;
    public Material frameTexture;
    public Texture[] wordPicture;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateAWord()
    {
        englishWord.text = "Dog";
        frenchWord.text = "Chien";

        PlayerPrefs.SetString("theWord", frenchWord.text);
        
        int index = Random.Range(0, 3);
        Debug.Log("the index " + index);
        GenerateRandomWords(index);
        frameTexture.SetTexture("_MainTex", wordPicture[0]);
    }
    private void GenerateRandomWords(int ind)
    {
        switch(ind)
        {
            case 0:
                word1.text = frenchWord.text;
                word2.text ="";
                word3.text = "";
                break;
            case 1:
                word1.text = "";
                word2.text = frenchWord.text;
                word3.text = "";
                break;
            case 2:
                word1.text = "";
                word2.text = "";
                word3.text = frenchWord.text;
                break;
        }
    }
}
