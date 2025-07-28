using UnityEngine;
using static GameEndContainer;

public class UIManager : Singleton<UIManager> {
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;
    [SerializeField] FeverContainer feverContainer;
    [SerializeField] GameEndContainer gameEndContainer;
    [SerializeField] ResumeProductionContainer resumeProductionContainer;


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
    
    // ���� ���� �����̳�
    public void ShowGameEndView(bool isGameOver) { // ���� ���� �� Ȱ��ȭ
        var gameEndType = isGameOver ? GameEndType.GameOver : GameEndType.GameClear;
        gameEndContainer.gameObject.SetActive(true);
        gameEndContainer.GameEndInitialize(gameEndType);
    }

    // resumeProduction �����̳�
    public void ReStartGame() => resumeProductionContainer.gameObject.SetActive(true);
    public void EndProduction() => resumeProductionContainer.EndProduction();
}
