using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeverContainer : MonoBehaviour {
    [Header("Fever Setting")]
    [SerializeField] private Slider slider;
    [SerializeField] private int maxFeverPoint = 15000;
    [SerializeField] private float maxFiverTime = 10f; // 피버 유지 시간

    [Header("Fever 연출")]
    [SerializeField] private GameObject feverText;
    [SerializeField] private GameObject feverBG;


    private int curFeverPoint; // 현재 피버 포인트
    private float currFiverTimer; // 피버 시간을 재는 타이머
    private bool isFevering;


    private void Awake() {
        curFeverPoint = 0;
        slider.value = 0;
    }

    private void OnEnable() {
        feverText.SetActive(false);
        feverBG.SetActive(false);
    }

    private void Update() {
        // 피버 타임 로직
        StartFeverTime();
    }

    // 피버 포인트를 추가하는 기능
    public void AddFeverPoint(int addPoint) {
        if (isFevering) return; // 피버 중엔 차지 않음

        // 최대, 최소값 제한
        curFeverPoint = Mathf.Clamp(curFeverPoint + addPoint, 0, maxFeverPoint);

        // 슬라이더에 값 반영
        slider.value = (float)curFeverPoint / (float)maxFeverPoint;

        // 피버 포인트가 다찼다면 피버 상태 돌입 플래그
        if (curFeverPoint >= maxFeverPoint) {
            curFeverPoint = 0; // 초기화
            isFevering = true;
        }
    }


    // 피버 게이지 초기화
    public void ResetFever() {
        curFeverPoint = 0;
        slider.value = 0;
    }

    // 피버 타임 로직
    public void StartFeverTime() {
        // 예외 처리
        if (!isFevering) return;

        // 피버 종료 조건
        if (currFiverTimer >= maxFiverTime) {
            isFevering = false;
            currFiverTimer = 0;
            FeverDirection(isFevering); // 피버 효과 On/Off
            return;
        }

        FeverDirection(isFevering); // 피버 효과 On/Off
        // 타이머 시간 추가
        currFiverTimer += Time.deltaTime;

        // 피버 게이지 감소 로직
        float normalizedTime = currFiverTimer / maxFiverTime; // 0.0 ~ 1.0 사이 값
        slider.value = Mathf.Lerp(1.0f, 0.0f, normalizedTime);
    }

    // 피버 효과 On/Off
    private void FeverDirection(bool isFever) {
        // 피버 전용 UI On/Off
        feverText.SetActive(isFever);
        feverBG.SetActive(isFever);

        // 피버 효과 On/Off
        UIManager.Instance.SetFiverState(isFever); // 점수 2배 활성화
        GameManager.Instance.SetFeverState(isFever); // 콜라이더 활성/비활성화
    }
}
