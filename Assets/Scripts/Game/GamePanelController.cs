using Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Omok.Constants;

namespace Omok
{

    public class GamePanelController : MonoBehaviour
    {
        [SerializeField] private PlayerHud playerAHud;
        [SerializeField] private PlayerHud playerBHud;
        [SerializeField] private TMPro.TMP_Text _gameTypeName;

        private void Start()
        {
            playerAHud.SetPlayerName("Player 1");
            playerBHud.SetPlayerName("Player 2");

            playerAHud.SetPlayerStone(PlayerType.Player1);
            playerBHud.SetPlayerStone(PlayerType.Player2);

            playerAHud.SetAvatarDirection(Vector2.left);
            playerBHud.SetAvatarDirection(Vector2.right);

            Debug.Log("Playing BGM");
            SoundManager.instance.PlayBGM(Enum_Bgm.GAME_BACKGOUND);
        }

        public void OnClickBackButton()
        {
            GameManager.Instance.OpenConfirmPanel("게임을 종료합니다", () =>
            {
                GameManager.Instance.ChangeToMainScene();
            });
        }
        public void OnClickBackButton2()
        {
            GameManager.Instance.OpenConfirmPanel("기보 보기를 종료합니다", () =>
            {
                GameManager.Instance.ChangeToMainScene();
            });
        }

        public void OnClickSettingsButton()
        {
            GameManager.Instance.OpenSettingsPanel();
        }

        public void SetPlayerTurnPanel(Constants.PlayerType playerType)
        {
            switch (playerType)
            {
                case Constants.PlayerType.None:
                    playerAHud.SetAvatarState(AvatarState.Wait);
                    playerBHud.SetAvatarState(AvatarState.Wait);
                    break;
                case Constants.PlayerType.Player1:
                    playerAHud.SetAvatarState(AvatarState.Think);
                    playerBHud.SetAvatarState(AvatarState.Wait);
                    break;
                case Constants.PlayerType.Player2:
                    playerAHud.SetAvatarState(AvatarState.Wait);
                    playerBHud.SetAvatarState(AvatarState.Think);
                    break;
            }
        }

        public void SetAvatarState(PlayerType player, AvatarState state)
        {
            if (player == PlayerType.Player1)
                playerAHud.SetAvatarState(state);
            if (player == PlayerType.Player2)
                playerBHud.SetAvatarState(state);
        }

        /// <summary>
        /// Sets the avatar for the specified player's HUD.
        /// </summary>
        /// <param name="playerType">The type of player (Player1/Player2) to update.</param>
        /// <param name="avatarID">The unique ID of the avatar to be applied.</param>
        public void SetAvatar(PlayerType playerType, string avatarID)
        {
            if (playerType == PlayerType.Player1)
                playerAHud.SetAvatar(avatarID);
            if (playerType == PlayerType.Player2)
                playerBHud.SetAvatar(avatarID);
        }

        public void SetGameTypeName(string name)
        {
            _gameTypeName.text = name;
        }
    }
}