using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [Header("Game Settings")]
    [SerializeField] private float gameTime = 120f; // 1판에 걸리는 시간 제한
    [SerializeField] private float maxSpawnTime = .5f; // 장해물 소환 주기

    [Header("Change Color Event Settings")]
    [SerializeField] private GameObject changeColorEffect_Prefab;
    [SerializeField] private float colorChangeEventCoolTime = 23f; // 색변환 이벤트 쿨타임

    // 멤버 변수
    private float changeColorTimer; // 색변환 이벤트 타이머
    private float spawnTimer = 0; // 장해물 활성화 타이머
    private bool isFever; // 피버 상태 토글 변수


    // 사용 안하기로 계획 변경 - 잠정 보류
    // 패턴 실행에 사용되는 변수
    /*
        private int randomPatten; // 랜덤 패턴 실행 키
        private int maxSpawnCount; // 소환할 최대 장해물 숫자
        private int currentSpawnCount; // 현재 소환한 장해물 수
        private bool isPattening;
    */


    protected override void Awake() {
        base.Awake();

        // 초기화
        changeColorEffect_Prefab.SetActive(false);
        changeColorTimer = colorChangeEventCoolTime;
    }

    private void Update() {
        // 시간 갱신
        UpdateClock();
        // 색변환 이벤트
        changeColorEvent();
        // 장해물 소환
        SpawnObstacle();

        // 장해물 소환 패턴
        //SelectObstaclePatten();
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
        gameTime -= Time.deltaTime; // 시간 감소
        UIManager.Instance.UpdateClock(gameTime);  // UI 갱신
    }

    /// <summary>
    /// 장해물을 반환하는 기능을 제공
    /// 시간에 따라 반환하는 도형이 달라진다.
    /// </summary>
    private GameObject GetObstacleObj() {
        // 반환 객체
        GameObject returnObj = null;

        if (gameTime > 105) { // 사각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.SquareObj);
        }
        else if (gameTime > 75) { // 오각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.PentagonObj);
        }
        else if (gameTime > 55) { // 육각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.hexagonObj);
        }
        else if (gameTime > 35) { // 칠각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.heptagonObj);
        }
        else { // 팔각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.octagonObj);
        }

        return returnObj;
    }

    // 주기마다 장해물 소환
    private void SpawnObstacle() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer < maxSpawnTime) return; // 소환 쿨타임이 다되지 않았다면 반환

        spawnTimer = 0;

        // 장해물 소환
        var obstacle = GetObstacleObj();
        obstacle.GetComponent<ObstacleBase>().OnToggleColliderActive(!isFever); // 콜라이더 활성/비활성
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
}
