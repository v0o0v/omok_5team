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

        // 이미지-> 텍스트 필드로 변경 - [leomanic]
        // [SerializeField] private Image playerATurnImage;
        // [SerializeField] private Image playerBTurnImage;

        [SerializeField] private TMPro.TMP_Text playerATurnText;
        [SerializeField] private TMPro.TMP_Text playerBTurnText;

        [SerializeField] private PlayerHud playerAHud;
        [SerializeField] private PlayerHud playerBHud;

        private void Start()
        {
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
                    playerATurnText.color = Color.white;
                    playerBTurnText.color = Color.white;
                    playerAHud.SetAvatarState(AvatarState.Wait);
                    playerBHud.SetAvatarState(AvatarState.Wait);
                    break;
                case Constants.PlayerType.Player1:
                    playerATurnText.color = Color.deepSkyBlue;
                    playerBTurnText.color = Color.white;
                    playerAHud.SetAvatarState(AvatarState.Think);
                    playerBHud.SetAvatarState(AvatarState.Wait);
                    break;
                case Constants.PlayerType.Player2:
                    playerATurnText.color = Color.white;
                    playerBTurnText.color = Color.deepSkyBlue;
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