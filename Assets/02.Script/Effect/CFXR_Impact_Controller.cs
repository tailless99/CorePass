using UnityEngine;

public class CFXR_Impact_Controller : MonoBehaviour
{
    [SerializeField] private GameObject[] lateEffects; // 커스텀하기 위해 추가한 이펙트를 제어하기 위한 변수
    [SerializeField] private float offTime = 3f; // 비활성화 시간
    private float timer; // 타이머 변수

    private void OnEnable() {
        timer = 0f;

        // 크기 초기화
        foreach(var effect in lateEffects) {
            effect.transform.localScale = new Vector3(.1f, .1f, .1f);
        }
    }

    private void Update() {
        timer += Time.deltaTime;

        // 비활성화 시간이 지났다면 비활성화
        if(timer > offTime) {
            gameObject.SetActive(false);
        }
    }
}
