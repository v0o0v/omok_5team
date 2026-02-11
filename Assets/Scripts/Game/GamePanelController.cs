using UnityEngine;
using UnityEngine.UI;
using static Tictactoe.Constants;

namespace Tictactoe {

    public class GamePanelController : MonoBehaviour {

        [SerializeField] private Image playerATurnImage;
        [SerializeField] private Image playerBTurnImage;

        public void OnClickBackButton(){
            GameManager.Instance.OpenConfirmPanel("게임을 종료합니다", () => {
                GameManager.Instance.ChangeToMainScene();
            });
        }

        public void OnClickSettingsButton(){
            GameManager.Instance.OpenSettingsPanel();
        }

        public void SetPlayerTurnPanel(PlayerType playerType){
            switch (playerType){
                case PlayerType.None:
                    playerATurnImage.color = Color.white;
                    playerBTurnImage.color = Color.white;
                    break;
                case PlayerType.Player1:
                    playerATurnImage.color = Color.deepSkyBlue;
                    playerBTurnImage.color = Color.white;
                    break;
                case PlayerType.Player2:
                    playerATurnImage.color = Color.white;
                    playerBTurnImage.color = Color.deepSkyBlue;
                    break;
            }
        }

    }

}