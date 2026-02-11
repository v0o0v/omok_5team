using UnityEngine;
using static Omok.Constants;

namespace Omok {

    public class MainPanelController : MonoBehaviour {

        public void onClickSinglePlayButton(){
            GameManager.Instance.ChangeToGameScene(Constants.GameType.SinglePlay);
        }

        public void onClickDualPlayButton(){
            GameManager.Instance.ChangeToGameScene(Constants.GameType.DualPlay);
        }

        public void onClickSettingsButton(){
            GameManager.Instance.OpenSettingsPanel();
        }

    }

}