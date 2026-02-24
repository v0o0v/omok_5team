using UnityEngine;

namespace Omok
{
    public class LocalDataStore : MonoBehaviour
    {
        private string _avatarID;
        private bool _isDirty = false;

        private void Awake()
        {
            Load();
        }

        private void Load()
        {
            _avatarID = PlayerPrefs.GetString("AvatarID", string.Empty);
        }

        public string GetAvatarID()
        {
            return _avatarID;
        }

        public void SetAvatarID(string avatarID)
        {
            if (_avatarID != avatarID)
            {
                _avatarID = avatarID;
                SetDirty();
            }
        }

        private void SetDirty()
        {
            _isDirty = true;
        }

        private void LateUpdate()
        {
            if (!_isDirty)
                return;

            SaveAllData();
            _isDirty = false;
        }

        private void SaveAllData()
        {
            PlayerPrefs.SetString("AvatarID", _avatarID);
            PlayerPrefs.Save();
        }
    }
}
