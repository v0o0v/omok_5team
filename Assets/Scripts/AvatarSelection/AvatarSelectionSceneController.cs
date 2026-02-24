using System;
using System.Collections.Generic;
using Omok;
using UnityEngine;
using static Omok.Constants;

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
            var avatarIDs = new Dictionary<PlayerType, string>();
            foreach (var entry in _panelContainers)
                avatarIDs[entry.PlayerType] = entry.AvatarSelectionPanel.GetSelectedAvatarID();

            GameManager.Instance.ChangeToGameScene(avatarIDs);
        }
    }
}
