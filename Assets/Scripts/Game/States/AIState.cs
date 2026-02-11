using static Tictactoe.Constants;

namespace Tictactoe.States {

    public class AIState : BaseState {

        private PlayerType _playerType;

        public AIState(bool isFirstPlayer){
            _playerType = isFirstPlayer ? PlayerType.Player1 : PlayerType.Player2;
        }

        public override void OnEnter(GameLogic gameLogic){
            GameManager.Instance.SetGameTurn(_playerType);
            PlayerType[,] board = gameLogic.Board;
            (int row, int col)? result = TicTacToeAI.GetBestMove(board);
            if (result.HasValue){
                int index = result.Value.row * Constants.BOARD_SIZE + result.Value.col;
                HandleMove(gameLogic, index);
            }
        }

        public override void HandleMove(GameLogic gameLogic, int index){
            ProcessMove(gameLogic, index, _playerType);
        }

        public override void HandleNextTurn(GameLogic gameLogic){
            gameLogic.ChangeGameState();
        }
        public override void OnExit(GameLogic gameLogic){ }

    }

}