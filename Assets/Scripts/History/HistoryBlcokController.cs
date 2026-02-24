using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Omok.Constants;

namespace Omok
{

    public class HistoryBlockController : MonoBehaviour
    {

        [SerializeField] private float xOffset = 0.45f;
        [SerializeField] private float yOffset = 0.45f;
        [SerializeField] private GameObject blockPrefab;

        private PlayerType[,] _board = new PlayerType[BOARD_SIZE, BOARD_SIZE];
        private int index = 0;
        private HistorySheet historySheet; 

        private void Start()
        {
            List<string> historyFiles = HistoryManager.GetHistoryFiles();
            foreach (var historyFile in historyFiles){
                Debug.Log("History File: " + historyFile);
            }

            historySheet = HistoryManager.Load<HistorySheet>(historyFiles[0]);
        }
        
        public void ProceedOneStep()
        {
            if (index >= historySheet.moves.Count) return;
            Move move = historySheet.moves[index];
            PlaceMarker(move.X, move.Y, move.playerType);
            index++;
        }
        
        public void ProceedAllStep()
        {
            while (index < historySheet.moves.Count)
            {
                ProceedOneStep();
            }
        }

        public void PlaceMarker(int x, int y, PlayerType playerType)
        {
            GameObject block = Instantiate(blockPrefab, transform);
            block.transform.localPosition = new Vector3(x * xOffset, y * yOffset * -1, 0);
            HistoryBlock blockComponent = block.GetComponent<HistoryBlock>();
            blockComponent.InitMarker(x, y, playerType, index);
        }

    }

}