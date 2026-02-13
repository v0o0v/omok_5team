using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Avatar : MonoBehaviour
    {
        [SerializeField]
        private Sprite _avatarWin;
        [SerializeField]
        private Sprite _avatarLose;
        [SerializeField]
        private Sprite _avatarThink;
        [SerializeField]
        private Sprite _avatarWait;

        [SerializeField]
        private Image _avatarImage;

        private Dictionary<string, Sprite> _avatarSprites;

        private void Awake()
        {
            _avatarSprites = new Dictionary<string, Sprite>();
            _avatarSprites.Add(AvatarAnimationID.Wait, _avatarWait);
            _avatarSprites.Add(AvatarAnimationID.Win, _avatarWin);
            _avatarSprites.Add(AvatarAnimationID.Lose, _avatarLose);
            _avatarSprites.Add(AvatarAnimationID.Think, _avatarThink);
        }

        public void PlayAnimation(string animationID)
        {
            if (_avatarSprites.TryGetValue(animationID, out var sprite))
                _avatarImage.sprite = sprite;
        }

        public void SetDirection(Vector2 direction)
        {
            var scale = gameObject.transform.localScale;
            scale.x *= direction == Vector2.left ? -1f : 1f;
            gameObject.transform.localScale = scale;
        }
    }
}
