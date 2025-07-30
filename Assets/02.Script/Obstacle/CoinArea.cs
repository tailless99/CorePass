using System.Xml.Schema;
using UnityEngine;

public class CoinArea : MonoBehaviour
{
    private GameObject currentCoin; // 코인을 담을 전역 변수
    private const float BOMB_SPAWN_RATE = 2f;
    private const int SLIVERCOIN_SPAWN_RATE = 40;
    private const int GOLDCOIN_SPAWN_RATE = 30;
    private const int REDCOIN_SPAWN_RATE = 20;
    private const int CLOVER_SPAWN_RATE = 10; // 나머지라서 아직은 사용안함


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
            case (int)PoolType.SliverCoin:
                coinType = PoolType.SliverCoin;
                break;
            case (int)PoolType.GoldCoin:
                coinType = PoolType.GoldCoin;
                break;
            case (int)PoolType.RedCoin:
                coinType = PoolType.RedCoin;
                break;
            case (int)PoolType.Clover:
                coinType = PoolType.Clover;
                break;
            case (int)PoolType.Bomb:
                coinType = PoolType.Bomb;
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
        var isSpawnBomb = (float)Random.Range(0,101) <= bombSpawnChance ? true : false;  // 폭탄 출현 여부

        if (isSpawnBomb) return (int)PoolType.Bomb;
        else {
            // 아이템 출현 확률 다시 출력
            var ranNum = Random.Range(0, 101);
            var selectIndex = ranNum <= SLIVERCOIN_SPAWN_RATE ? (int)PoolType.SliverCoin :
                ranNum <= SLIVERCOIN_SPAWN_RATE + GOLDCOIN_SPAWN_RATE ? (int)PoolType.GoldCoin :
                ranNum <= SLIVERCOIN_SPAWN_RATE + GOLDCOIN_SPAWN_RATE + REDCOIN_SPAWN_RATE ? (int)PoolType.RedCoin : (int)PoolType.Clover;
            return selectIndex;
        }
    }
}
