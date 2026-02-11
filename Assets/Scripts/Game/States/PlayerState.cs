using static Omok.Constants;

namespace Omok.States {

    public class PlayerState : BaseState {

        private Constants.PlayerType _playerType;

        public PlayerState(bool isFirstPlayer){
            _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
        }

        public override void OnEnter(GameLogic gameLogic){
            gameLogic.blockController.onBlockClicked = (x,y) => { HandleMove(gameLogic, x,y); };
            GameManager.Instance.SetGameTurn(_playerType);
        }

        public override void HandleMove(GameLogic gameLogic, int x, int y){
            ProcessMove(gameLogic, x, y, _playerType);
        }

        public override void HandleNextTurn(GameLogic gameLogic){
            gameLogic.ChangeGameState();
        }

        public override void OnExit(GameLogic gameLogic){ }

    }

}