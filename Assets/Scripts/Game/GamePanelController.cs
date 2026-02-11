using UnityEngine;
using UnityEngine.UI;
using static Omok.Constants;

namespace Omok {

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

        public void SetPlayerTurnPanel(Constants.PlayerType playerType){
            switch (playerType){
                case Constants.PlayerType.None:
                    playerATurnImage.color = Color.white;
                    playerBTurnImage.color = Color.white;
                    break;
                case Constants.PlayerType.Player1:
                    playerATurnImage.color = Color.deepSkyBlue;
                    playerBTurnImage.color = Color.white;
                    break;
                case Constants.PlayerType.Player2:
                    playerATurnImage.color = Color.white;
                    playerBTurnImage.color = Color.deepSkyBlue;
                    break;
            }
        }

    }

}