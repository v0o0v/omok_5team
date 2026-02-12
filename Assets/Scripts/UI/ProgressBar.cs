using UnityEngine;
using UnityEngine.UI;

namespace Omok
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider progressSlider;

        public void SetValue(float current, float max)
        {
            if (progressSlider == null) return;

            progressSlider.minValue = 0;
            progressSlider.maxValue = max;

            progressSlider.value = current;
        }
    }
}
