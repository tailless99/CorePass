using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] ScoreContainer scoreContainer;
    [SerializeField] ClockContainer clockContainer;


    // ��� ó��
    public void AddScore(int score) => scoreContainer.AddScore(score); // ���� �߰�
    public void UpdateClock(float time) => clockContainer.UpdateClock(time); // �ð� ����
}
