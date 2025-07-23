using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private Animator textAnimator;
    private int score = 0;
    private bool isFever;


    private void Awake() {
        textAnimator = scoreText.GetComponent<Animator>();
    }

    private void Start() {
        // 초기화
        scoreText.text = score.ToString();
    }

    public void AddScore(int addScore) {
        int feverBonus = isFever ? 2 : 1; // 피버중이면 점수가 2배, 아니면 1배
        score += addScore * feverBonus;
        scoreText.text = score.ToString();
        textAnimator.SetTrigger("ScoreChanged"); // 애니메이션 재생
    }

    // 피버 On/Off 변수 토글
    public void SetFiverState(bool isFevered) => isFever = isFevered;
}