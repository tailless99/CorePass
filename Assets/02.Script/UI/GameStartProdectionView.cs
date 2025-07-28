using TMPro;
using UnityEngine;

public class GameStartProdectionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;

    private Animator animator;
    private int currCount;  // UI에 표시되는 숫자 카운트
    private bool isAnimPlay;


    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        currCount = 3;
        isAnimPlay = false;
    }

    private void Update() {
        // 연출이 가능하고, 아직 남았을 경우
        if(currCount > 0) {
            PlayOnShootAnimation();
        }
        // 모든 연출이 끝났을 경우
        else {
            // 초기화 및 비활성화
            isAnimPlay = false;
            GameManager.Instance.ReStartGameSetting(); // 게임 실행
            UIManager.Instance.EndProduction();
        }
    }

    public void PlayOnShootAnimation() {
        if (isAnimPlay || animator.GetBool("isStart")) return;
        isAnimPlay = true; // 중복 실행 방지
        
        countText.text = currCount.ToString(); // UI 숫자 갱신
        animator.SetBool("isStart", true); // 애니메이션 실행
    }

    public void ChangeText() {
        currCount--;
        countText.text = currCount.ToString(); // UI 숫자 갱신
    }
}
