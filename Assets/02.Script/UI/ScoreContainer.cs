using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private Animator textAnimator;
    private int score = 0;

    private void Awake() {
        textAnimator = scoreText.GetComponent<Animator>();
    }

    private void Start() {
        // �ʱ�ȭ
        scoreText.text = score.ToString();
    }

    public void AddScore(int addScore) {
        score += addScore;
        scoreText.text = score.ToString();
        textAnimator.SetTrigger("ScoreChanged"); // �ִϸ��̼� ���
    }
}