using UnityEngine;
using static Omok.Constants;

namespace Omok {

    public class MainPanelController : MonoBehaviour {

        void Start()
        {
            SoundManager.instance.PlayBGM(Enum_Bgm.TITLE);
        }

        // 싱글 플레이
        public void onClickSinglePlayButton(){
            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            GameManager.Instance.ChangeToGameScene(Constants.GameType.SinglePlay);
        }

        // 2인 플레이
        public void onClickDualPlayButton(){
            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            GameManager.Instance.ChangeToGameScene(Constants.GameType.DualPlay);
        }

        // 기보 보기
        public void onClickGameRecordButton(){
            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            GameManager.Instance.ChangeToHistory();
        }

        // 설정 팝업
        public void onClickSettingsButton(){
            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            GameManager.Instance.OpenSettingsPanel();
        }
    }
}