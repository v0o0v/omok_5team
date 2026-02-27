using Common;
using UnityEngine;

namespace Omok
{
    public class SettingsPanelController : PanelController
    {
        private SoundManager _soundManager;

        [SerializeField]
        public SwitchToggle _sfxSwitchToggle;
        [SerializeField]
        public SwitchToggle _bgmSwitchToggle;

        private void Start()
        {
            _soundManager = SoundManager.instance;
            _sfxSwitchToggle.isOn = _soundManager.IsSFXEnabled();
            _bgmSwitchToggle.isOn = _soundManager.IsBGMEnabled();
        }

        public void OnClickCloseButton()
        {
            base.Hide();
        }

        public void OnBGMToggleChanged(bool value)
        {
            _soundManager.EnableBGM(value);
        }

        public void OnSFXToggleChanged(bool value)
        {
            _soundManager.EnableSFX(value);
        }
    }
}