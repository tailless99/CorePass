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
        // 버튼 클릭 -> 타이머 토글
        volumeBtn.onClick.AddListener(() => ShowSliderUI(true));
        
        // 아이콘 슬라이더 값 갱신ㄹ
        IconSlider.value = volumeContainer.GetSliderValue();
    }

    private void Update() {
        if (isBtnCilck && !volumeContainer.isClick) {
            curretTimer += Time.deltaTime;

            if(curretTimer >= maxShowTime) {
                // 타이머 초기화
                curretTimer = 0;

                // UI 보여주기 토글
                ShowSliderUI(false);
            }
        }

        // 볼륨 슬라이더가 클릭되었다면 대기 시간 초기화
        if (volumeContainer.isClick) {
            curretTimer = 0;
        }
    }

    // UI 보여주기 토글 기능
    private void ShowSliderUI(bool isActive) {
        isBtnCilck = isActive; // 타이머 토글
        SliderObj.SetActive(isActive);

        // ICon 하단의 슬라이더 조절
        IconSlider.value = volumeContainer.GetSliderValue(); // value 갱신
        IconSlider.gameObject.SetActive(!isActive);
    }
}
