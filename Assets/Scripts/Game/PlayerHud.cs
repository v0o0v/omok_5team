using Game;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omok
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar _progressBar;
        [SerializeField]
        private Game.Avatar _avatar;
        [SerializeField]
        private TMP_Text _playerNameText;
        [SerializeField]
        private Sprite _blackStoneSprite;
        [SerializeField]
        private Sprite _whiteStoneSprite;
        [SerializeField]
        private Image _stoneImage;

        private Dictionary<AvatarState, Action> _avatarStateActions;

        public void Awake()
        {
            _avatarStateActions = new Dictionary<AvatarState, Action>();
            _avatarStateActions.Add(AvatarState.Wait, () =>
            {
                SetActiveTimerGauge(false);
                PlayAvatarAnimation(AvatarAnimationID.Wait);
            });
            _avatarStateActions.Add(AvatarState.Win, () =>
            {
                SetActiveTimerGauge(false);
                PlayAvatarAnimation(AvatarAnimationID.Win);
            });
            _avatarStateActions.Add(AvatarState.Lose, () =>
            {
                SetActiveTimerGauge(false);
                PlayAvatarAnimation(AvatarAnimationID.Lose);
            });
            _avatarStateActions.Add(AvatarState.Think, () =>
            {
                SetActiveTimerGauge(true);
                PlayAvatarAnimation(AvatarAnimationID.Think);
            });
        }

        public void SetPlayerName(string name)
        {
            _playerNameText.text = name;
        }

        public void SetPlayerStone(Constants.PlayerType playerType)
        {
            _stoneImage.sprite = playerType == Constants.PlayerType.Player1 ? _blackStoneSprite : _whiteStoneSprite;
        }

        public void SetAvatarState(AvatarState avatarState)
        {
            _avatarStateActions[avatarState]?.Invoke();
        }

        public void SetAvatarDirection(Vector2 direction)
        {
            _avatar.SetDirection(direction);
        }

        private void SetActiveTimerGauge(bool value)
        {
            _progressBar.gameObject.SetActive(value);
        }

        private void PlayAvatarAnimation(string animationID)
        {
            _avatar?.PlayAnimation(animationID);
        }

        private void Update()
        {
            if (GetTimer().IsRunning(0))
                _progressBar.SetValue(GetTimer().GetRemainTime(0), 30);
        }

        private Timer GetTimer()
        {
            return GameManager.Instance.GetTimer();
        }
    }
}
