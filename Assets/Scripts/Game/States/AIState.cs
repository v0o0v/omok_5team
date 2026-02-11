using static Omok.Constants;

namespace Omok.States {

    public class AIState {
        // : BaseState {
        //
        // private Constants.PlayerType _playerType;
        //
        // public AIState(bool isFirstPlayer){
        //     _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
        // }
        //
        // public override void OnEnter(GameLogic gameLogic){
        //     GameManager.Instance.SetGameTurn(_playerType);
        //     Constants.PlayerType[,] board = gameLogic.Board;
        //     (int row, int col)? result = TicTacToeAI.GetBestMove(board);
        //     if (result.HasValue){
        //         int index = result.Value.row * Constants.BOARD_SIZE + result.Value.col;
        //         HandleMove(gameLogic, index);
        //     }
        // }
        //
        // public override void HandleMove(GameLogic gameLogic, int x, int y){
        //     ProcessMove(gameLogic, x, y, _playerType);
        // }
        //
        // public override void HandleNextTurn(GameLogic gameLogic){
        //     gameLogic.ChangeGameState();
        // }
        // public override void OnExit(GameLogic gameLogic){ }

    }

}