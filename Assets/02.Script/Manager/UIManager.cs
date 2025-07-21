using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;


    // 기능 처리
    public void AddScore(int score) => scoreContainer.AddScore(score); // 점수 추가
    public void UpdateClock(float time) => clockContainer.UpdateClock(time); // 시간 갱신
}
