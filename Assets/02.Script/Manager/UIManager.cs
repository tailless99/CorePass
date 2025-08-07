using UnityEngine;
using static GameEndContainer;

public class UIManager : Singleton<UIManager> {
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;
    [SerializeField] FeverContainer feverContainer;
    [SerializeField] GameEndContainer gameEndContainer;
    [SerializeField] ResumeProductionContainer resumeProductionContainer;

    private void Start() {
        // 이벤트 등록
        EventBusManager.Instance.SubscribeOnPlayerDeathEvent(() => ShowGameEndView(true)); // 플레이어가 죽을 때 이벤트
        EventBusManager.Instance.SubscribeOnGameClearEvent(() => ShowGameEndView(false)); // 게임 클리어 이벤트
        EventBusManager.Instance.SubscribeOnRestartAnimationStarted(() => StartRestartEffect()); // 재시작 연출 시작 이벤트
        EventBusManager.Instance.SubscribeOnRestartAnimationFinished(() => EndProduction()); // 재시작 연출 종료 이벤트
        EventBusManager.Instance.SubscribeOnGameOver_ResetUI(() => ResetScore()); // 게임 오버 - 초기화 이벤트
        EventBusManager.Instance.SubscribeOnGameOver_ResetUI(() => ResetFever()); // 게임 오버 - 초기화 이벤트
        EventBusManager.Instance.SubscribeOnFeverTimeStarted(() => SetFiverState(true)); // 피버 시작 이벤트
        EventBusManager.Instance.SubscribeOnFeverTimeFinished(() => SetFiverState(false)); // 피버 종료 이벤트

    }

    // 기능 처리
    // 점수 컨테이너
    public void AddScore(int score) => scoreContainer.AddScore(score); // 점수 추가
    public void ResetScore() => scoreContainer.ResetScore(); // 점수 추가
    public int GetScore() => scoreContainer.GetScore(); // 점수 반환
    public void SetFiverState(bool isFeverde) => scoreContainer.SetFiverState(isFeverde); // 피버 상태 토글

    // 시간 컨테이너
    public void UpdateClock(float time) => clockContainer.UpdateClock(time); // 시간 갱신
    public float GetMaxGameTime() => clockContainer.GetMaxGameTime(); // 최대 게임 시간 반환

    // 피버 컨테이너
    public void AddFeverPoint(int addPoint) => feverContainer.AddFeverPoint(addPoint); // 피버 포인트 추가
    public void ResetFever() => feverContainer.ResetFever(); // 피버 초기화
    
    // 게임 엔딩 컨테이너
    private void ShowGameEndView(bool isGameOver) { // 게임 엔딩 뷰 활성화
        var gameEndType = isGameOver ? GameEndType.GameOver : GameEndType.GameClear;
        gameEndContainer.gameObject.SetActive(true);
        gameEndContainer.GameEndInitialize(gameEndType);
    }

    // resumeProduction 컨테이너
    private void StartRestartEffect() => resumeProductionContainer.gameObject.SetActive(true);
    private void EndProduction() => resumeProductionContainer.EndProduction();
}
