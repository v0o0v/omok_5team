using static Omok.Constants;
using System.Threading.Tasks;
using UnityEngine;

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
                OmokAI.GetBestMove(board, _playerType, 10)
            );
            if (result.HasValue)
            {
                HandleMove(gameLogic, result.Value.x, result.Value.y);
            }
        }

        public override async void HandleMove(GameLogic gameLogic, int x, int y)
        {
            // AI 턴에서 속도가 너무 빠른 문제
            // 유니티 게임 시간 기준으로 0.8초 지연
            await Awaitable.WaitForSecondsAsync(0.8f);
            ProcessMove(gameLogic, x, y, _playerType);
        }

        public override void HandleNextTurn(GameLogic gameLogic)
        {
            gameLogic.ChangeGameState();
        }

        public override void OnExit(GameLogic gameLogic)
        {
            gameLogic.IsInputLocked = false;
        }

    }

}