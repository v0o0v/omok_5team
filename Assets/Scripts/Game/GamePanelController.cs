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

        private void Start()
        {
            playerAHud.SetPlayerName("Player 1");
            playerBHud.SetPlayerName("Player 2");

            playerAHud.SetPlayerStone(PlayerType.Player1);
            playerBHud.SetPlayerStone(PlayerType.Player2);

            playerAHud.SetAvatarDirection(Vector2.left);
            playerBHud.SetAvatarDirection(Vector2.right);
        }

        public void OnClickBackButton()
        {
            GameManager.Instance.OpenConfirmPanel("게임을 종료합니다", () =>
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
    }
}