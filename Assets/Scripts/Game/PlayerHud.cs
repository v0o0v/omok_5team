using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Omok
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar _progressBar;
        private Timer _timer;

        public void Activate(bool value)
        {
            gameObject.SetActive(value);
            _timer = GameManager.Instance.GetTimer();
        }

        public void UpdateTimerGauge(float limitTime, float maxTime)
        {
            _progressBar.SetValue(limitTime, maxTime);
        }

        private void Update()
        {
            if (_timer.IsRunning(0))
                _progressBar.SetValue(_timer.GetRemainTime(0), 30);
        }
    }
}
