using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Omok
{
    public class SettingsPanelController : PanelController
    {
        private SoundManager _soundManager;

        [SerializeField]
        public SwitchToggle _sfxSwitchToggle;
        [SerializeField]
        public SwitchToggle _bgmSwitchToggle;
        [SerializeField]
        public Slider _bgmSlider;
        [SerializeField]
        public Slider _sfxSlider;

        private void Start()
        {
            _soundManager = SoundManager.instance;
            _sfxSwitchToggle.isOn = _soundManager.IsSFXEnabled();
            _bgmSwitchToggle.isOn = _soundManager.IsBGMEnabled();
            _bgmSlider.value = _soundManager.GetBGMVolume();
            _sfxSlider.value = _soundManager.GetSFXVolume();
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

        public void OnBGMValueChanged(float value)
        {
            _soundManager.SetBGMVolume(value);
        }

        public void OnSFXValueChanged(float value)
        {
            _soundManager.SetSFXVolume(value);
        }
    }
}