using System;
using System.Collections.Generic;
using System.Linq;
using AvatarSelection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Omok.Constants;

namespace Omok
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameObject settingsPanelPrefab;
        [SerializeField] private GameObject confirmPanelPrefab;
        [SerializeField] private Loop _loop;
        [SerializeField] private LocalDataStore _localDataStore;

        private Canvas _canvas;
        private GamePanelController _gamePanelController;

        private GameType _gameType;
        private Dictionary<PlayerType, string> _avatarIDs;

        private GameLogic _gameLogic;
        private Timer _timer;

        private void Start()
        {
            _avatarIDs = new Dictionary<PlayerType, string>();
            _timer = new Omok.Timer(new Time(), _loop);
        }

        protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            _canvas = FindFirstObjectByType<Canvas>();

            if (scene.name == SCENE_GAME)
            {
                BlockController blockController = FindFirstObjectByType<BlockController>();
                _gamePanelController = FindFirstObjectByType<GamePanelController>();

                _gameLogic = new GameLogic(_gameType, blockController, _timer, _gamePanelController);
                _gamePanelController.SetAvatar(PlayerType.Player1, _avatarIDs[PlayerType.Player1]);

                if (_gameType == GameType.DualPlay)
                    _gamePanelController.SetAvatar(PlayerType.Player2, _avatarIDs[PlayerType.Player2]);
                if (_gameType == GameType.SinglePlay)
                    _gamePanelController.SetAvatar(PlayerType.Player2, AvatarID.Avatar4);

                _gamePanelController.SetGameTypeName(_gameType == GameType.SinglePlay ? "싱글플레이" : "2인 플레이");

                _gameLogic.Start();
            }
            else if (scene.name == SCENE_AVATAR_SELECTION)
            {
                var ctrl = FindFirstObjectByType<AvatarSelectionSceneController>();

                var avatarID = _localDataStore.GetAvatarID();
                ctrl.SetAvatar(PlayerType.Player1, avatarID == null ? AvatarID.Avatar0 : avatarID);

                if (_gameType == GameType.DualPlay)
                    ctrl.SetAvatar(PlayerType.Player2, AvatarID.Avatar0);

                if (_gameType == GameType.SinglePlay)
                {
                    ctrl.SetAvatar(PlayerType.Player2, AvatarID.Avatar4);
                    ctrl.DisableSelection(PlayerType.Player2);
                }
            }
        }

        /// <summary>
        /// 제한시간 타이머를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public Timer GetTimer()
        {
            return _timer;
        }

        public void SetGameTurn(PlayerType playerTurn)
        {
            _gamePanelController.SetPlayerTurnPanel(playerTurn);
        }

        public void OpenSettingsPanel()
        {
            GameObject settingPanelObject = Instantiate(settingsPanelPrefab, _canvas.transform);
            settingPanelObject.GetComponent<SettingsPanelController>().Show();
        }

        public void OpenConfirmPanel(string message, Action onConfirm)
        {
            GameObject confirmPanel = Instantiate(confirmPanelPrefab, _canvas.transform);
            confirmPanel.GetComponent<ConfirmPanelController>().Show(message, onConfirm);
        }
        // Continue 버튼 추가된 ConfirmPanel - [leomanic]
        public void OpenContinuePanel(string message, Action onConfirm, Action onContinue)
        {
            GameObject confirmPanel = Instantiate(confirmPanelPrefab, _canvas.transform);
            confirmPanel.GetComponent<ConfirmPanelController>().Show(message, onConfirm, onContinue);
        }

        public void ChangeToGameScene(GameType gameType)
        {
            _gameType = gameType;

            // Logic: DualPlay always requires selection; SinglePlay requires it only if no AvatarID is set.
            var isAvatarIDSettedBefore = !string.IsNullOrEmpty(_localDataStore.GetAvatarID());
            var isSelectAvatarRequired = _gameType == GameType.DualPlay ||
                (_gameType == GameType.SinglePlay && !isAvatarIDSettedBefore);

            if (isSelectAvatarRequired)
                SceneManager.LoadScene("AvatarSelection");
            else
            {
                _avatarIDs[PlayerType.Player1] = _localDataStore.GetAvatarID();
                _avatarIDs[PlayerType.Player2] = _localDataStore.GetAvatarID();

                SceneManager.LoadScene("Game");
            }
        }

        // 아바타 선택을 거치지 않는 게임 계속하기 용
        public void ChangeToGameScene(GameType gameType, bool isContinue)
        {
            _gameType = gameType;

            _avatarIDs[PlayerType.Player1] = _localDataStore.GetAvatarID();
            _avatarIDs[PlayerType.Player2] =  _avatarIDs[PlayerType.Player2];


            SceneManager.LoadScene("Game");
        }

        public void ChangeToGameScene(IReadOnlyDictionary<PlayerType, string> avatarIDs)
        {
            foreach (var entry in avatarIDs)
                _avatarIDs[entry.Key] = entry.Value;

            _localDataStore.SetAvatarID(avatarIDs[PlayerType.Player1]);

            SceneManager.LoadScene("Game");
        }

        public void ChangeToMainScene()
        {
            // 메인 화면으로 복귀시 타이머 종료
            _timer.Stop();
            SceneManager.LoadScene("Main");
        }

        public void ChangeToHistory()
        {
            SceneManager.LoadScene("History");
        }

    }

}