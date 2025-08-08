using UnityEngine;

public class InfinityRotate : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private bool isLeftRoll = false;
    private float rotateDir;    // T: 왼쪽 회전, F : 오른쪽 회전

    private void Start() {
        SetRatateDir(isLeftRoll);
    }

    private void Update() {
        transform.Rotate(Vector3.forward * rollSpeed * rotateDir * Time.deltaTime);
    }

    // T: 왼쪽 회전, F : 오른쪽 회전
    public void SetRatateDir(bool isLeftRoll) {
        rotateDir = isLeftRoll ? 1 : -1;
    }
}
