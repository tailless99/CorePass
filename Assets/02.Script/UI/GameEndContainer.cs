using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndContainer : MonoBehaviour
{
    public enum GameEndType { GameOver, GameClear }
    private GameEndType type;

    [SerializeField] private TextMeshProUGUI title; // UI 상단의 타이틀
    [SerializeField] private TextMeshProUGUI score; // Score Text
    [SerializeField] private GameObject AdvertisingBtn; // 광고보고 다시하기 버튼

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
        
        // 광고보고 다시하기는 게임당 1번만
        if (!isReStarted) isReStarted = true;
        else AdvertisingBtn.SetActive(false);

        score.text = UIManager.Instance.GetScore().ToString("N0");
    }

    // 게임을 처음부터 다시 시작하는 기능
    public void RestartGame() {
        isReStarted = false;
        GameManager.Instance.ReStartGameSetting(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 광고 본 후의 리워드(다시 시작) 함수
    public void ResumeGameAfterAd() {
        // 다시 시작하기 기능
        GameManager.Instance.ResumeGameAfterAd();
        gameObject.SetActive(false);
    }
}
