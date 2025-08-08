using UnityEngine;

public class InfinityRotate : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private bool isLeftRoll = false;
    private float rotateDir;    // T: ���� ȸ��, F : ������ ȸ��

    private void Start() {
        SetRatateDir(isLeftRoll);
    }

    private void Update() {
        transform.Rotate(Vector3.forward * rollSpeed * rotateDir * Time.deltaTime);
    }

    // T: ���� ȸ��, F : ������ ȸ��
    public void SetRatateDir(bool isLeftRoll) {
        rotateDir = isLeftRoll ? 1 : -1;
    }
}
