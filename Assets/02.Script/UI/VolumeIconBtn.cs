using UnityEngine;
using UnityEngine.UI;

public class VolumeIconBtn : MonoBehaviour
{
    [SerializeField] private Button volumeBtn;
    [SerializeField] private Slider IconSlider;
    [SerializeField] private GameObject SliderObj;
    [SerializeField] private VolumeContainer volumeContainer;

    [SerializeField] private float maxShowTime = 3f;
    private float curretTimer = 0;
    
    private bool isBtnCilck = false;


    private void Awake() {
        volumeBtn = GetComponent<Button>();
    }

    private void Start() {
        // ��ư Ŭ�� -> Ÿ�̸� ���
        volumeBtn.onClick.AddListener(() => ShowSliderUI(true));
        
        // ������ �����̴� �� ���Ť�
        IconSlider.value = volumeContainer.GetSliderValue();
    }

    private void Update() {
        if (isBtnCilck && !volumeContainer.isClick) {
            curretTimer += Time.deltaTime;

            if(curretTimer >= maxShowTime) {
                // Ÿ�̸� �ʱ�ȭ
                curretTimer = 0;

                // UI �����ֱ� ���
                ShowSliderUI(false);
            }
        }

        // ���� �����̴��� Ŭ���Ǿ��ٸ� ��� �ð� �ʱ�ȭ
        if (volumeContainer.isClick) {
            curretTimer = 0;
        }
    }

    // UI �����ֱ� ��� ���
    private void ShowSliderUI(bool isActive) {
        isBtnCilck = isActive; // Ÿ�̸� ���
        SliderObj.SetActive(isActive);

        // ICon �ϴ��� �����̴� ����
        IconSlider.value = volumeContainer.GetSliderValue(); // value ����
        IconSlider.gameObject.SetActive(!isActive);
    }
}
