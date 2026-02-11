using System;
using TMPro;
using UnityEngine;

namespace Tictactoe {

    public class ConfirmPanelController : PanelController {

        [SerializeField] private TMP_Text messageText;
        private Action _onConfirmButtonClicked;

        public void Show(string message, Action onConfirmButtonClick = null){
            this._onConfirmButtonClicked = onConfirmButtonClick;
            messageText.text = message;
            base.Show();
        }

        public void OnClickCloseButton(){
            Hide(() => { _onConfirmButtonClicked?.Invoke(); });
        }

    }

}