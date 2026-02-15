using static Omok.Constants;
using System.Threading.Tasks;

namespace Omok.States
{

    public class AIState : BaseState
    {

        public AIState(bool isFirstPlayer) : base(isFirstPlayer)
        { }

        public override async void OnEnter(GameLogic gameLogic)
        {
            gameLogic.IsInputLocked = true;
            GameManager.Instance.SetGameTurn(_playerType);
            PlayerType[,] board = gameLogic.Board;
            
            // AI Turn 에서 화면이 잠시동안 Freeze 되는 버그 수정 (비동기처리) - [leomanic]
            var result = await Task.Run(() => 
                TicTacToeAI.GetBestMove(board, _playerType, 10)
            );
            if (result.HasValue)
            {
                HandleMove(gameLogic, result.Value.x, result.Value.y);
            }
            gameLogic.IsInputLocked = false;
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