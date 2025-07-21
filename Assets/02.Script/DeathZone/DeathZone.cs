using Unity.VisualScripting;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5f;

    private float rotateDir;    // T: ���� ȸ��, F : ������ ȸ��

    private void Start() {
        SetRatateDir(false);
    }

    private void Update() {
        transform.Rotate(Vector3.forward * rollSpeed * rotateDir);
    }

    // T: ���� ȸ��, F : ������ ȸ��
    public void SetRatateDir(bool isLeftRoll) {
        rotateDir = isLeftRoll ? 1 : -1;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.TryGetComponent<PlayerController>(out var player);
            player?.PlayerDeath();
        }
    }
}
