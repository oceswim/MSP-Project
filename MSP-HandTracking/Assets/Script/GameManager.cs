using System.Collections;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject wordGenerator, categoryButton, inTownButton;
    public GameObject LeapObject;
    private Vector3 StartPosNew = new Vector3(-3, 6.2f, -28.71f);
    private Vector3 StartPosOld = new Vector3(-3, 6.2f, -14.5f);
    // Start is called before the first frame update
    void Awake()
    {
        // if no file saved create a brand new game
        if (!File.Exists(Application.persistentDataPath + "/ThePlayerInfo.gd"))
        {
            LeapObject.transform.position = StartPosNew;
            PlayerPrefs.DeleteAll();
            Game.current = new Game();
            PlayerPrefs.SetInt("Level", Game.current.thePlayer.currentLevel);
            PlayerPrefs.SetInt("MaxLevel", Game.current.thePlayer.levelReached);
            Debug.Log("new game: "+ PlayerPrefs.GetInt("Level")+ "; "+ PlayerPrefs.GetInt("MaxLevel")+"/"+Game.current.thePlayer.levelReached);
        }
        //if file found, create a new game and load saved information
        else
        {
            LeapObject.transform.position = StartPosOld;
            SaveSystem.LoadPlayer();
            Game.current = new Game();
            Game.current.thePlayer.currentLevel = SaveSystem.currentLevel; 
            Game.current.thePlayer.levelReached = SaveSystem.reachedLevel;
            PlayerPrefs.SetInt("Level", Game.current.thePlayer.currentLevel);

            PlayerPrefs.SetInt("MaxLevel", Game.current.thePlayer.levelReached);
            Debug.Log("Loaded current level in manager: " + PlayerPrefs.GetInt("Level"));
            Debug.Log("Loaded max level in manager: " + PlayerPrefs.GetInt("MaxLevel"));
        }
        

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        if(PlayerPrefs.HasKey("MaxLevel"))
        {
            if(PlayerPrefs.GetInt("MaxLevel")>1)
            {
                categoryButton.SetActive(true);
            }
        }
        
    }
    
    public void ExitGame()
    {
        Debug.Log("BYE");
        OnApplicationQuit();
    }
    private void OnApplicationQuit()
    {
        Debug.Log("BYE");
        SaveSystem.SavePlayer();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
    public void SwitchLevels()
    {
      
        if(wordGenerator.activeInHierarchy)
        {
            Debug.Log("OFF");
            wordGenerator.SetActive(false);
        }
        else if(!wordGenerator.activeInHierarchy)
        {
            Debug.Log("ON");
            wordGenerator.SetActive(true);
        }
    }
    public void CategorySelection()
    {
        if(PlayerPrefs.GetInt("MaxLevel")==3)//since category selection available only at level 2, then colors and animal button already active;
        {
          
          inTownButton.SetActive(true);
  
        }
    }
    public void SwitchCategory(int category)
    {
        if (PlayerPrefs.GetInt("Level") != category)//if new category selected different from 
        {
            switch (category)
            {
                case 1:
                    PlayerPrefs.SetInt("Level", 1);
                    break;
                case 2:
                    PlayerPrefs.SetInt("Level", 2);
                    break;
                case 3:
                    PlayerPrefs.SetInt("Level", 3);
                    break;
            }
            StartCoroutine(SwitchLevelCoroutine());
        }
    }
    private IEnumerator SwitchLevelCoroutine()
    {
        SwitchLevels();
        yield return new WaitForSeconds(1);
        SwitchLevels();
    }
}
