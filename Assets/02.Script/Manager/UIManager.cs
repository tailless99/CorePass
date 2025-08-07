using UnityEngine;
using static GameEndContainer;

public class UIManager : Singleton<UIManager> {
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;
    [SerializeField] FeverContainer feverContainer;
    [SerializeField] GameEndContainer gameEndContainer;
    [SerializeField] ResumeProductionContainer resumeProductionContainer;

    private void Start() {
        // �̺�Ʈ ���
        EventBusManager.Instance.SubscribeOnPlayerDeathEvent(() => ShowGameEndView(true)); // �÷��̾ ���� �� �̺�Ʈ
        EventBusManager.Instance.SubscribeOnGameClearEvent(() => ShowGameEndView(false)); // ���� Ŭ���� �̺�Ʈ
        EventBusManager.Instance.SubscribeOnRestartAnimationStarted(() => StartRestartEffect()); // ����� ���� ���� �̺�Ʈ
        EventBusManager.Instance.SubscribeOnRestartAnimationFinished(() => EndProduction()); // ����� ���� ���� �̺�Ʈ
        EventBusManager.Instance.SubscribeOnGameOver_ResetUI(() => ResetScore()); // ���� ���� - �ʱ�ȭ �̺�Ʈ
        EventBusManager.Instance.SubscribeOnGameOver_ResetUI(() => ResetFever()); // ���� ���� - �ʱ�ȭ �̺�Ʈ
        EventBusManager.Instance.SubscribeOnFeverTimeStarted(() => SetFiverState(true)); // �ǹ� ���� �̺�Ʈ
        EventBusManager.Instance.SubscribeOnFeverTimeFinished(() => SetFiverState(false)); // �ǹ� ���� �̺�Ʈ

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
