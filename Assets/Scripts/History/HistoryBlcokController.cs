using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using static Omok.Constants;

namespace Omok
{

    public class HistoryBlockController : MonoBehaviour
    {

        [SerializeField] private float xOffset = 0.45f;
        [SerializeField] private float yOffset = 0.45f;
        [SerializeField] private GameObject blockPrefab, historyPanel;
        [SerializeField] private GameObject contentPanel;

        private PlayerType[,] _board = new PlayerType[BOARD_SIZE, BOARD_SIZE];
        private int index = 0;
        private HistorySheet historySheet;
        private List<GameObject> placedBlocks = new List<GameObject>();
        private ObjectPool<GameObject> blockPool;
        private HistoryPanel lastSelectedPanel;

        private void Awake()
        {
            blockPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(blockPrefab, transform),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: true,
                defaultCapacity: 50,
                maxSize: BOARD_SIZE * BOARD_SIZE
            );
        }

        private void Start()
        {
            List<string> historyFiles = HistoryManager.GetHistoryFiles();
            
            historyFiles = HistoryManager.GetHistoryFiles()
                .OrderByDescending(f => File.GetLastWriteTime(f))
                .ToList();
            foreach (var historyFile in historyFiles)
            {
                var hs = HistoryManager.Load<HistorySheet>(historyFile);
                var instantiate = Instantiate(historyPanel, contentPanel.transform);
                HistoryPanel panelComponent = instantiate.GetComponent<HistoryPanel>();
                panelComponent.Init(hs);
                instantiate.GetComponent<Button>().onClick.AddListener(() =>
                {
                    lastSelectedPanel?.setOutline(false);
                    ClearBoard();
                    historySheet = hs;
                    lastSelectedPanel = instantiate.GetComponent<HistoryPanel>();
                    lastSelectedPanel?.setOutline(true);
                });
            }

            
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

        public void ProceedOneBackStep()
        {
            if (index <= 0) return;
            index--;
            GameObject lastBlock = placedBlocks[placedBlocks.Count - 1];
            placedBlocks.RemoveAt(placedBlocks.Count - 1);
            blockPool.Release(lastBlock);
        }

        public void ProceedAllBackStep()
        {
            while (index > 0)
            {
                ProceedOneBackStep();
            }
        }

        public void PlaceMarker(int x, int y, PlayerType playerType)
        {
            GameObject block = blockPool.Get();
            block.transform.localPosition = new Vector3(x * xOffset, y * yOffset * -1, 0);
            HistoryBlock blockComponent = block.GetComponent<HistoryBlock>();
            blockComponent.InitMarker(x, y, playerType, index + 1);
            placedBlocks.Add(block);
        }
        
        public void ClearBoard()
        {
            foreach (var block in placedBlocks)
            {
                blockPool.Release(block);
            }
            placedBlocks.Clear();
            index = 0;
        }

    }

}