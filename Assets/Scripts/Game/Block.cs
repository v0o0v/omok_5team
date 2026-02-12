using System;
using UnityEngine;

namespace Omok {

    public class Block : MonoBehaviour {

        [SerializeField] private Sprite blackStoneSprite;
        [SerializeField] private Sprite whiteStoneSprite;
        private SpriteRenderer spriteRenderer;
        private int _x;
        private int _y;

        private Action<int, int> _onBlockClicked;
        // 사운드 매니저 추가 - [leomanic]
        private SoundManager sound;

        private void Awake(){
            spriteRenderer = GetComponent<SpriteRenderer>();
            sound = FindFirstObjectByType<SoundManager>();
        }

        public void InitMarker(int x, int y, Constants.PlayerType playerType){
            _x = x;
            _y = y;
            SetMarker(playerType);
            Debug.Log("플레이 사운드");
            SoundManager.instance.PlaySFX(Enum_Sfx.PLACE_STONE2);
        }

        public void SetMarker(Constants.PlayerType markerType){
            switch (markerType){
                case Constants.PlayerType.Player1:
                    spriteRenderer.sprite = blackStoneSprite;
                    break;
                case Constants.PlayerType.Player2:
                    spriteRenderer.sprite = whiteStoneSprite;
                    break;
                case Constants.PlayerType.None:
                    spriteRenderer.sprite = null;
                    break;
            }
        }

    }

}