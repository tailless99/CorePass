using TMPro;
using UnityEngine;

public class AddSocreTextContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] addScoreTextsUI;
    [SerializeField] private Color[] textColor; // 0 : Plus | 1 : Minus


    private void Start() {
        // 이벤트 구독
        EventBusManager.Instance.SubscribeOnAddScore((addScore) => AddScoreTextActive(addScore));
    }

    private void AddScoreTextActive(int addScore) {
        foreach(var addText in addScoreTextsUI) {
            if (!addText.gameObject.activeSelf) {
                var plusNum = addScore > 0;
                addText.text = plusNum ? $"+{addScore}" : $"{addScore}";
                addText.color = plusNum ? textColor[0] : textColor[1];
                addText.gameObject.SetActive(true);
                break;
            }
        }
    }
}
