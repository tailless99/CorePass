using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeContainer : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Slider volumeSlider;

    public bool isClick = false;


    // 활성화 시 초기화
    private void OnEnable() {
        isClick = false;
    }

    // 볼륨 슬라이더가 변경되었을 때 호출되는 함수
    public void onChangedSliderValue() {
        EventBusManager.Instance.Publish(new ChangedBGMVolumeEvent(volumeSlider.value));
    }

    // 슬라이더가 눌렸을 때
    public void OnPointerDown(PointerEventData eventData) {
        isClick = true;
    }

    // 슬라이더의 클릭이 끝났을 때
    public void OnPointerUp(PointerEventData eventData) {
        isClick = false;
    }

    public float GetSliderValue() => volumeSlider.value;
}
