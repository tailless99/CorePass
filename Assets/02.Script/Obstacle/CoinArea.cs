using System.Xml.Schema;
using UnityEngine;

public class CoinArea : MonoBehaviour
{
    private GameObject currentCoin; // ������ ���� ���� ����
    private const float BOMB_SPAWN_RATE = 2f;
    private const int SLIVERCOIN_SPAWN_RATE = 40;
    private const int GOLDCOIN_SPAWN_RATE = 30;
    private const int REDCOIN_SPAWN_RATE = 20;
    private const int CLOVER_SPAWN_RATE = 10; // �������� ������ ������


    /// <summary>
    /// coinIndex�� � ������ ��������� ���ϴ� �Ķ�����Դϴ�.
    /// 0 : �ǹ� ����
    /// 1 : ��� ����
    /// 2 : ���� ����
    /// </summary>
    /// <param name="coinIndex"></param>
    public void ActiveCoin(int coinIndex) {
        // ���� Ÿ�Կ� ���� �̳� �Ҵ�
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
        
        currentCoin = ObjectManager.Instance.MakeObj(coinType); // ������ Ȱ��ȭ
        currentCoin.gameObject.transform.SetParent(this.transform); // �θ� ����
        currentCoin.gameObject.transform.localPosition = Vector3.zero;
        currentCoin.gameObject.transform.localScale = Vector3.one * 1.5f;
    }

    // ������ ���� ����
    private int SpawnObjType() {
        var gameLevel = GameManager.Instance.GetGameLevel();
        var bombSpawnChance = gameLevel * BOMB_SPAWN_RATE; // ��ź ���� Ȯ��
        var isSpawnBomb = (float)Random.Range(0,101) <= bombSpawnChance ? true : false;  // ��ź ���� ����

        if (isSpawnBomb) return (int)PoolType.Bomb;
        else {
            // ������ ���� Ȯ�� �ٽ� ���
            var ranNum = Random.Range(0, 101);
            var selectIndex = ranNum <= SLIVERCOIN_SPAWN_RATE ? (int)PoolType.SliverCoin :
                ranNum <= SLIVERCOIN_SPAWN_RATE + GOLDCOIN_SPAWN_RATE ? (int)PoolType.GoldCoin :
                ranNum <= SLIVERCOIN_SPAWN_RATE + GOLDCOIN_SPAWN_RATE + REDCOIN_SPAWN_RATE ? (int)PoolType.RedCoin : (int)PoolType.Clover;
            return selectIndex;
        }
    }
}
