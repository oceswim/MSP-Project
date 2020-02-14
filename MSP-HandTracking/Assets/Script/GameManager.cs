using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject wordGenerator;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("Level"))
        {
            Debug.Log("new game");
            PlayerPrefs.SetInt("Level", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Level", 3);
            Debug.Log("Player level" + PlayerPrefs.GetInt("Level"));
        }


        /*   if (Game.current == null)
           {
               PlayerPrefs.DeleteAll();
               Debug.Log("new game");
               Game.current = new Game();
               playerHealth = Game.current.thePlayer.health;
               PlayerPrefs.SetInt("firstLoad", 1);//allows to create a new game only at the very first load
           }*/

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);

    }
    public void ExitGame()
    {
        Debug.Log("BYE");
        Application.Quit();
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
}
