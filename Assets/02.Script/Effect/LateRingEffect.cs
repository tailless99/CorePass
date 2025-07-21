using UnityEngine;

public class LateRingEffect : MonoBehaviour
{
    [SerializeField] private float upScaleSpeed = 1f;

    private void Update() {
        transform.localScale += upScaleSpeed * Vector3.one;
    }
}
