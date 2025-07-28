using TMPro;
using UnityEngine;

public class AddTimeView : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI addTimeText;
    [SerializeField] private float addTime = 10; // ���� �ð��� ���� ��ġ
    [SerializeField] private float addTimeDelay = 1.5f; // �ð��� �� �߰��� ������ �ɸ��� �ð�

    private bool isAddTimeEvent; // �ð� ���� �̺�Ʈ ���� �÷���
    private float gameTime; // ��ǥ �ð�
    private float currGameTime; // ���� �ð�
    private float startLerpTime;

    private ResumeProductionContainer parentClass;

    private void Awake() {
        parentClass = transform.parent.GetComponent<ResumeProductionContainer>();
    }

    private void OnEnable() {
        addTimeText.text = $"+ {addTime.ToString()} Sec";
        currGameTime = GameManager.Instance.GetGameTime(); // ���� �ð�

        var maxTime = UIManager.Instance.GetMaxGameTime();
        gameTime = Mathf.Min(currGameTime + addTime, maxTime); // ��ǥ �ð�
        isAddTimeEvent = false;
    }

    private void Update() {
        if (isAddTimeEvent) {
            float timeElapsed = Time.time - startLerpTime;
            float percentageComplete = timeElapsed / addTimeDelay;

            currGameTime = Mathf.Lerp(currGameTime, gameTime, Mathf.Clamp01(percentageComplete));

            // �ð� ������Ʈ
            UIManager.Instance.UpdateClock(currGameTime);

            // �ð��� ��ǥġ�� �����ߴ��� Ȯ��
            if (currGameTime >= gameTime) {
                isAddTimeEvent = false; // �ߺ� ���� ����
                parentClass.NextProdectStart(); // ���� ���� ����
            }
        }
    }

    // �ִϸ��̼����� ����Ǵ� �ð� �߰� ���
    private void StartProdect() {
        if (!isAddTimeEvent) // �̹� ���� ���� �ƴ϶�� ����
        {
            startLerpTime = Time.time; // Lerp ���� �ð� ���
            isAddTimeEvent = true;
        }
    }
}
