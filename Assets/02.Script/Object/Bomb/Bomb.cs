using Unity.VisualScripting;
using UnityEngine;

public class Bomb : ItemBase
{
    [SerializeField] private GameObject CollideEffect_Prefab;
    [SerializeField] float knockBackPower = 100f;

    protected override void DetectedFunc(Collider2D collision) {
        var effect = Instantiate(CollideEffect_Prefab,transform.position, Quaternion.identity); // ÆÄ±« ÀÌÆåÆ® Àç»ý
        Destroy(effect, 0.5f);

        var dir = (collision.transform.position - transform.position).normalized;
        var player = collision.GetComponent<PlayerController>();
        player.StartKnockBack(dir, knockBackPower); // ÇÃ·¹ÀÌ¾î ³Ë¹é
    }
}
