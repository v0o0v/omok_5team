using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omok {

    public class Block : MonoBehaviour {

        [SerializeField] private Sprite blackStoneSprite;
        [SerializeField] private Sprite whiteStoneSprite;
        private SpriteRenderer spriteRenderer;

        public enum MarkerType { None, Black, White }

        private int _x;
        private int _y;

        private Action<int, int> _onBlockClicked;
        
        private void Awake(){
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void InitMarker(int x, int y, Constants.PlayerType playerType){
            _x = x;
            _y = y;
            SetMarker(playerType);
            // _onBlockClicked = onBlockClicked;
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

        // private void OnMouseUpAsButton(){
        //     if (EventSystem.current.IsPointerOverGameObject()){
        //         return;
        //     }
        //     _onBlockClicked?.Invoke(_x, _y);
        // }

    }

}