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
            _index++;
            if (_index >= _avatarContainers.Length)
                _index = 0;

            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            ShowAvatar(_index);
        }

        /// <summary>
        /// Switches to the previous avatar in the list. Wraps around to the last if at the start.
        /// </summary>
        public void OnPrevButtonClicked()
        {
            _index--;
            if (_index < 0)
                _index = _avatarContainers.Length - 1;

            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE3);
            ShowAvatar(_index);
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
            _nameText.text = _avatarContainers[index].Name;
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
