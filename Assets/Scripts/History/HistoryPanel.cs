using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omok
{

    public class HistoryPanel : MonoBehaviour
    {

        [SerializeField] private TMP_Text dateTimeText;
        [SerializeField] private TMP_Text player1Text;
        [SerializeField] private TMP_Text player2Text;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private GameObject historyPanel;
        
        public void Init(HistorySheet historySheet)
        {
            dateTimeText.text = historySheet.dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            player1Text.text = $"{historySheet.player1}";
            player2Text.text = $"{historySheet.player2}";
            resultText.text = $"{historySheet.result}";
        }
        
        public void setOutline(bool show)
        {
            var outline = historyPanel.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = show;
            }
        }
    }

}