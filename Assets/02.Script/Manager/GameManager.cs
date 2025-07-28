using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [Header("Game Settings")]
    [SerializeField] private float gameTime = 120f; // 1�ǿ� �ɸ��� �ð� ����
    [SerializeField] private float maxSpawnTime = .5f; // ���ع� ��ȯ �ֱ�
    [SerializeField] private AudioClip gameBGM;
    [SerializeField] private AudioClip gameClearsound;

    [Header("Change Color Event Settings")]
    [SerializeField] private GameObject changeColorEffect_Prefab;
    [SerializeField] private float colorChangeEventCoolTime = 23f; // ����ȯ �̺�Ʈ ��Ÿ��

    // ��� ����
    private float changeColorTimer; // ����ȯ �̺�Ʈ Ÿ�̸�
    private float spawnTimer = 0; // ���ع� Ȱ��ȭ Ÿ�̸�
    private bool isFever; // �ǹ� ���� ��� ����
    [SerializeField] private bool isSpawnStop; // ��ֹ� ��ȯ ���� �÷��� ����
    [SerializeField] private bool isGameOver;


    protected override void Awake() {
        base.Awake();

        // �ʱ�ȭ
        changeColorEffect_Prefab.SetActive(false);
        changeColorTimer = colorChangeEventCoolTime;
    }

    private void Start() {
        SoundManager.Instance.PlaySound(gameBGM, 0.18f, 1f, true); // BGM ���
    }

    private void OnEnable() {
        gameTime = UIManager.Instance.GetMaxGameTime();
        UIManager.Instance.UpdateClock(gameTime);  // UI ����

        isSpawnStop = true;
        isGameOver = true;
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
        // ���� ������ �ð��� �帣�� ����
        if (isGameOver) return;

        gameTime -= Time.deltaTime; // �ð� ����
        UIManager.Instance.UpdateClock(gameTime);  // UI ����

        // ���� Ŭ���� ó��
        if (gameTime <= 117 && !isGameOver) {
            // �������� ó��
            isSpawnStop = false;
            GameOver();

            // ���� ���� �� ���
            UIManager.Instance.ShowGameEndView(false);
            // Ŭ���� ���� ���
            SoundManager.Instance.PlaySound(gameClearsound, 0.18f, 1f);
        }
    }

    /// <summary>
    /// ���ع��� ��ȯ�ϴ� ����� ����
    /// �ð��� ���� ��ȯ�ϴ� ������ �޶�����.
    /// </summary>
    private GameObject GetObstacleObj() {
        // ��ȯ ��ü
        GameObject returnObj = null;

        if (gameTime > 105) { // �簢�� ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.SquareObj);
        }
        else if (gameTime > 75) { // ������ ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.PentagonObj);
        }
        else if (gameTime > 55) { // ������ ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.hexagonObj);
        }
        else if (gameTime > 35) { // ĥ���� ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.heptagonObj);
        }
        else if (gameTime > 4) { // �Ȱ��� ��ȯ
            returnObj = ObjectManager.Instance.MakeObj(PoolType.octagonObj);
        }

        return returnObj;
    }

    // �ֱ⸶�� ���ع� ��ȯ
    private void SpawnObstacle() {
        if (isSpawnStop) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < maxSpawnTime) return; // ��ȯ ��Ÿ���� �ٵ��� �ʾҴٸ� ��ȯ

        spawnTimer = 0;

        // ���ع� ��ȯ
        var obstacle = GetObstacleObj();
        obstacle.GetComponent<ObstacleBase>()?.OnToggleColliderActive(!isFever); // �ݶ��̴� Ȱ��/��Ȱ��
    }

    public void SetFeverState(bool isFevered) {
        isFever = isFevered;

        // �ǹ� ���� �� ��� ��ֹ��� �ݶ��̴� ��Ȱ��ȭ
        if (isFevered) {
            var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (var obstacle in obstacles) {
                obstacle.GetComponent<ObstacleBase>().OnToggleColliderActive(!isFevered); // �ݶ��̴� Ȱ��/��Ȱ��
            }
        }
    }

    // ���� ���� ���� ��ȯ
    public void GameOver() => isGameOver = !isGameOver;

    // ���� �ð� ��ȯ
    public float GetGameTime() => gameTime;

    // ������ �ٽ� �����ϴ� ���
    public void ResumeGameAfterAd() {
        // �ʻ󿡼� ��ֹ� ����
        isSpawnStop = true; // ��ȯ ���� �÷���
        AllObstacleActive(false);

        // ���� ����
        // UI ��� Ű��
        UIManager.Instance.ReStartGame();
    }

    // ������ �ٽ� �����ϱ� ���� ��ó��
    public void ReStartGameSetting(bool isInit = false) {
        StartGameSetting();
        AllObstacleActive(false);

        // �ʱ�ȭ ��, Ÿ�̸� �ʱ�ȭ
        if (isInit) {
            gameTime = UIManager.Instance.GetMaxGameTime();
            UIManager.Instance.UpdateClock(gameTime);  // UI ����
        }

        // �ٸ� �����̳� �ʱ�ȭ
        UIManager.Instance.ResetScore();
        SoundManager.Instance.AllAudioStop();
        SoundManager.Instance.PlaySound(gameBGM, 0.18f, 1f, true); // BGM ���

        var player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().ToggleGamePlayerActivated(true);
    }

    // ������ ��� ��ֹ��� On/Off�ϴ� ���
    private void AllObstacleActive(bool isActive) {
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in obstacles) {
            obstacle.SetActive(isActive); // ��ֹ� ��Ȱ��ȭ
        }
    }

    // �ʱ� ���ӽ��ۿ� ���Ǵ� �ʱ�ȭ
    public void StartGameSetting() {
        isSpawnStop = false; // ���� ����
        GameOver(); // ���� ���ѽð� Ÿ�̸� ����
    }

    // ���� ���� ���� ��ȯ
    public bool GetIsGameOver() => isGameOver;
}
