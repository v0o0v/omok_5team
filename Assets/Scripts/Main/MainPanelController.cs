using UnityEngine;
using static Omok.Constants;

namespace Omok {

    public class MainPanelController : MonoBehaviour {

        // 싱글 플레이
        public void onClickSinglePlayButton(){
            GameManager.Instance.ChangeToGameScene(Constants.GameType.SinglePlay);
        }

        // 2인 플레이
        public void onClickDualPlayButton(){
            GameManager.Instance.ChangeToGameScene(Constants.GameType.DualPlay);
        }

        // 기보 보기
        public void onClickGameRecordButton(){
            GameManager.Instance.ChangeToHistory();
        }

        // 설정 팝업
        public void onClickSettingsButton(){
            GameManager.Instance.OpenSettingsPanel();
        }
    }
}