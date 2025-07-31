using TMPro;
using UnityEngine;

public class ClockContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float maxGameTime = 120; // ���� ���� �ð�

    private void Awake() {
        // �ʱ�ȭ
        UpdateClock(0);
    }

    public void UpdateClock(float time) {
        var timeTemp = Mathf.Clamp(time, 0, maxGameTime);
        timeText.text = timeTemp.ToString("F0");
    }

    public float GetMaxGameTime() => maxGameTime;
}
