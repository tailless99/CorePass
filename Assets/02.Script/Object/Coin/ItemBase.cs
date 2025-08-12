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
            DetectedFunc(collision); // 자식에서 재정의해서 사용하는 함수

            // 이벤트 실행
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

    // 자식에서 재정의해서 사용하는 함수
    // 자식에서 겹침시 추가적으로 정의해야 하는 기능을 이곳에 재정의한다.
    protected virtual void DetectedFunc(Collider2D collision) { }
}
