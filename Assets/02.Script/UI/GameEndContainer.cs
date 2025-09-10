using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndContainer : MonoBehaviour
{
    public enum GameEndType { GameOver, GameClear }
    private GameEndType type;

    [SerializeField] private TextMeshProUGUI title; // UI ����� Ÿ��Ʋ
    [SerializeField] private TextMeshProUGUI score; // Score Text
    [SerializeField] private GameObject AdvertisingBtn; // ������ �ٽ��ϱ� ��ư

    [SerializeField] private bool isReStarted;

    private void Start() {
        EventBusManager.Instance.Subscribe<TakeRewardAfterAdEvent>(_=> ResumeGameAfterAd());
    }

    public void GameEndInitialize(GameEndType type) {
        this.type = type;

        switch (type) {
            case GameEndType.GameOver:
                title.text = "GameOver";
                AdvertisingBtn.SetActive(true);
                break;
            case GameEndType.GameClear:
                AdvertisingBtn.SetActive(false);
                title.text = "GameClear";
                break;
        }
        
        // ������ �ٽ��ϱ�� ���Ӵ� 1����
        if (!isReStarted) isReStarted = true;
        else AdvertisingBtn.SetActive(false);

        score.text = UIManager.Instance.GetScore().ToString("N0");
    }

    // ������ ó������ �ٽ� �����ϴ� ���
    public void RestartGame() {
        isReStarted = false;
        GameManager.Instance.ReStartGameSetting(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ���� �� ���� ������(�ٽ� ����) �Լ�
    public void ResumeGameAfterAd() {
        // �ٽ� �����ϱ� ���
        GameManager.Instance.ResumeGameAfterAd();
        gameObject.SetActive(false);
    }
}
