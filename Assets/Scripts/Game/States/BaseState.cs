namespace Omok.States {

    public abstract class BaseState {

        public abstract void OnEnter(GameLogic gameLogic);
        public abstract void HandleMove(GameLogic gameLogic, int x, int y);
        public abstract void HandleNextTurn(GameLogic gameLogic);
        public abstract void OnExit(GameLogic gameLogic);

        public void ProcessMove(GameLogic gameLogic, int x, int y, Constants.PlayerType playerType){
            if (gameLogic.PlaceMarker(x, y, playerType)){
                GameLogic.GameResult gameResult = gameLogic.CheckGameResult();
                if (gameResult == GameLogic.GameResult.None){
                    HandleNextTurn(gameLogic);
                }
                else{
                    gameLogic.EndGame(gameResult);
                }
            }
        }

    }

}