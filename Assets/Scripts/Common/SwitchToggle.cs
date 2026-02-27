using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common
{
    // Unity의 기본 Toggle과 유사한 이벤트를 제공하기 위해 Serializable 클래스 정의
    [System.Serializable]
    public class ToggleEvent : UnityEvent<bool> { }

    public class SwitchToggle : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI References")]
        [SerializeField] private RectTransform handle;
        [SerializeField] private Image backgroundImage;

        [Header("Settings")]
        public bool isOn = false; // 외부에서 접근 가능하도록 public
        [SerializeField] private float transitionSpeed = 0.15f;

        [Header("Colors")]
        [SerializeField] private Color onColor = new Color(0.192f, 0.820f, 0.345f); // iOS Green
        [SerializeField] private Color offColor = new Color(0.898f, 0.898f, 0.918f); // iOS Gray

        [Header("Events")]
        // Unity 기본 Toggle의 onValueChanged와 동일한 역할
        public ToggleEvent onValueChanged;

        private float _moveRange;
        private float _t;

        void Start()
        {
            // 배경 너비와 핸들 너비를 기준으로 이동 범위 계산
            float backgroundWidth = GetComponent<RectTransform>().rect.width;
            float handleWidth = handle.rect.width;

            // 여백을 고려한 핸들 이동 반경 (중앙 기준)
            _moveRange = (backgroundWidth - handleWidth) * 0.5f - 4f;

            // 초기 시각적 상태 설정 (애니메이션 없이 즉시 반영)
            _t = isOn ? 1f : 0f;
            UpdateVisuals(_t);
        }

        void Update()
        {
            // 목표 상태(0 또는 1)로 부드럽게 이동
            float target = isOn ? 1f : 0f;
            _t = Mathf.MoveTowards(_t, target, Time.deltaTime / transitionSpeed);

            UpdateVisuals(_t);
        }

        private void UpdateVisuals(float progress)
        {
            // 위치 보간 (SmoothStep을 사용해 더 부드럽게 연출)
            float smoothProgress = Mathf.SmoothStep(0, 1, progress);
            float posX = Mathf.Lerp(-_moveRange, _moveRange, smoothProgress);
            handle.anchoredPosition = new Vector2(posX, 0);

            // 색상 보간
            backgroundImage.color = Color.Lerp(offColor, onColor, smoothProgress);
        }

        // 클릭 시 호출
        public void OnPointerClick(PointerEventData eventData)
        {
            Toggle();
        }

        private void Toggle()
        {
            isOn = !isOn;

            // 중요: Unity의 Toggle.onValueChanged와 동일하게 이벤트 호출
            if (onValueChanged != null)
            {
                onValueChanged.Invoke(isOn);
            }
        }

        // 외부 스크립트에서 코드로 값을 바꿀 때 사용하는 함수
        public void SetState(bool value)
        {
            if (isOn != value)
            {
                isOn = value;
                onValueChanged.Invoke(isOn);
            }
        }
    }
}