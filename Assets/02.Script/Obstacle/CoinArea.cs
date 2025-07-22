using UnityEngine;

public class CoinArea : MonoBehaviour
{
    private GameObject currentCoin; // 코인을 담을 전역 변수

    /// <summary>
    /// coinIndex는 어떤 코인을 출력할지를 정하는 파라메터입니다.
    /// 0 : 실버 코인
    /// 1 : 골드 코인
    /// 2 : 레드 코인
    /// </summary>
    /// <param name="coinIndex"></param>
    public void ActiveCoin(int coinIndex) {
        // 코인 타입에 따른 이넘 할당
        PoolType coinType = PoolType.SliverCoin;

        switch (coinIndex) {
            case 0:
                coinType = PoolType.SliverCoin;
                break;
            case 1:
                coinType = PoolType.GoldCoin;
                break;
            case 2:
                coinType = PoolType.RedCoin;
                break;
            case 3:
                coinType = PoolType.Bomb;
                break;
            case 4:
                coinType = PoolType.Clover;
                break;
        }
        
        currentCoin = ObjectManager.Instance.MakeObj(coinType); // 아이템 활성화
        currentCoin.gameObject.transform.SetParent(this.transform); // 부모 설정
        currentCoin.gameObject.transform.localPosition = Vector3.zero;
        currentCoin.gameObject.transform.localScale = Vector3.one * 1.5f;
    }

    private void OnDisable() {
        // 만약 코인이 있다면
        if (currentCoin != null) {
            // 코인 반환
            currentCoin.gameObject.SetActive(false);
        }
    }
}
