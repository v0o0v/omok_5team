using System;
using System.Linq;
using System.Reflection;
using Game;
using TMPro;
using UnityEngine;

namespace AvatarSelection
{
    public class AvatarSelectionPanel : MonoBehaviour
    {
        [SerializeField]
        private int _index;
        [SerializeField]
        private TMP_Text _nameText;
        [SerializeField]
        private GameObject _nextButton;
        [SerializeField]
        private GameObject _prevButton;

        [SerializeField]
        private AvatarContainer[] _avatarContainers;

        [SerializeField] 
        private GameObject otherPanel;

        [Serializable]
        private class AvatarContainer
        {
            [SerializeField]
            public string Name;

            /// <summary>
            /// The unique identifier for the avatar. Use constants defined in the <see cref="AvatarID"/> class.
            /// </summary>
            [SerializeField]
            public string ID;

            [SerializeField]
            public Game.Avatar Avatar;
        }

        /// <summary>
        /// Switches to the next avatar in the list. Wraps around to the first if at the end.
        /// </summary>
        public void OnNextButtonClicked()
        {
            int checkIdx = CheckDuplicate();
            _index++;
            if(checkIdx !=_avatarContainers.Length && checkIdx == _index) _index++;
            if(checkIdx !=0) {
                if (_index >= _avatarContainers.Length)
                    _index = 0;
            } else if (_index >= _avatarContainers.Length)
            {
                _index = 1 ;
            }

            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            ShowAvatar(_index);
        }

        /// <summary>
        /// Switches to the previous avatar in the list. Wraps around to the last if at the start.
        /// </summary>
        public void OnPrevButtonClicked()
        {
            int checkIdx = CheckDuplicate();
            _index--;
            if(checkIdx-1 !=0 && checkIdx == _index) _index--;
            if (_index < 0)
                _index = _avatarContainers.Length - 1;

            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            ShowAvatar(_index);
        }

        // 상대방 아바타와 중복 선택이 안되게 처리 - [leomanic] 
        // [AvatarSelectionPanel_Left]<->[AvatarSelectionPanel_Right] 서로 비교
        public int CheckDuplicate()
        {
            // 1. 상대방 패널 게임오브젝트에서 '스크립트'를 먼저 가져옵니다.
            AvatarSelectionPanel otherPanelScript = otherPanel.GetComponent<AvatarSelectionPanel>();

            if (otherPanelScript != null)
            {
                // 2. 해당 스크립트 내의 배열에 접근합니다.
                // (단, _avatarContainers가 public이거나 public 프로퍼티가 있어야 합니다.)
                var containers = otherPanelScript._avatarContainers; 
                var count = containers.Length < _avatarContainers.Length ? containers.Length: _avatarContainers.Length;
                for (int i = 0; i < count; i++) {
                    if(containers[i].Avatar.gameObject.activeSelf)
                    {
                        // Debug.Log("other avatar idx = "+ i);
                        return i;
                    }
                }
            }
            return -1;      
        }

        /// <summary>
        /// Activates the selected avatar, plays its idle animation, and updates the displayed name text.
        /// </summary>
        /// <param name="index">The index of the avatar container to display.</param>
        private void ShowAvatar(int index)
        {
            for (int i = 0; i < _avatarContainers.Length; i++)
                _avatarContainers[i].Avatar.gameObject.SetActive(i == index);

            _avatarContainers[index].Avatar.PlayAnimation(AvatarAnimationID.Wait);
            // _nameText.text = _avatarAContainers[index].Name;
            _nameText.text = ((Omok.AVATAR_NAME)(index)).ToString();
        }

        public string GetSelectedAvatarID()
        {
            return _avatarContainers[_index].ID;
        }

        public void SetAvatar(string avatarID)
        {
            for (var index = 0; index < _avatarContainers.Count(); index++)
            {
                var container = _avatarContainers[index];
                if (container.ID != avatarID)
                    continue;

                _index = index;
                ShowAvatar(index);
                return;
            }
        }

        public void DisableSelection()
        {
            _nextButton.gameObject.SetActive(false);
            _prevButton.gameObject.SetActive(false);
        }
    }
}
