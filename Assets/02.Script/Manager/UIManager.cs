using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;
    [SerializeField] FeverContainer feverContainer;


    // ��� ó��
    public void AddScore(int score) => scoreContainer.AddScore(score); // ���� �߰�
    public void SetFiverState(bool isFeverde) => scoreContainer.SetFiverState(isFeverde); // �ǹ� ���� ���
    public void UpdateClock(float time) => clockContainer.UpdateClock(time); // �ð� ����
    public void AddFeverPoint(int addPoint) => feverContainer.AddFeverPoint(addPoint); // �ǹ� ����Ʈ �̰�
}
