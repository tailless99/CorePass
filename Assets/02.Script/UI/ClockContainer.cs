using TMPro;
using UnityEngine;

public class ClockContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float maxGameTime = 120; // 게임 제한 시간

    private void Awake() {
        // 초기화
        UpdateClock(0);
    }

    public void UpdateClock(float time) {
        var timeTemp = Mathf.Clamp(time, 0, maxGameTime);
        timeText.text = timeTemp.ToString("F0");
    }

    public float GetMaxGameTime() => maxGameTime;
}
