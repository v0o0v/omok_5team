using static Omok.Constants;

namespace Omok.States
{

    public class AIState : BaseState
    {

        public AIState(bool isFirstPlayer) : base(isFirstPlayer)
        { }

        public override void OnEnter(GameLogic gameLogic)
        {
            GameManager.Instance.SetGameTurn(_playerType);
            PlayerType[,] board = gameLogic.Board;
            (int x, int y)? result = TicTacToeAI.GetBestMove(board, _playerType, 10);
            if (result.HasValue)
            {
                HandleMove(gameLogic, result.Value.x, result.Value.y);
            }
        }

        public override void HandleMove(GameLogic gameLogic, int x, int y)
        {
            ProcessMove(gameLogic, x, y, _playerType);
        }

        public override void HandleNextTurn(GameLogic gameLogic)
        {
            gameLogic.ChangeGameState();
        }

        public override void OnExit(GameLogic gameLogic)
        { }

    }

}