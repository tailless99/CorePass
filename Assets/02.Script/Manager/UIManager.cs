using System;
using UnityEngine;
using static GameEndContainer;

public class UIManager : Singleton<UIManager> {
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;
    [SerializeField] FeverContainer feverContainer;
    [SerializeField] GameEndContainer gameEndContainer;
    [SerializeField] ResumeProductionContainer resumeProductionContainer;

    private void Start() {

        #region �̺�Ʈ ���
        // �÷��̾� ��� �̺�Ʈ
        EventBusManager.Instance.Subscribe<PlayerDeathEvent>(_=> ShowGameEndView(true)); // �÷��̾ ���� �� �̺�Ʈ

        // ���� Ŭ���� �̺�Ʈ
        EventBusManager.Instance.Subscribe<GameClearEvent>(_ => ShowGameEndView(false)); // ���� Ŭ���� �̺�Ʈ

        // ���� �ٽ� ���� �̺�Ʈ
        EventBusManager.Instance.Subscribe<RestartAnimationStartedEvent>(_ => StartRestartEffect()); // ����� ���� ���� �̺�Ʈ
        EventBusManager.Instance.Subscribe<RestartAnimationFinishedEvent>(_ => EndProduction()); // ����� ���� ���� �̺�Ʈ

        // ���� ���� �̺�Ʈ
        EventBusManager.Instance.Subscribe<GameOver_ResetUIEvent>(_ => ResetScore()); // ���� ���� - �ʱ�ȭ �̺�Ʈ
        EventBusManager.Instance.Subscribe<GameOver_ResetUIEvent>(_ => ResetFever()); // ���� ���� - �ʱ�ȭ �̺�Ʈ

        // �ǹ� �̺�Ʈ
        EventBusManager.Instance.Subscribe<FeverTimeStartedEvent>(_=> SetFiverState(true)); // �ǹ� ���� �̺�Ʈ
        EventBusManager.Instance.Subscribe<FeverTimeFinishedEvent>(_ => SetFiverState(false)); // �ǹ� ���� �̺�Ʈ

        // ����/�ǹ� ���� �߰� �̺�Ʈ
        EventBusManager.Instance.Subscribe<AddScoreEvent>((e) => AddScore(e.Score)); // �ǹ� ���� �̺�Ʈ
        EventBusManager.Instance.Subscribe<AddFeverPointEvent>((e) => AddFeverPoint(e.Score)); // �ǹ� ���� �̺�Ʈ
        #endregion

    }

    // ��� ó��
    // ���� �����̳�
    public void AddScore(int score) => scoreContainer.AddScore(score); // ���� �߰�
    public void ResetScore() => scoreContainer.ResetScore(); // ���� �߰�
    public int GetScore() => scoreContainer.GetScore(); // ���� ��ȯ
    public void SetFiverState(bool isFeverde) => scoreContainer.SetFiverState(isFeverde); // �ǹ� ���� ���

    // �ð� �����̳�
    public void UpdateClock(float time) => clockContainer.UpdateClock(time); // �ð� ����
    public float GetMaxGameTime() => clockContainer.GetMaxGameTime(); // �ִ� ���� �ð� ��ȯ

    // �ǹ� �����̳�
    public void AddFeverPoint(int addPoint) => feverContainer.AddFeverPoint(addPoint); // �ǹ� ����Ʈ �߰�
    public void ResetFever() => feverContainer.ResetFever(); // �ǹ� �ʱ�ȭ
    
    // ���� ���� �����̳�
    private void ShowGameEndView(bool isGameOver) { // ���� ���� �� Ȱ��ȭ
        var gameEndType = isGameOver ? GameEndType.GameOver : GameEndType.GameClear;
        gameEndContainer.gameObject.SetActive(true);
        gameEndContainer.GameEndInitialize(gameEndType);
    }

    // resumeProduction �����̳�
    private void StartRestartEffect() => resumeProductionContainer.gameObject.SetActive(true);
    private void EndProduction() => resumeProductionContainer.EndProduction();
}
