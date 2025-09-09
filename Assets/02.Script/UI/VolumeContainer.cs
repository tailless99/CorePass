using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeContainer : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Slider volumeSlider;

    public bool isClick = false;


    // Ȱ��ȭ �� �ʱ�ȭ
    private void OnEnable() {
        isClick = false;
    }

    // ���� �����̴��� ����Ǿ��� �� ȣ��Ǵ� �Լ�
    public void onChangedSliderValue() {
        EventBusManager.Instance.Publish(new ChangedBGMVolumeEvent(volumeSlider.value));
    }

    // �����̴��� ������ ��
    public void OnPointerDown(PointerEventData eventData) {
        isClick = true;
    }

    // �����̴��� Ŭ���� ������ ��
    public void OnPointerUp(PointerEventData eventData) {
        isClick = false;
    }

    public float GetSliderValue() => volumeSlider.value;
}
