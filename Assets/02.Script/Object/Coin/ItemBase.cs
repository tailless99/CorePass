using UnityEngine;

public class ItemBase : MonoBehaviour {
    [SerializeField] private int score = 100;
    [SerializeField] private int addFeverScore = 0;
    [SerializeField] private AudioClip getCoinSound;
    [SerializeField] private float playerSound = 0.08f;

    private void OnEnable() {
        transform.localScale = Vector3.one * 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            DetectedFunc(collision); // �ڽĿ��� �������ؼ� ����ϴ� �Լ�

            // �̺�Ʈ ����
            EventBusManager.Instance.StartEvent_AddScore(score);
            EventBusManager.Instance.StartEvent_AddFeverPoint(addFeverScore);
            EventBusManager.Instance.StartEvent_PlayOneShot(new PlaySoundEvent(getCoinSound, playerSound));
            
            transform.gameObject.SetActive(false);
        }
    }
    
    private void OnDisable() {
        transform.localScale = Vector3.one * 1.5f;
        gameObject.SetActive(false);
    }

    // �ڽĿ��� �������ؼ� ����ϴ� �Լ�
    // �ڽĿ��� ��ħ�� �߰������� �����ؾ� �ϴ� ����� �̰��� �������Ѵ�.
    protected virtual void DetectedFunc(Collider2D collision) { }
}
