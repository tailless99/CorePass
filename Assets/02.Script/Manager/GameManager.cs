using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Game Settings")]
    [SerializeField] private float gameTime = 120f; // 1판에 걸리는 시간 제한
    [SerializeField] private float maxSpawnTime = .5f; // 장해물 소환 주기

    [Header("Change Color Event Settings")]
    [SerializeField] private GameObject changeColorEffect_Prefab;
    [SerializeField] private float colorChangeEventCoolTime = 23f; // 색변환 이벤트 쿨타임

    // 멤버 변수
    private float changeColorTimer; // 색변환 이벤트 타이머
    private float spawnTimer = 0; // 장해물 활성화 타이머


    // 사용 안하기로 계획 변경 - 잠정 보류
    // 패턴 실행에 사용되는 변수
    /*
        private int randomPatten; // 랜덤 패턴 실행 키
        private int maxSpawnCount; // 소환할 최대 장해물 숫자
        private int currentSpawnCount; // 현재 소환한 장해물 수
        private bool isPattening;
    */


    private void Awake() {
        changeColorEffect_Prefab.SetActive(false);

        // 초기화
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

        if (gameTime > 119) { // 사각형 반환
        //if (gameTime > 105) { // 사각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.SquareObj);
        }
        //else if (gameTime > 75) { // 오각형 반환
        else if (gameTime > 118) { // 오각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.PentagonObj);
        }
        else if (gameTime > 117) { // 육각형 반환
        //else if (gameTime > 55) { // 육각형 반환
            returnObj = ObjectManager.Instance.MakeObj(PoolType.hexagonObj);
        }
        else if (gameTime > 116) { // 칠각형 반환
        //else if (gameTime > 35) { // 칠각형 반환
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
        GetObstacleObj();
    }

    /*
    // 장해물 소환 패턴
    private void SelectObstaclePatten() {
        // 패턴 중이 아니라면
        if (!isPattening) {
            // 초기화
            isPattening = true; // 패턴 실행 플래그
            randomPatten = Random.Range(0, 101); // 0 ~ 100% 사이 랜덤 가중치 출력
            currentSpawnCount = 0; // 현재 소환한 장해물 수 초기화
            spawnTimer = 0; // 장해물 활성화 타이머 초기화
        }
        // 패턴 중이라면
        else {
            // 가중치에 따라 패턴 실행
            if (randomPatten > 70) { // 30% 확률의 패턴
                Patten_RandomSafeBlock();
            }
        }
    }

#region PattenFunc
    // 랜덤한 곳이 안전지대가 되는 패턴
    private void Patten_RandomSafeBlock() {
        maxSpawnCount = 6; // 소환할 최대 장해물 숫자
        maxSpawnTime = .2f; // 소환주기

        while(currentSpawnCount < maxSpawnCount) {
            spawnTimer += Time.deltaTime;

            if (spawnTimer < maxSpawnTime) continue;
            spawnTimer = 0f; // 재사용을 위해 초기화

            // 장해물 생성
            var obj = GetObstacleObj();
            obj.TryGetComponent<ObstacleBase>(out var obstacleObj);
            
            // 생성한 장해물 클래스에 들어가서 안전지대 설정하는 부분을 리빌딩하고
            // 안전지대를 내가 고를 수 있도록 커스텀 해야함.

            // 후처리
            currentSpawnCount++; // 소환 카운트 증가
        }

        isPattening = false;
    }
#endregion
    */
}
