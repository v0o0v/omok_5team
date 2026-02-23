using System;
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

        private Canvas _canvas;
        private GamePanelController _gamePanelController;

        private GameType _gameType;
        private GameLogic _gameLogic;
        private Timer _timer;

        private void Start()
        {
        }

        protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            _canvas = FindFirstObjectByType<Canvas>();

            if (scene.name == SCENE_GAME)
            {
                BlockController blockController = FindFirstObjectByType<BlockController>();
                _gamePanelController = FindFirstObjectByType<GamePanelController>();
                if (_timer == null)
                    _timer = new Omok.Timer(new Time(), _loop);
                _gameLogic = new GameLogic(_gameType, blockController, _timer, _gamePanelController);

                _gamePanelController.SetAvatar(PlayerType.Player1, AvatarID.Avatar0);
                SetOpponentAvatar();

                _gameLogic.Start();
            }
        }

        private void SetOpponentAvatar()
        {
            if (_gameType == GameType.SinglePlay)
                _gamePanelController.SetAvatar(PlayerType.Player2, AvatarID.Avatar1);
            else
                _gamePanelController.SetAvatar(PlayerType.Player2, AvatarID.Avatar0);
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

        public void ChangeToGameScene(GameType gameType)
        {
            _gameType = gameType;
            SceneManager.LoadScene("Game");
        }

        public void ChangeToMainScene()
        {
            SceneManager.LoadScene("Main");
        }

    }

}