namespace Omok {

    public static class Constants 
    {

        public const string SCENE_MAIN = "Main";
        public const string SCENE_GAME = "Game";
        public const string SCENE_HISTORY = "History";
        public const string SCENE_AVATAR_SELECTION = "AvatarSelection";
        public const string SINGLE_PLAY = "싱글 플레이";
        public const string DUAL_PLAY = "2인 플레이";
        public const string MULTI_PLAY = "멀티 플레이";


        public enum GameType { SinglePlay, DualPlay }
        
        public enum PlayerType { None, Player1, Player2 }
        
        public const int BOARD_SIZE = 15;

    }

}