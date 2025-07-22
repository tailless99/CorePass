using UnityEngine;

public class CoinArea : MonoBehaviour
{
    private GameObject currentCoin; // ������ ���� ���� ����

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
        
        currentCoin = ObjectManager.Instance.MakeObj(coinType); // ������ Ȱ��ȭ
        currentCoin.gameObject.transform.SetParent(this.transform); // �θ� ����
        currentCoin.gameObject.transform.localPosition = Vector3.zero;
        currentCoin.gameObject.transform.localScale = Vector3.one * 1.5f;
    }

    private void OnDisable() {
        // ���� ������ �ִٸ�
        if (currentCoin != null) {
            // ���� ��ȯ
            currentCoin.gameObject.SetActive(false);
        }
    }
}
