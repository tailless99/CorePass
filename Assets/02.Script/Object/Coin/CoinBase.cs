using UnityEngine;

public class CoinBase : MonoBehaviour {
    [SerializeField] private int score = 100;
    [SerializeField] private AudioClip getCoinSound;

    private Vector3 initialLocalScale; // ������ �ʱ� ���� ������
    private Transform obstacleBaseTransform; // �θ� ObstacleBase�� Transform

    void Awake() {
    }

    private void OnEnable() {
        initialLocalScale = transform.localScale; // ������ �ʱ� ������ ����
        obstacleBaseTransform = GetComponentInParent<ObstacleBase>()?.transform;
    }

    // �θ� ������ ���� �� ����ǵ��� LateUpdate���� ó��
    void LateUpdate() {
        if (obstacleBaseTransform == null) obstacleBaseTransform = GetComponentInParent<ObstacleBase>()?.transform;

        // �θ��� ���� �������� �����ɴϴ�.
        Vector3 parentScale = obstacleBaseTransform.localScale;

        // �θ��� �� �� �������� 0�� �ƴ� ��쿡�� �������� ����
        float inverseX = (parentScale.x != 0) ? initialLocalScale.x / parentScale.x : 0;
        float inverseY = (parentScale.y != 0) ? initialLocalScale.y / parentScale.y : 0;
        float inverseZ = (parentScale.z != 0) ? initialLocalScale.z / parentScale.z : 0;

        // �θ��� ũ�⿡ ����� �������� ����
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
