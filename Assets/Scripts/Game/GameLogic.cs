using Game;
using Omok.States;
using static Omok.Constants;

namespace Omok
{
    public class GameLogic
    {
        private ForbiddenPositionGetter _forbiddenPositionGetter;
        public BlockController blockController;
        public GamePanelController _gamePanelController;
        public bool IsInputLocked { get; set; } = false;

        private PlayerType[,] _board;

        public BaseState playerAState;
        public BaseState playerBState;

        private BaseState _currentState;
        private readonly Timer _timer;

        public enum GameResult { None, Win, Lose, Draw }

        public PlayerType[,] Board => _board;
        private GameType _gameType;

        public GameLogic(GameType gameType, BlockController blockController, Timer timer, GamePanelController gamePanelController)
        {
            _timer = timer;
            _gamePanelController = gamePanelController;
            _forbiddenPositionGetter = new ForbiddenPositionGetter();
            this.blockController = blockController;
            _board = new PlayerType[BOARD_SIZE, BOARD_SIZE];
            blockController.initBoard(_board);
            _gameType = gameType;

            switch (gameType)
            {
                case GameType.SinglePlay:
                    playerAState = new PlayerState(true);
                    playerBState = new AIState(false);
                    SetState(playerAState);
                    break;
                case GameType.DualPlay:
                    playerAState = new PlayerState(true);
                    playerBState = new PlayerState(false);
                    SetState(playerAState);
                    break;
            }
        }

        public void SetState(BaseState newState)
        {
            // 기존 구동중인 초읽기 중단
            if (_timer.IsRunning(0))
                _timer.Stop(0);

            _currentState?.OnExit(this);
            _currentState = newState;
            _currentState?.OnEnter(this);

            // 초읽기 시작
            _timer.Start(0, 30, () =>
            {
                EndGame(_currentState.GetPlayerType() == PlayerType.Player1 ? GameResult.Lose : GameResult.Win);
            });
        }

        public bool PlaceMarker(int x, int y, PlayerType playerType)
        {
            if (_board[y, x] != PlayerType.None) {
                SoundManager.instance.PlaySFX(Enum_Sfx.BLOCKED_STONE);
                return false;
            }

            blockController.PlaceMarker(x, y, playerType);
            _board[y, x] = playerType;
            return true;
        }

        public void ChangeGameState()
        {
            blockController.ClearForbiddenMarks();

            SetState(_currentState == playerAState ? playerBState : playerAState);

            var points = _forbiddenPositionGetter.GetForbiddenPosition(_board, _currentState.GetPlayerType());
            foreach (var (x, y) in points)
                blockController.PutForbiddenMark(x, y);
        }

        public GameResult CheckGameResult()
        {
            if (OmokAI.CheckGameWin(PlayerType.Player1, _board))
            {
                return GameResult.Win;
            }

            if (OmokAI.CheckGameWin(PlayerType.Player2, _board))
            {
                return GameResult.Lose;
            }

            if (OmokAI.CheckGameDraw(_board))
            {
                return GameResult.Draw;
            }

            return GameResult.None;
        }

        public void EndGame(GameResult gameResult)
        {
            _ = gameResult switch  {              

                    GameResult.Win => ExecuteSequence(() => { SoundManager.instance.PlaySFX(Enum_Sfx.WINNING3); }),
                    GameResult.Lose =>ExecuteSequence(() => { if(_gameType == GameType.SinglePlay) SoundManager.instance.PlaySFX(Enum_Sfx.FALL1);
                                        else SoundManager.instance.PlaySFX(Enum_Sfx.WINNING3); }),
                    GameResult.Draw => ExecuteSequence(() => { SoundManager.instance.PlaySFX(Enum_Sfx.FALL1);  }),
                    _ => null            
            };

            string resultStr = gameResult switch
            {
                GameResult.Win => "Player1 승리!",
                GameResult.Lose => _gameType == GameType.SinglePlay? "Player1 패배":"Player2 승리!",
                GameResult.Draw => "무승부!",
                _ => ""
            };

            _gamePanelController.SetAvatarState(PlayerType.Player1, gameResult == GameResult.Win ? AvatarState.Win : AvatarState.Lose);
            _gamePanelController.SetAvatarState(PlayerType.Player2, gameResult == GameResult.Win ? AvatarState.Lose : AvatarState.Win);

            GameManager.Instance.OpenConfirmPanel(resultStr
                , () => { GameManager.Instance.ChangeToMainScene(); }
            );
        }

        // 헬퍼 함수
        object ExecuteSequence(System.Action action) { action(); return null; }

    }
}