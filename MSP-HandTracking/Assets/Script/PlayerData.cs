
[System.Serializable]
public class PlayerData // here we store informations of our player
{
    public int levelReached;
    public int currentLevel;

    public PlayerData()
    {
        currentLevel = 1;
        levelReached = 1;
    }
}