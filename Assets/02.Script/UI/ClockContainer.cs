using TMPro;
using UnityEngine;

public class ClockContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private void Awake() {
        // �ʱ�ȭ
        UpdateClock(0);
    }

    public void UpdateClock(float time) {
        timeText.text = "Time : " + time.ToString("F0");
    }
}
