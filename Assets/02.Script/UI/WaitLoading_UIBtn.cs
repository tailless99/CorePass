using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitLoading_UIBtn : MonoBehaviour
{
    [SerializeField] private CanvasGroup myObj;

    private void Start() {
        // 입력 큐에 남은 이벤트를 무시하기 위해 한 프레임 대기
        StartCoroutine(ClearAndActivateUI());
    }

    private IEnumerator ClearAndActivateUI() {
        // 한 프레임을 기다려 입력 큐에 있는 이벤트를 처리하도록 유도
        yield return null;

        // 그 이후에 UI를 활성화
        if (myObj != null) {
            myObj.interactable = true;
            myObj.blocksRaycasts = true;
        }
    }
}
