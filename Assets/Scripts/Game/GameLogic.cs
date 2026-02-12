using Omok.States;
using static Omok.Constants;

namespace Omok {

    public class GameLogic {

        public BlockController blockController;

        private PlayerType[,] _board;

        public BaseState playerAState;
        public BaseState playerBState;

        private BaseState _currentState;
        private readonly Timer _timer;

        public enum GameResult { None, Win, Lose, Draw }

        public PlayerType[,] Board => _board;

        public GameLogic(GameType gameType, BlockController blockController, Timer timer)
        {
            _timer = timer;

            gameType = GameType.DualPlay; //TODO 삭제할것

            this.blockController = blockController;
            _board = new PlayerType[BOARD_SIZE, BOARD_SIZE];
            switch (gameType){
                case GameType.SinglePlay:
                    playerAState = new PlayerState(true);
                    // playerBState = new AIState(false);
                    SetState(playerAState);
                    break;
                case GameType.DualPlay:
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

        public bool PlaceMarker(int x, int y, PlayerType playerType){
            if (_board[y, x] != PlayerType.None)
                return false;

            blockController.PlaceMarker(x, y, playerType);
            _board[y, x] = playerType;
            return true;
        }

        public void ChangeGameState(){
            SetState(_currentState == playerAState ? playerBState : playerAState);

            _timer.Start(0, 3, () => { EndGame(_currentState.GetPlayerType() == PlayerType.Player1 ? GameResult.Win : GameResult.Lose); });
        }

        public GameResult CheckGameResult(){
            if (TicTacToeAI.CheckGameWin(PlayerType.Player1, _board)){
                return GameResult.Win;
            }

            if (TicTacToeAI.CheckGameWin(PlayerType.Player2, _board)){
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