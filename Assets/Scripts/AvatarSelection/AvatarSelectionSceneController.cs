using System;
using Omok;
using UnityEngine;

namespace AvatarSelection
{
    public class AvatarSelectionSceneController : MonoBehaviour
    {
        [SerializeField]
        private AvatarSelectionPanelContainer[] _panelContainers;

        [Serializable]
        private class AvatarSelectionPanelContainer
        {
            [SerializeField]
            public Constants.PlayerType PlayerType;
            [SerializeField]
            public AvatarSelectionPanel AvatarSelectionPanel;
        }

        public void OnStartButtonClicked()
        {
            GameManager.Instance.ChangeToGameScene();
        }
    }
}
