using Unity.VisualScripting;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5f;

    private float rotateDir;    // T: 왼쪽 회전, F : 오른쪽 회전

    private void Start() {
        SetRatateDir(false);
    }

    private void Update() {
        transform.Rotate(Vector3.forward * rollSpeed * rotateDir);
    }

    // T: 왼쪽 회전, F : 오른쪽 회전
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
