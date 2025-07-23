using UnityEngine;

public class InfinitePullEffect: MonoBehaviour
{
    public Material mat;
    public float speed = 0.5f;

    private void OnEnable() {
        mat.mainTextureScale = Vector2.one;
        mat.mainTextureOffset = Vector2.zero;
    }

    void Update() {
        //float t = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f; // 0~1 반복
        float t = Time.time * speed;
        Vector2 tiling = Vector2.one * (1 + t); // 확대/축소
        Vector2 offset = (Vector2.one - tiling) * 0.5f; // 중심 유지

        mat.mainTextureScale = tiling;
        mat.mainTextureOffset = offset;
    }
}
