using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       if(!PlayerPrefs.HasKey("Level"))
       {
            Debug.Log("new game");
            PlayerPrefs.SetInt("Level", 1);
       }
        else
        {
            Debug.Log("Player level"+ PlayerPrefs.GetInt("Level"));
        }
    }

}
