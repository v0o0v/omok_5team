using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Omok.Block;
using static Omok.Constants;

namespace Omok {

    public class BlockController : MonoBehaviour {

        [SerializeField] private float xOffset = 0.45f;
        [SerializeField] private float yOffset = 0.45f;
        [SerializeField] private GameObject blockPrefab;
        private int i = 0, j = 0;
        public Action<int, int> onBlockClicked;
        // public Dictionary<string, Block> blockDictionary = new();

        public void PlaceMarker(int x, int y, Constants.PlayerType playerType){
            GameObject block = Instantiate(blockPrefab, transform);
            block.transform.localPosition = new Vector3(x * xOffset, y * yOffset * -1, 0);
            Block blockComponent = block.GetComponent<Block>();
            // blockDictionary.Add(""+x+"_"+y, blockComponent);
            blockComponent.InitMarker(x, y, playerType);
        }
        
        private void OnMouseUpAsButton(){
            if (EventSystem.current.IsPointerOverGameObject()){
                return;
            }
            
            // 마우스 월드 좌표 가져오기
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // 로컬 좌표로 변환
            Vector3 localPos = transform.InverseTransformPoint(mouseWorldPos);
            
            // 바둑판 격자 위치 계산
            int x = Mathf.RoundToInt(localPos.x / xOffset);
            int y = Mathf.RoundToInt(localPos.y / yOffset) * -1;
            onBlockClicked?.Invoke(x, y);
        }

    }

}