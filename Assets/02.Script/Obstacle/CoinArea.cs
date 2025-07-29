using UnityEngine;

public class CoinArea : MonoBehaviour
{
    private GameObject currentCoin; // ������ ���� ���� ����
    private const float BOMB_SPAWN_RATE = 3f;

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
        
        currentCoin = ObjectManager.Instance.MakeObj(coinType); // ������ Ȱ��ȭ
        currentCoin.gameObject.transform.SetParent(this.transform); // �θ� ����
        currentCoin.gameObject.transform.localPosition = Vector3.zero;
        currentCoin.gameObject.transform.localScale = Vector3.one * 1.5f;
    }

    // ������ ���� ����
    private int SpawnObjType() {
        var gameLevel = GameManager.Instance.GetGameLevel();
        var bombSpawnChance = gameLevel * BOMB_SPAWN_RATE; // ��ź ���� Ȯ��
        var isSpawnBomb = Random.Range(0,101) <= bombSpawnChance ? true : false;  // ��ź ���� ����

        if (isSpawnBomb) return (int)PoolType.Bomb;
        else return Random.Range(0, 4);
    }
}
