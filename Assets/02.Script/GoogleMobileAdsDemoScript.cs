using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    // ������ ���� ��ü
    private RewardedAd rewardedAd;

    // �׽�Ʈ�� ���� ���� ID
    private const string adUnitId = "ca-app-pub-3940256099942544/5224354917";

    public Button showAdButton;

    void Start() {
        // ����� ���� SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) => {
            if (initStatus == null) {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }

            Debug.Log("Google Mobile Ads initialization complete.");

            // �ʱ� ���� �ε�
            LoadRewardedAd();
        });

        // ��ư Ŭ�� �̺�Ʈ�� ���� �����ֱ� �Լ� ����
        showAdButton.onClick.AddListener(ShowRewardedAd);
    }

    // ������ ���� �ε�
    public void LoadRewardedAd() {
        // �̹� ���� ������ �ı��ϰ� ���� �ε�
        if (rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        // ���� ��û ����
        var adRequest = new AdRequest();

        // ���� �ε� �� �̺�Ʈ ó��
        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) => {
            if (error != null) {
                Debug.LogError($"Rewarded ad failed to load with error: {error.GetMessage()}");
                return;
            }

            // ���� �ε� ���� ��
            rewardedAd = ad;
            RegisterEventHandlers(rewardedAd);
        });
    }

    // ���� �̺�Ʈ �ڵ鷯 ���
    private void RegisterEventHandlers(RewardedAd ad) {
        ad.OnAdFullScreenContentClosed += () => {
            // ���� ������ ���� ���� �̸� �ε�
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) => {
            // ���� �� ���� ���� �̸� �ε�
            LoadRewardedAd();
        };
    }

    // ������ ���� �����ֱ�
    public void ShowRewardedAd() {
        if (rewardedAd != null && rewardedAd.CanShowAd()) {
            rewardedAd.Show((Reward reward) => {
                // ������ ���޵� �� ȣ��Ǵ� �κ�
                ContinueGame();
            });
        }
        else {
            // ���� �غ���� �ʾ��� �� �ٽ� �ε�
            LoadRewardedAd();
        }
    }

    // ���� ��û �� ������ �̾ �����ϴ� �Լ�
    private void ContinueGame() {
        // ���� ���� �̺�Ʈ ����
        EventBusManager.Instance.Publish<TakeRewardAfterAdEvent>(new TakeRewardAfterAdEvent());
    }
}