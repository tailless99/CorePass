using UnityEngine;

public class CoinBase : MonoBehaviour {
    [SerializeField] private int score = 100;
    [SerializeField] private AudioClip getCoinSound;

    private Vector3 initialLocalScale; // 코인의 초기 로컬 스케일
    private Transform obstacleBaseTransform; // 부모 ObstacleBase의 Transform

    void Awake() {
    }

    private void OnEnable() {
        initialLocalScale = transform.localScale; // 코인의 초기 스케일 저장
        obstacleBaseTransform = GetComponentInParent<ObstacleBase>()?.transform;
    }

    // 부모 스케일 변경 후 적용되도록 LateUpdate에서 처리
    void LateUpdate() {
        if (obstacleBaseTransform == null) obstacleBaseTransform = GetComponentInParent<ObstacleBase>()?.transform;

        // 부모의 현재 스케일을 가져옵니다.
        Vector3 parentScale = obstacleBaseTransform.localScale;

        // 부모의 각 축 스케일이 0이 아닌 경우에만 역스케일 적용
        float inverseX = (parentScale.x != 0) ? initialLocalScale.x / parentScale.x : 0;
        float inverseY = (parentScale.y != 0) ? initialLocalScale.y / parentScale.y : 0;
        float inverseZ = (parentScale.z != 0) ? initialLocalScale.z / parentScale.z : 0;

        // 부모의 크기에 비례해 역스케일 적용
        transform.localScale = new Vector3(inverseX, inverseY, inverseZ);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            UIManager.Instance.AddScore(score);
            SoundManager.Instance.PlayOneShotSound(getCoinSound, .08f);
            transform.gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        transform.localScale = Vector3.one;
    }
}
