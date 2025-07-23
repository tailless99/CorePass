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
        //float t = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f; // 0~1 �ݺ�
        float t = Time.time * speed;
        Vector2 tiling = Vector2.one * (1 + t); // Ȯ��/���
        Vector2 offset = (Vector2.one - tiling) * 0.5f; // �߽� ����

        mat.mainTextureScale = tiling;
        mat.mainTextureOffset = offset;
    }
}
