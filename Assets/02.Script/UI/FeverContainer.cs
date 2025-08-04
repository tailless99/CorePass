using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeverContainer : MonoBehaviour {
    [Header("Fever Setting")]
    [SerializeField] private Slider slider;
    [SerializeField] private int maxFeverPoint = 15000;
    [SerializeField] private float maxFiverTime = 10f; // �ǹ� ���� �ð�

    [Header("Fever ����")]
    [SerializeField] private GameObject feverText;
    [SerializeField] private GameObject feverBG;


    private int curFeverPoint; // ���� �ǹ� ����Ʈ
    private float currFiverTimer; // �ǹ� �ð��� ��� Ÿ�̸�
    private bool isFevering;


    private void Awake() {
        curFeverPoint = 0;
        slider.value = 0;
    }

    private void OnEnable() {
        feverText.SetActive(false);
        feverBG.SetActive(false);
    }

    private void Update() {
        // �ǹ� Ÿ�� ����
        StartFeverTime();
    }

    // �ǹ� ����Ʈ�� �߰��ϴ� ���
    public void AddFeverPoint(int addPoint) {
        if (isFevering) return; // �ǹ� �߿� ���� ����

        // �ִ�, �ּҰ� ����
        curFeverPoint = Mathf.Clamp(curFeverPoint + addPoint, 0, maxFeverPoint);

        // �����̴��� �� �ݿ�
        slider.value = (float)curFeverPoint / (float)maxFeverPoint;

        // �ǹ� ����Ʈ�� ��á�ٸ� �ǹ� ���� ���� �÷���
        if (curFeverPoint >= maxFeverPoint) {
            curFeverPoint = 0; // �ʱ�ȭ
            isFevering = true;
        }
    }


    // �ǹ� ������ �ʱ�ȭ
    public void ResetFever() {
        curFeverPoint = 0;
        slider.value = 0;
    }

    // �ǹ� Ÿ�� ����
    public void StartFeverTime() {
        // ���� ó��
        if (!isFevering) return;

        // �ǹ� ���� ����
        if (currFiverTimer >= maxFiverTime) {
            isFevering = false;
            currFiverTimer = 0;
            FeverDirection(isFevering); // �ǹ� ȿ�� On/Off
            return;
        }

        FeverDirection(isFevering); // �ǹ� ȿ�� On/Off
        // Ÿ�̸� �ð� �߰�
        currFiverTimer += Time.deltaTime;

        // �ǹ� ������ ���� ����
        float normalizedTime = currFiverTimer / maxFiverTime; // 0.0 ~ 1.0 ���� ��
        slider.value = Mathf.Lerp(1.0f, 0.0f, normalizedTime);
    }

    // �ǹ� ȿ�� On/Off
    private void FeverDirection(bool isFever) {
        // �ǹ� ���� UI On/Off
        feverText.SetActive(isFever);
        feverBG.SetActive(isFever);

        // �ǹ� ȿ�� On/Off
        UIManager.Instance.SetFiverState(isFever); // ���� 2�� Ȱ��ȭ
        GameManager.Instance.SetFeverState(isFever); // �ݶ��̴� Ȱ��/��Ȱ��ȭ
    }
}
