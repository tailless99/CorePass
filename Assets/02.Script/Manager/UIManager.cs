using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;
    [SerializeField] FeverContainer feverContainer;


    // 기능 처리
    public void AddScore(int score) => scoreContainer.AddScore(score); // 점수 추가
    public void SetFiverState(bool isFeverde) => scoreContainer.SetFiverState(isFeverde); // 피버 상태 토글
    public void UpdateClock(float time) => clockContainer.UpdateClock(time); // 시간 갱신
    public void AddFeverPoint(int addPoint) => feverContainer.AddFeverPoint(addPoint); // 피버 포인트 ㅜ가
}
