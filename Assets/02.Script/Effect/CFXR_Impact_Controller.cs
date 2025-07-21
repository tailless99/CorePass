using UnityEngine;

public class CFXR_Impact_Controller : MonoBehaviour
{
    [SerializeField] private GameObject[] lateEffects; // Ŀ�����ϱ� ���� �߰��� ����Ʈ�� �����ϱ� ���� ����
    [SerializeField] private float offTime = 3f; // ��Ȱ��ȭ �ð�
    private float timer; // Ÿ�̸� ����

    private void OnEnable() {
        timer = 0f;

        // ũ�� �ʱ�ȭ
        foreach(var effect in lateEffects) {
            effect.transform.localScale = new Vector3(.1f, .1f, .1f);
        }
    }

    private void Update() {
        timer += Time.deltaTime;

        // ��Ȱ��ȭ �ð��� �����ٸ� ��Ȱ��ȭ
        if(timer > offTime) {
            gameObject.SetActive(false);
        }
    }
}
