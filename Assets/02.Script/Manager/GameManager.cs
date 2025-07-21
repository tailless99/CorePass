using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Game Settings")]
    [SerializeField] private float gameTime = 120f; // 1�ǿ� �ɸ��� �ð� ����
    [SerializeField] private float maxSpawnTime = .5f; // ���ع� ��ȯ �ֱ�

    [Header("Change Color Event Settings")]
    [SerializeField] private GameObject changeColorEffect_Prefab;
    [SerializeField] private float colorChangeEventCoolTime = 23f; // ����ȯ �̺�Ʈ ��Ÿ��

    // ��� ����
    private float changeColorTimer; // ����ȯ �̺�Ʈ Ÿ�̸�
    private float spawnTimer = 0; // ���ع� Ȱ��ȭ Ÿ�̸�


    // ��� ���ϱ�� ��ȹ ���� - ���� ����
    // ���� ���࿡ ���Ǵ� ����
    /*
        private int randomPatten; // ���� ���� ���� Ű
        private int maxSpawnCount; // ��ȯ�� �ִ� ���ع� ����
        private int currentSpawnCount; // ���� ��ȯ�� ���ع� ��
        private bool isPattening;
    */


    private void Awake() {
        changeColorEffect_Prefab.SetActive(false);

        // �ʱ�ȭ
        changeColorTimer = colorChangeEventCoolTime;
    }

    private void Update() {
        // �ð� ����
        UpdateClock();
        // ����ȯ �̺�Ʈ
        changeColorEvent();
        // ���ع� ��ȯ
        SpawnObstacle();

        // ���ع� ��ȯ ����
        //SelectObstaclePatten();
    }

    // ����ȯ �̺�Ʈ
    public void changeColorEvent() {
        // Ÿ�̸� ó��
        changeColorTimer -= Time.deltaTime;
        if (changeColorTimer > 0) return;
        changeColorTimer = colorChangeEventCoolTime; // �ٽ� �ʱ�ȭ

        changeColorEffect_Prefab.SetActive(true); // ���� Ȱ��ȭ

        // ���� ����Ʈ ���� ����
        if (ColorManager.Instance.ShuffleColorList()) {

            // �ʵ��� ������Ʈ�� ���� ���ġ ����
            // 1. �÷��̾� �� ��ȯ
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().SetColliderColor(); // ����ȯ ����

            // 2. �ʵ��� ���ع� ����ȯ
            var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (var obstacle in obstacles) {
                obstacle.GetComponent<ObstacleBase>().SetColliderColor(); // ����ȯ ����
            }
        }
    }

    // �ð� ����
    public void UpdateClock() {
        gameTime -= Time.deltaTime; // �ð� ����
        UIManager.Instance.UpdateClock(gameTime);  // UI ����
    }

    /// <summary>
    /// ���ع��� ��ȯ�ϴ� ����� ����
    /// �ð��� ���� ��ȯ�ϴ� ������ �޶�����.
    /// </summary>
    private GameObject GetObstacleObj() {
        // ��ȯ ��ü
        GameObject returnObj = null;

        if (gameTime > 119) { // �簢�� ��ȯ
        //if (gameTime > 105) { // �簢�� ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.SquareObj);
        }
        //else if (gameTime > 75) { // ������ ��ȯ
        else if (gameTime > 118) { // ������ ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.PentagonObj);
        }
        else if (gameTime > 117) { // ������ ��ȯ
        //else if (gameTime > 55) { // ������ ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.hexagonObj);
        }
        else if (gameTime > 116) { // ĥ���� ��ȯ
        //else if (gameTime > 35) { // ĥ���� ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.heptagonObj);
        }
        else { // �Ȱ��� ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.octagonObj);
        }

        return returnObj;
    }

    // �ֱ⸶�� ���ع� ��ȯ
    private void SpawnObstacle() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer < maxSpawnTime) return; // ��ȯ ��Ÿ���� �ٵ��� �ʾҴٸ� ��ȯ
        
        spawnTimer = 0;
        
        // ���ع� ��ȯ
        GetObstacleObj();
    }

    /*
    // ���ع� ��ȯ ����
    private void SelectObstaclePatten() {
        // ���� ���� �ƴ϶��
        if (!isPattening) {
            // �ʱ�ȭ
            isPattening = true; // ���� ���� �÷���
            randomPatten = Random.Range(0, 101); // 0 ~ 100% ���� ���� ����ġ ���
            currentSpawnCount = 0; // ���� ��ȯ�� ���ع� �� �ʱ�ȭ
            spawnTimer = 0; // ���ع� Ȱ��ȭ Ÿ�̸� �ʱ�ȭ
        }
        // ���� ���̶��
        else {
            // ����ġ�� ���� ���� ����
            if (randomPatten > 70) { // 30% Ȯ���� ����
                Patten_RandomSafeBlock();
            }
        }
    }

#region PattenFunc
    // ������ ���� �������밡 �Ǵ� ����
    private void Patten_RandomSafeBlock() {
        maxSpawnCount = 6; // ��ȯ�� �ִ� ���ع� ����
        maxSpawnTime = .2f; // ��ȯ�ֱ�

        while(currentSpawnCount < maxSpawnCount) {
            spawnTimer += Time.deltaTime;

            if (spawnTimer < maxSpawnTime) continue;
            spawnTimer = 0f; // ������ ���� �ʱ�ȭ

            // ���ع� ����
            var obj = GetObstacleObj();
            obj.TryGetComponent<ObstacleBase>(out var obstacleObj);
            
            // ������ ���ع� Ŭ������ ���� �������� �����ϴ� �κ��� �������ϰ�
            // �������븦 ���� �� �� �ֵ��� Ŀ���� �ؾ���.

            // ��ó��
            currentSpawnCount++; // ��ȯ ī��Ʈ ����
        }

        isPattening = false;
    }
#endregion
    */
}
