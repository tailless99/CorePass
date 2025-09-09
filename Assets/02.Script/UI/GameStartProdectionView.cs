using TMPro;
using UnityEngine;

public class GameStartProdectionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;

    private Animator animator;
    private int currCount;  // UI�� ǥ�õǴ� ���� ī��Ʈ
    private bool isAnimPlay;


    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        currCount = 3;
        isAnimPlay = false;
    }

    private void Update() {
        // ������ �����ϰ�, ���� ������ ���
        if(currCount > 0) {
            PlayOnShootAnimation();
        }
        // ��� ������ ������ ���
        else {
            // �ʱ�ȭ �� ��Ȱ��ȭ
            isAnimPlay = false;

            // ���� ���� �̺�Ʈ ����
            EventBusManager.Instance.Publish(new RestartAnimationFinishedEvent());
        }
    }

    public void PlayOnShootAnimation() {
        if (isAnimPlay || animator.GetBool("isStart")) return;
        isAnimPlay = true; // �ߺ� ���� ����
        
        countText.text = currCount.ToString(); // UI ���� ����
        animator.SetBool("isStart", true); // �ִϸ��̼� ����
    }

    public void ChangeText() {
        currCount--;
        countText.text = currCount.ToString(); // UI ���� ����
    }
}
