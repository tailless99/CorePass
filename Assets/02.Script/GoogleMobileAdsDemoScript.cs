using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    // 보상형 광고 객체
    private RewardedAd rewardedAd;

    // 테스트용 광고 단위 ID
    private const string adUnitId = "ca-app-pub-3940256099942544/5224354917";

    public Button showAdButton;

    void Start() {
        // 모바일 광고 SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) => {
            if (initStatus == null) {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }

            Debug.Log("Google Mobile Ads initialization complete.");

            // 초기 광고 로드
            LoadRewardedAd();
        });

        // 버튼 클릭 이벤트에 광고 보여주기 함수 연결
        showAdButton.onClick.AddListener(ShowRewardedAd);
    }

    // 보상형 광고 로드
    public void LoadRewardedAd() {
        // 이미 광고가 있으면 파괴하고 새로 로드
        if (rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        // 광고 요청 생성
        var adRequest = new AdRequest();

        // 광고 로드 및 이벤트 처리
        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) => {
            if (error != null) {
                Debug.LogError($"Rewarded ad failed to load with error: {error.GetMessage()}");
                return;
            }

            // 광고 로드 성공 시
            rewardedAd = ad;
            RegisterEventHandlers(rewardedAd);
        });
    }

    // 광고 이벤트 핸들러 등록
    private void RegisterEventHandlers(RewardedAd ad) {
        ad.OnAdFullScreenContentClosed += () => {
            // 광고가 닫히면 다음 광고를 미리 로드
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) => {
            // 실패 시 다음 광고를 미리 로드
            LoadRewardedAd();
        };
    }

    // 보상형 광고 보여주기
    public void ShowRewardedAd() {
        if (rewardedAd != null && rewardedAd.CanShowAd()) {
            rewardedAd.Show((Reward reward) => {
                // 보상이 지급될 때 호출되는 부분
                ContinueGame();
            });
        }
        else {
            // 광고가 준비되지 않았을 때 다시 로드
            LoadRewardedAd();
        }
    }

    // 광고 시청 후 게임을 이어서 진행하는 함수
    private void ContinueGame() {
        // 보상 지급 이벤트 실행
        EventBusManager.Instance.Publish<TakeRewardAfterAdEvent>(new TakeRewardAfterAdEvent());
    }
}