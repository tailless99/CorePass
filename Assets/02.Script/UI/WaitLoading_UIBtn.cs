using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitLoading_UIBtn : MonoBehaviour
{
    [SerializeField] private CanvasGroup myObj;

    private void Start() {
        // �Է� ť�� ���� �̺�Ʈ�� �����ϱ� ���� �� ������ ���
        StartCoroutine(ClearAndActivateUI());
    }

    private IEnumerator ClearAndActivateUI() {
        // �� �������� ��ٷ� �Է� ť�� �ִ� �̺�Ʈ�� ó���ϵ��� ����
        yield return null;

        // �� ���Ŀ� UI�� Ȱ��ȭ
        if (myObj != null) {
            myObj.interactable = true;
            myObj.blocksRaycasts = true;
        }
    }
}
