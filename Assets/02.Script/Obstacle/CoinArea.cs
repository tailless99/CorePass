using UnityEngine;

public class CoinArea : MonoBehaviour
{
    private GameObject currentCoin; // 코인을 담을 전역 변수
    private const float BOMB_SPAWN_RATE = 3f;

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
        var selectItemIndex = SpawnObjType();

        switch (selectItemIndex) {
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

    // 아이템 선택 로직
    private int SpawnObjType() {
        var gameLevel = GameManager.Instance.GetGameLevel();
        var bombSpawnChance = gameLevel * BOMB_SPAWN_RATE; // 폭탄 출현 확률
        var isSpawnBomb = Random.Range(0,101) <= bombSpawnChance ? true : false;  // 폭탄 출현 여부

        if (isSpawnBomb) return (int)PoolType.Bomb;
        else return Random.Range(0, 4);
    }
}
