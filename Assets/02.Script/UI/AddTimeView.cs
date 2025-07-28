using TMPro;
using UnityEngine;

public class AddTimeView : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI addTimeText;
    [SerializeField] private float addTime = 10; // 기존 시간에 더할 수치
    [SerializeField] private float addTimeDelay = 1.5f; // 시간이 다 추가될 때까지 걸리는 시간

    private bool isAddTimeEvent; // 시간 증가 이벤트 실행 플래그
    private float gameTime; // 목표 시간
    private float currGameTime; // 현재 시간
    private float startLerpTime;

    private ResumeProductionContainer parentClass;

    private void Awake() {
        parentClass = transform.parent.GetComponent<ResumeProductionContainer>();
    }

    private void OnEnable() {
        addTimeText.text = $"+ {addTime.ToString()} Sec";
        currGameTime = GameManager.Instance.GetGameTime(); // 현재 시간

        var maxTime = UIManager.Instance.GetMaxGameTime();
        gameTime = Mathf.Min(currGameTime + addTime, maxTime); // 목표 시간
        isAddTimeEvent = false;
    }

    private void Update() {
        if (isAddTimeEvent) {
            float timeElapsed = Time.time - startLerpTime;
            float percentageComplete = timeElapsed / addTimeDelay;

            currGameTime = Mathf.Lerp(currGameTime, gameTime, Mathf.Clamp01(percentageComplete));

            // 시간 업데이트
            UIManager.Instance.UpdateClock(currGameTime);

            // 시간이 목표치에 도달했는지 확인
            if (currGameTime >= gameTime) {
                isAddTimeEvent = false; // 중복 실행 방지
                parentClass.NextProdectStart(); // 다음 연출 시작
            }
        }
    }

    // 애니메이션으로 실행되는 시간 추가 기능
    private void StartProdect() {
        if (!isAddTimeEvent) // 이미 진행 중이 아니라면 시작
        {
            startLerpTime = Time.time; // Lerp 시작 시간 기록
            isAddTimeEvent = true;
        }
    }
}
