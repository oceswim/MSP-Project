
   [System.Serializable]
    public class Game
    {
        public static Game current;
        public PlayerData thePlayer;
        public Game()
        {
            thePlayer = new PlayerData();
        }

    }

