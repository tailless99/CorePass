using GoogleMobileAds.Api;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [Header("Game Settings")]
    [SerializeField] private float gameTime = 120f; // 1판에 걸리는 시간 제한
    [SerializeField] private float maxSpawnTime = .5f; // 장해물 소환 주기

    [Header("Change Color Event Settings")]
    [SerializeField] private GameObject changeColorEffect_Prefab;
    [SerializeField] private float colorChangeEventCoolTime = 23f; // 색변환 이벤트 쿨타임

    // 멤버 변수
    private int gameLevel;
    private float changeColorTimer; // 색변환 이벤트 타이머
    private float spawnTimer = 0; // 장해물 활성화 타이머
    private bool isFever; // 피버 상태 토글 변수
    private bool isSpawnStop; // 장애물 소환 제어 플래그 변수
    private bool isGameOver;

    protected override void Awake() {
        base.Awake();

        // 초기화
        changeColorEffect_Prefab.SetActive(false);
        changeColorTimer = colorChangeEventCoolTime;
    }

    private void Start() {
        // 이벤트 구독
        EventBusManager.Instance.Subscribe<RestartAnimationFinishedEvent>(_ => ReStartGameSetting());
        EventBusManager.Instance.Subscribe<FeverTimeStartedEvent>(_=> SetFeverState(true));
        EventBusManager.Instance.Subscribe<FeverTimeFinishedEvent>(_=> SetFeverState(false));
    }

    private void OnEnable() {
        gameTime = UIManager.Instance.GetMaxGameTime();
        UIManager.Instance.UpdateClock(gameTime);  // UI 갱신

        isSpawnStop = true;
        isGameOver = true;
    }

    private void Update() {
        // 시간 갱신
        UpdateClock();
        // 색변환 이벤트
        changeColorEvent();
        // 장해물 소환
        SpawnObstacle();
    }

    // 색변환 이벤트
    public void changeColorEvent() {
        // 타이머 처리
        changeColorTimer -= Time.deltaTime;
        if (changeColorTimer > 0) return;
        changeColorTimer = colorChangeEventCoolTime; // 다시 초기화

        changeColorEffect_Prefab.SetActive(true); // 연출 활성화

        // 색상 리스트 변경 로직
        if (ColorManager.Instance.ShuffleColorList()) {

            // 필드의 오브젝트의 색상 재배치 로직
            // 1. 플레이어 색 변환
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().SetColliderColor(); // 색변환 로직

            // 2. 필드의 장해물 색변환
            var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (var obstacle in obstacles) {
                obstacle.GetComponent<ObstacleBase>().SetColliderColor(); // 색변환 로직
            }
        }
    }

    // 시간 갱신
    public void UpdateClock() {
        // 게임 오버시 시간이 흐르지 않음
        if (isGameOver) return;

        gameTime -= Time.deltaTime; // 시간 감소
        UIManager.Instance.UpdateClock(gameTime);  // UI 갱신

        // 게임 클리어 처리
        if (gameTime <= 0 && !isGameOver) {
            // 게임종료 처리
            isSpawnStop = false;
            GameOver();

            // 게임 클리어 이벤트 실행
            EventBusManager.Instance.Publish(new GameClearEvent());
        }
    }

    /// <summary>
    /// 장해물을 반환하는 기능을 제공
    /// 시간에 따라 반환하는 도형이 달라진다.
    /// </summary>
    private GameObject GetObstacleObj() {
        // 반환 객체
        GameObject returnObj = null;
        gameLevel = 0;

        if (gameTime > 105) { // 사각형 반환
            gameLevel = 1;
        }
        else if (gameTime > 75) { // 오각형 반환
            gameLevel = 2;
        }
        else if (gameTime > 55) { // 육각형 반환
            gameLevel = 3;
        }
        else if (gameTime > 35) { // 칠각형 반환
            gameLevel = 4;
        }
        else if (gameTime > 4) { // 팔각형 반환
            gameLevel = 5;
        }
        else {
            gameLevel = 6;
        }

        switch (gameLevel) {
            case 1:
                returnObj = ObjectManager.Instance.MakeObj(PoolType.SquareObj);
                break;
            case 2:
                returnObj = ObjectManager.Instance.MakeObj(PoolType.PentagonObj);
                break;
            case 3:
                returnObj = ObjectManager.Instance.MakeObj(PoolType.hexagonObj);
                break;
            case 4:
                returnObj = ObjectManager.Instance.MakeObj(PoolType.heptagonObj);
                break;
            case 5:
                returnObj = ObjectManager.Instance.MakeObj(PoolType.octagonObj);
                break;
            case 6:
                returnObj = null;
                break;
        }

        return returnObj;
    }

    // 주기마다 장해물 소환
    private void SpawnObstacle() {
        if (isSpawnStop) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < maxSpawnTime) return; // 소환 쿨타임이 다되지 않았다면 반환

        spawnTimer = 0;

        // 장해물 소환
        var obstacle = GetObstacleObj();
        obstacle.GetComponent<ObstacleBase>()?.OnToggleColliderActive(!isFever); // 콜라이더 활성/비활성
    }

    public void SetFeverState(bool isFevered) {
        isFever = isFevered;

        // 피버 시작 시 모든 장애물의 콜라이더 비활성화
        if (isFevered) {
            var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (var obstacle in obstacles) {
                obstacle.GetComponent<ObstacleBase>().OnToggleColliderActive(!isFevered); // 콜라이더 활성/비활성
            }
        }
    }

    // 게임 오버 상태 변환
    public void GameOver() => isGameOver = !isGameOver;

    // 게임 시간 반환
    public float GetGameTime() => gameTime;

    // 광고보고 다시 시작하는 기능
    public void ResumeGameAfterAd() {
        // 맵상에서 장애물 제거
        isSpawnStop = true; // 소환 막는 플래그
        AllObstacleActive(false);

        // 재시작 연출 이벤트 실행
        EventBusManager.Instance.Publish(new RestartAnimationStartedEvent());
    }

    // 게임을 다시 시작하기 위한 전처리
    public void ReStartGameSetting(bool isInit = false) {
        StartGameSetting();
        AllObstacleActive(false);

        // 초기화 시, 타이머 초기화
        if (isInit) {
            gameTime = UIManager.Instance.GetMaxGameTime();
            EventBusManager.Instance.Publish(new GameOver_ResetUIEvent()); // UI 초기화 이벤트 실행
        }
        changeColorTimer = colorChangeEventCoolTime;

        // 다른 컨테이너 초기화
        EventBusManager.Instance.Publish(new GameOver_ResetSoundEvent()); // 사운드 초기화 이벤트 실행

        var player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().ToggleGamePlayerActivated(true);
    }

    // 씬상의 모든 장애물을 On/Off하는 기능
    private void AllObstacleActive(bool isActive) {
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in obstacles) {
            obstacle.SetActive(isActive); // 장애물 비활성화
        }
    }

    // 초기 게임시작에 사용되는 초기화
    public void StartGameSetting() {
        isSpawnStop = false; // 스폰 시작
        GameOver(); // 게임 제한시간 타이머 시작
    }

    // 게임 종료 여부 반환
    public bool GetIsGameOver() => isGameOver;

    // 게임 난이도 반환
    public int GetGameLevel() => gameLevel;
}
