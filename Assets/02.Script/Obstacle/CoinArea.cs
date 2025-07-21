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
        var coinType = coinIndex == 0 ? PoolType.SliverCoin : coinIndex == 1 ? PoolType.GoldCoin : PoolType.RedCoin;
        
        currentCoin = ObjectManager.Instance.MakeObj(coinType); // ���� Ȱ��ȭ
        currentCoin.gameObject.transform.SetParent(this.transform); // �θ� ����
        currentCoin.gameObject.transform.localPosition = Vector3.zero;
    }

    private void OnDisable() {
        // ���� ������ �ִٸ�
        if (currentCoin != null) {
            // ���� ��ȯ
            currentCoin.gameObject.SetActive(false);
        }
    }
}
