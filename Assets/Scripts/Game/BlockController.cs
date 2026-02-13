using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static Omok.Block;
using static Omok.Constants;

namespace Omok
{

    public class BlockController : MonoBehaviour
    {

        [SerializeField] private float xOffset = 0.45f;
        [SerializeField] private float yOffset = 0.45f;
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private GameObject forbiddenMarkPrefab;
        [SerializeField] private GameObject cursorPrefab;
        private GameObject prevCursor, _cursor;
        private int _x = 7, _y = 7;
        public Action<int, int> onBlockClicked;
        // public Dictionary<string, Block> blockDictionary = new();
        private List<GameObject> _forbiddenMarks = new();
        private List<Vector2> _forbiddenPositions = new();

        private PlayerType[,] _board;
        private PlayerType currPlayerType = PlayerType.None;

        public void initBoard(PlayerType[,] board = null)
        {
            _board = board;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                _y--;
                if(_y < 0) _y = BOARD_SIZE-1;
                MarkCursor(_x,_y);            
            } else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _y++;
                if(_y > BOARD_SIZE-1) _y = 0;
                MarkCursor(_x,_y);                
            } else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {          
                _x--;
                if(_x < 0) _x = BOARD_SIZE-1;      
                MarkCursor(_x,_y);   
            } else if(Input.GetKeyDown(KeyCode.RightArrow))
            {        
                _x++;
                if(_x > BOARD_SIZE-1) _x = 0;        
                MarkCursor(_x,_y);   
            } else if(Input.GetKeyDown(KeyCode.Space))
            {
                // 빈자리가 아니면 return
                if (_board[_x, _y] != PlayerType.None || IsForbiddenPosition(_x, _y))
                    return;
                    currPlayerType = (currPlayerType == PlayerType.Player1) ? PlayerType.Player2 :  PlayerType.Player1;
                    PlaceMarker(_x, _y, currPlayerType);                
            }
        }

        // Cursor 표시
        private void MarkCursor(int x, int y) {
            if(prevCursor !=null ) {
                Destroy(prevCursor);    // 이전 커서가 있다면 삭제함                
            } 
            _cursor = Instantiate(cursorPrefab, transform);
            _cursor.transform.localPosition = new Vector3(x * xOffset, y * (yOffset-0.002f) * -1, 0);
            prevCursor = _cursor;   // 이전 커서에 저장해둠
        }

        public void PlaceMarker(int x, int y, Constants.PlayerType playerType)
        {
            Debug.Log($"Place Marker {x},{y}");
            GameObject block = Instantiate(blockPrefab, transform);

            if (prevCursor != null)
            {
                Destroy(prevCursor);    // 이전 커서가 있다면 삭제함                
            }
            MarkCursor(x,y);
            currPlayerType = playerType;

            block.transform.localPosition = new Vector3(x * xOffset, y * yOffset * -1, 0);
            Block blockComponent = block.GetComponent<Block>();
            // blockDictionary.Add(""+x+"_"+y, blockComponent);
            blockComponent.InitMarker(x, y, playerType);
        }

        public void PutForbiddenMark(int x, int y)
        {
            GameObject mark = Instantiate(forbiddenMarkPrefab, transform);
            mark.transform.localPosition = new Vector3(x * xOffset, y * yOffset * -1, 0);
            _forbiddenMarks.Add(mark);
            _forbiddenPositions.Add(new Vector2(x, y));
        }

        public void ClearForbiddenMarks()
        {
            foreach (var mark in _forbiddenMarks)
                Destroy(mark);
            _forbiddenMarks.Clear();
            _forbiddenPositions.Clear();
        }

        private void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // 마우스 월드 좌표 가져오기
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 로컬 좌표로 변환
            Vector3 localPos = transform.InverseTransformPoint(mouseWorldPos);

            // 바둑판 격자 위치 계산
            int x = Mathf.RoundToInt(localPos.x / xOffset);
            int y = Mathf.RoundToInt(localPos.y / yOffset) * -1;

            // 금지 위치 일경우 무시처리
            if (IsForbiddenPosition(x, y))
                return;

            onBlockClicked?.Invoke(x, y);
        }

        private bool IsForbiddenPosition(int x, int y)
        {
            return _forbiddenPositions.Any(entry => entry.x == x && entry.y == y);
        }
    }

}