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
        // �ʱ�ȭ
        scoreText.text = score.ToString();
    }

    public void AddScore(int addScore) {
        int feverBonus = isFever ? 2 : 1; // �ǹ����̸� ������ 2��, �ƴϸ� 1��
        score += addScore * feverBonus;
        scoreText.text = score.ToString();
        textAnimator.SetTrigger("ScoreChanged"); // �ִϸ��̼� ���
    }

    // �ǹ� On/Off ���� ���
    public void SetFiverState(bool isFevered) => isFever = isFevered;
}