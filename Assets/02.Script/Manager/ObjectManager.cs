using UnityEngine;

public enum PoolType { SquareObj, PentagonObj, hexagonObj, heptagonObj, octagonObj, SliverCoin, GoldCoin, RedCoin }
public class ObjectManager : Singleton<ObjectManager>
{
    [Header("Obstacle Prefabs")]
    [SerializeField] public GameObject SquareObj_Prefab;    // 사각형
    [SerializeField] public GameObject PentagonObj_Prefab;  // 오각형
    [SerializeField] public GameObject hexagonObj_Prefab;   // 육각형
    [SerializeField] public GameObject heptagonObj_Prefab;  // 칠각형
    [SerializeField] public GameObject octagonObj_Prefab;   // 팔각형

    [Header("Item Prefabs")]
    [SerializeField] public GameObject SliverCoinObj_Prefab;
    [SerializeField] public GameObject GoldCoinObj_Prefab;
    [SerializeField] public GameObject RedCoinObj_Prefab;


    public Transform SpawnParent; // 인스턴스한 오브젝트를 모아넣을 오브젝트

    // 실제 오브젝트를 저장할 변수들
    [SerializeField] GameObject[] SquareObj;
    [SerializeField] GameObject[] PentagonObj;
    [SerializeField] GameObject[] hexagonObj;
    [SerializeField] GameObject[] heptagonObj;
    [SerializeField] GameObject[] octagonObj;

    // 실제 아이템을 저장할 변수들
    [SerializeField] GameObject[] SliverCoinObj;
    [SerializeField] GameObject[] GoldCoinObj;
    [SerializeField] GameObject[] RedCoinObj;


    // 반환 변수
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

        Generate();
    }

    private void Generate() {
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
    }

    public GameObject MakeObj(PoolType type) {
        switch (type) {
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
                
            case PoolType.SliverCoin:
                targetPool = SliverCoinObj;
                break;
            case PoolType.GoldCoin:
                targetPool = GoldCoinObj;
                break;
            case PoolType.RedCoin:
                targetPool = RedCoinObj;
                break;
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
