using Unity.VisualScripting;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.TryGetComponent<PlayerController>(out var player);
            player?.PlayerDeath();
        }
    }
}
