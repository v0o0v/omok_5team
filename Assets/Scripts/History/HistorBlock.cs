using System;
using TMPro;
using UnityEngine;
using static Omok.Constants;

namespace Omok
{

    public class HistoryBlock : MonoBehaviour
    {

        [SerializeField] private Sprite blackStoneSprite;
        [SerializeField] private Sprite whiteStoneSprite;
        [SerializeField] TextMeshProUGUI countText;
        private SpriteRenderer spriteRenderer;
        private int _x;
        private int _y;

        private Action<int, int> _onBlockClicked;
        private SoundManager sound;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sound = FindFirstObjectByType<SoundManager>();
        }

        public void InitMarker(int x, int y, PlayerType playerType, int count)
        {
            _x = x;
            _y = y;
            SetMarker(playerType);
            // SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE2);
            countText.text = count.ToString();
            countText.color = playerType == PlayerType.Player1 ? Color.white : Color.black;
        }

        public void SetMarker(PlayerType markerType)
        {
            switch (markerType)
            {
                case PlayerType.Player1:
                    spriteRenderer.sprite = blackStoneSprite;
                    break;
                case PlayerType.Player2:
                    spriteRenderer.sprite = whiteStoneSprite;
                    break;
                case PlayerType.None:
                    spriteRenderer.sprite = null;
                    break;
            }
        }

    }

}