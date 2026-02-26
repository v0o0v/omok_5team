using UnityEngine;

namespace Omok
{

    public class HistoryGamePanelController : MonoBehaviour
    {

        private void Start()
        {
            SoundManager.instance.PlayBGM(Enum_Bgm.GAME_BACKGOUND);
        }

        public void OnClickBackButton()
        {
            GameManager.Instance.OpenConfirmPanel("기보 보기를 종료합니다", () => { GameManager.Instance.ChangeToMainScene(); });
        }

        public void OnClickSettingsButton()
        {
            GameManager.Instance.OpenSettingsPanel();
        }

    }

}