using static Tictactoe.Constants;

namespace Tictactoe.States {

    public class PlayerState : BaseState {

        private PlayerType _playerType;

        public PlayerState(bool isFirstPlayer){
            _playerType = isFirstPlayer ? PlayerType.Player1 : PlayerType.Player2;
        }

        public override void OnEnter(GameLogic gameLogic){
            gameLogic.blockController.onBlockClicked = blockIndex => { HandleMove(gameLogic, blockIndex); };
            GameManager.Instance.SetGameTurn(_playerType);
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