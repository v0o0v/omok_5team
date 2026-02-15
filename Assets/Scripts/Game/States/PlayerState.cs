using static Omok.Constants;

namespace Omok.States {

    public class PlayerState : BaseState {
        public PlayerState(bool isFirstPlayer) : base(isFirstPlayer)
        {
        }

        public override void OnEnter(GameLogic gameLogic){
            gameLogic.blockController.onBlockClicked = (x,y) => { HandleMove(gameLogic, x,y); };
            GameManager.Instance.SetGameTurn(_playerType);
        }

        public override void HandleMove(GameLogic gameLogic, int x, int y)
        {
            if (gameLogic.IsInputLocked)
                return;
            ProcessMove(gameLogic, x, y, _playerType);
        }

        public override void HandleNextTurn(GameLogic gameLogic){
            gameLogic.ChangeGameState();
        }

        public override void OnExit(GameLogic gameLogic){ }

    }

}