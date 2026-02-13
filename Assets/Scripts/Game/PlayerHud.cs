using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omok
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar _progressBar;
        [SerializeField]
        private Game.Avatar _avatar;
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

        public void SetAvatarState(AvatarState avatarState)
        {
            _avatarStateActions[avatarState]?.Invoke();
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
