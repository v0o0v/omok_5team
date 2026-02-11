using Omok.States;
using static Omok.Constants;

namespace Omok {

    public class GameLogic {

        public BlockController blockController;

        private Constants.PlayerType[,] _board;

        public BaseState playerAState;
        public BaseState playerBState;

        private BaseState _currentState;

        public enum GameResult { None, Win, Lose, Draw }

        public Constants.PlayerType[,] Board => _board;

        public GameLogic(Constants.GameType gameType, BlockController blockController){
            gameType = GameType.DualPlay; //TODO 삭제할것
            
            this.blockController = blockController;
            _board = new Constants.PlayerType[BOARD_SIZE, BOARD_SIZE];
            switch (gameType){
                case Constants.GameType.SinglePlay:
                    playerAState = new PlayerState(true);
                    // playerBState = new AIState(false);
                    SetState(playerAState);
                    break;
                case Constants.GameType.DualPlay:
                    playerAState = new PlayerState(true);
                    playerBState = new PlayerState(false);
                    SetState(playerAState);
                    break;
            }
        }

        public void SetState(BaseState newState){
            _currentState?.OnExit(this);
            _currentState = newState;
            _currentState?.OnEnter(this);
        }

        public bool PlaceMarker(int x, int y, Constants.PlayerType playerType){
            if (_board[y, x] != Constants.PlayerType.None)
                return false;

            blockController.PlaceMarker(x, y, playerType);
            _board[y, x] = playerType;
            return true;
        }

        public void ChangeGameState(){
            SetState(_currentState == playerAState ? playerBState : playerAState);
        }

        public GameResult CheckGameResult(){
            if (TicTacToeAI.CheckGameWin(Constants.PlayerType.Player1, _board)){
                return GameResult.Win;
            }

            if (TicTacToeAI.CheckGameWin(Constants.PlayerType.Player2, _board)){
                return GameResult.Lose;
            }

            if (TicTacToeAI.CheckGameDraw(_board)){
                return GameResult.Draw;
            }

            return GameResult.None;
        }

        public void EndGame(GameResult gameResult){
            string resultStr = gameResult switch{
                GameResult.Win => "Player1 승리!",
                GameResult.Lose => "Player2 승리!",
                GameResult.Draw => "무승부!",
                _ => ""
            };
            GameManager.Instance.OpenConfirmPanel(resultStr
                , () => { GameManager.Instance.ChangeToMainScene(); }
            );
        }

    }

}