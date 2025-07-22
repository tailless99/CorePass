using UnityEngine;

public enum PoolType { SquareObj, PentagonObj, hexagonObj, heptagonObj, octagonObj, SliverCoin, GoldCoin, RedCoin, Bomb, Clover }
public class ObjectManager : Singleton<ObjectManager>
{
    [Header("Obstacle Prefabs")]
    [SerializeField] public GameObject SquareObj_Prefab;    // �簢��
    [SerializeField] public GameObject PentagonObj_Prefab;  // ������
    [SerializeField] public GameObject hexagonObj_Prefab;   // ������
    [SerializeField] public GameObject heptagonObj_Prefab;  // ĥ����
    [SerializeField] public GameObject octagonObj_Prefab;   // �Ȱ���

    [Header("Item Prefabs")]
    [SerializeField] public GameObject SliverCoinObj_Prefab; // �ǹ� ����
    [SerializeField] public GameObject GoldCoinObj_Prefab;   // ��� ����
    [SerializeField] public GameObject RedCoinObj_Prefab;    // ���� ����
    [SerializeField] public GameObject BombObj_Prefab;    // ��ź
    [SerializeField] public GameObject CloverObj_Prefab;    // Ŭ�ι�


    public Transform SpawnParent; // �ν��Ͻ��� ������Ʈ�� ��Ƴ��� ������Ʈ

    // ���� ������Ʈ�� ������ ������
    [SerializeField] GameObject[] SquareObj;
    [SerializeField] GameObject[] PentagonObj;
    [SerializeField] GameObject[] hexagonObj;
    [SerializeField] GameObject[] heptagonObj;
    [SerializeField] GameObject[] octagonObj;

    // ���� �������� ������ ������
    [SerializeField] GameObject[] SliverCoinObj;
    [SerializeField] GameObject[] GoldCoinObj;
    [SerializeField] GameObject[] RedCoinObj;
    [SerializeField] GameObject[] BombObj;
    [SerializeField] GameObject[] CloverObj;


    // ��ȯ ����
    GameObject[] targetPool;
    


    protected override void Awake() {
        base.Awake();
        
        // Obstacle
        SquareObj = new GameObject[10];
        PentagonObj = new GameObject[10];
        hexagonObj = new GameObject[10];
        heptagonObj = new GameObject[10];
        octagonObj = new GameObject[10];

        // Items
        SliverCoinObj = new GameObject[100];
        GoldCoinObj = new GameObject[100];
        RedCoinObj = new GameObject[100];
        BombObj = new GameObject[100];
        CloverObj = new GameObject[100];

        Generate();
    }

    private void Generate() {
#region
        // Obstacle
        for (int i = 0; i < SquareObj.Length; i++) {
            SquareObj[i] = Instantiate(SquareObj_Prefab, SpawnParent);
            SquareObj[i].SetActive(false);
        }
        for (int i = 0; i < PentagonObj.Length; i++) {
            PentagonObj[i] = Instantiate(PentagonObj_Prefab, SpawnParent);
            PentagonObj[i].SetActive(false);
        }
        for (int i = 0; i < hexagonObj.Length; i++) {
            hexagonObj[i] = Instantiate(hexagonObj_Prefab, SpawnParent);
            hexagonObj[i].SetActive(false);
        }
        for (int i = 0; i < heptagonObj.Length; i++) {
            heptagonObj[i] = Instantiate(heptagonObj_Prefab, SpawnParent);
            heptagonObj[i].SetActive(false);
        }
        for (int i = 0; i < octagonObj.Length; i++) {
            octagonObj[i] = Instantiate(octagonObj_Prefab, SpawnParent);
            octagonObj[i].SetActive(false);
        }
#endregion

#region
        // Item
        for (int i = 0; i < SliverCoinObj.Length; i++) {
            SliverCoinObj[i] = Instantiate(SliverCoinObj_Prefab, SpawnParent);
            SliverCoinObj[i].SetActive(false);
        }
        for (int i = 0; i < GoldCoinObj.Length; i++) {
            GoldCoinObj[i] = Instantiate(GoldCoinObj_Prefab, SpawnParent);
            GoldCoinObj[i].SetActive(false);
        }
        for (int i = 0; i < RedCoinObj.Length; i++) {
            RedCoinObj[i] = Instantiate(RedCoinObj_Prefab, SpawnParent);
            RedCoinObj[i].SetActive(false);
        }
        for (int i = 0; i < BombObj.Length; i++) {
            BombObj[i] = Instantiate(BombObj_Prefab, SpawnParent);
            BombObj[i].SetActive(false);
        }
        for (int i = 0; i < CloverObj.Length; i++) {
            CloverObj[i] = Instantiate(CloverObj_Prefab, SpawnParent);
            CloverObj[i].SetActive(false);
        }
#endregion
    }

    public GameObject MakeObj(PoolType type) {
        switch (type) {
#region
            case PoolType.SquareObj:
                targetPool = SquareObj;
                break;
            case PoolType.PentagonObj:
                targetPool = PentagonObj;
                break;
            case PoolType.hexagonObj:
                targetPool = hexagonObj;
                break;
            case PoolType.heptagonObj:
                targetPool = heptagonObj;
                break;
            case PoolType.octagonObj:
                targetPool = octagonObj;
                break;
#endregion

#region
            case PoolType.SliverCoin:
                targetPool = SliverCoinObj;
                break;
            case PoolType.GoldCoin:
                targetPool = GoldCoinObj;
                break;
            case PoolType.RedCoin:
                targetPool = RedCoinObj;
                break;
            case PoolType.Bomb:
                targetPool = BombObj;
                break;
            case PoolType.Clover:
                targetPool = CloverObj;
                break;
#endregion
        }

        for (int i = 0; i < targetPool.Length; i++) {
            if (!targetPool[i].activeSelf) {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
}
