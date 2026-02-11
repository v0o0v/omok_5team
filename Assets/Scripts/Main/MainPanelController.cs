using UnityEngine;
using static Tictactoe.Constants;

namespace Tictactoe {

    public class MainPanelController : MonoBehaviour {

        public void onClickSinglePlayButton(){
            GameManager.Instance.ChangeToGameScene(GameType.SinglePlay);
        }

        public void onClickDualPlayButton(){
            GameManager.Instance.ChangeToGameScene(GameType.DualPlay);
        }

        public void onClickSettingsButton(){
            GameManager.Instance.OpenSettingsPanel();
        }

    }

}