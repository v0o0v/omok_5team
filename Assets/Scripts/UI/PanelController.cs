using System;
using DG.Tweening;
using UnityEngine;

namespace Tictactoe {

    [RequireComponent(typeof(CanvasGroup))]
    public class PanelController : MonoBehaviour {

        [SerializeField] private RectTransform panelTransform;

        private CanvasGroup _canvasGroup;

        private void Awake(){
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show(){
            _canvasGroup.alpha = 0;
            panelTransform.localScale = Vector3.zero;

            _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.Linear);
            panelTransform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void Hide(Action onComplete = null){
            _canvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear);
            panelTransform.DOScale(0, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => {
                    onComplete?.Invoke();
                    Destroy(gameObject);
                });
        }

    }

}