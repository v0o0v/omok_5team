using System;
using TMPro;
using UnityEngine;

namespace Omok {

    public class ConfirmPanelController : PanelController {

        [SerializeField] private TMP_Text messageText;
        [SerializeField] private GameObject buttonLaytout;
        private Action _onConfirmButtonClicked;
        private Action _onContinueButtonClicked;

        public void Show(string message, Action onConfirmButtonClick = null){
            this._onConfirmButtonClicked = onConfirmButtonClick;
            messageText.text = message;
            base.Show();
        }
        public void Show(string message, Action onConfirmButtonClick = null, Action onContinueButtonClick = null){
            this._onConfirmButtonClicked = onConfirmButtonClick;
            this._onContinueButtonClicked = onContinueButtonClick;
            buttonLaytout.SetActive(true);
            messageText.text = message;
            base.Show();
        }

        public void OnClickCloseButton(){
            Hide(() => { _onConfirmButtonClicked?.Invoke(); });
        }
        public void OnClickContinueButton(){
            Hide(() => { _onContinueButtonClicked?.Invoke(); });
        }

    }

}