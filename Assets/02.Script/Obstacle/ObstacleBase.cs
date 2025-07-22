using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObstacleBase : MonoBehaviour {
    [Header("Object Settings")]
    [SerializeField] private float scaleDownSpeed = 0.1f;

    [Header("Collider Settings")]
    [SerializeField] private GameObject[] colliders;    // 감지 영역 객체

    [Header("Coin Settings")]
    //[SerializeField] private GameObject[] CoinPrefabs;      // 코인 프리팹들
    [SerializeField] private Transform[] spawnPoints;       // 코인을 소환할 위치
    [SerializeField] private float spawnProbability = 50;   // 코인 생성 확률 

    private List<Color> currentColorList = new List<Color>(); // 현재 사용중인 색상 리스트
    private float rollSpeed = .05f;
    private float rotateDir;    // T: 왼쪽 회전, F : 오른쪽 회전
    public Vector3 myLocalScale;
    private bool isStart;

    private void Start() {
        SetRatateDir(false);
        
    }

    private void OnEnable() {
        InitializeObstacle();
        SetColliderColor();
    }

    // 사용 전 초기화 구문
    private void InitializeObstacle() {
        // 로컬 스케일 기록
        myLocalScale = transform.localScale;
        
        if (!isStart) {
            isStart = true;
            return;
        }
        
        // 회전속도 랜덤
        rollSpeed = Random.Range(0.02f, 0.09f); 

        // 코인 배치
        foreach(var spawnPos in spawnPoints) {
            var ranVal = Random.Range(0, 101);
            bool isSpawned = ranVal <= spawnProbability ? true : false;
            if (isSpawned) {
                spawnPos.TryGetComponent<CoinArea>(out var coinArea);
                
                int coinIndex = Random.Range(0, 5); // 생성할 아이템 인덱스 랜덤 생성
                coinArea?.ActiveCoin(coinIndex); // 오브젝트 풀을 사용해서 아이템 활성화
            }
        }
    }

    private void Update() {
        transform.Rotate(Vector3.forward * rollSpeed * rotateDir);
        transform.localScale -= Vector3.one * scaleDownSpeed * Time.deltaTime;
    }

    // T: 왼쪽 회전, F : 오른쪽 회전
    public void SetRatateDir(bool isLeftRoll) {
        rotateDir = isLeftRoll ? 1 : -1;
    }

    private void OnDisable() {
        transform.localScale = myLocalScale;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("DeathZone")) {
            this.transform.gameObject.SetActive(false);
        }
    }

    // 사용 가능한 색상을 리스트화
    public void SetColliderColor() {
        var colorManager = ColorManager.Instance;
        if (colorManager == null) return;

        currentColorList = colorManager?.GetUseColorList();
        ChangedColliderColor();
    }

    // 콜라이더 색 변경
    private void ChangedColliderColor() {
        // 색상 및 콜라이더 초기화
        foreach (var collider in colliders) {
            collider.GetComponent<SpriteRenderer>().color = currentColorList[1];
            collider.GetComponent<Collider2D>().enabled = true;
        }

        // 랜덤한 구역을 안전 구역으로 설정 (3개)
        var ranStartIndex = Random.Range(0, colliders.Length); // 안전 구역 시작 인덱스

        // 안전 구역으로 설정될 콜라이더 인덱스들을 저장 (중복 처리 방지)
        HashSet<int> safeZoneIndices = new HashSet<int>();

        for (int i = 0; i < 3; i++) {
            int safeZoneIndex = (ranStartIndex + i) % colliders.Length;
            safeZoneIndices.Add(safeZoneIndex); // HashSet에 추가

            // 안전 구역 콜라이더 설정
            if (colliders[safeZoneIndex] != null) {
                SpriteRenderer sr = colliders[safeZoneIndex].GetComponent<SpriteRenderer>();
                if (sr != null) {
                    sr.color = currentColorList[0]; // 첫 번째 색상으로 칠하기
                }
                Collider2D col2D = colliders[safeZoneIndex].GetComponent<Collider2D>();
                if (col2D != null) {
                    col2D.enabled = false; // 콜라이더 비활성화
                }
            }
        }

        // 3. 나머지 콜라이더 3칸씩 다른 색으로 칠하기
        int colorListIndex = 1; // 안전지대 색상 다음부터 시작
        int countColored = 0;   // 안전지대를 제외하고 색칠한 콜라이더 개수

        for (int i = 0; i < colliders.Length; i++) {
            // 현재 순회하는 콜라이더 인덱스 (ranStartIndex 다음부터 시작하도록 조정)
            int currentColliderIndex = (ranStartIndex + 3 + i) % colliders.Length; // 안전지대 다음부터 시작하도록

            // 만약 현재 콜라이더가 안전 구역에 속하면 건너뜁니다.
            if (safeZoneIndices.Contains(currentColliderIndex)) {
                continue; // 안전 구역은 이미 칠해졌고 건드리지 않음
            }

            // 3개씩 묶어서 색상 변경
            if (countColored % 3 == 0 && countColored != 0) // 첫 3개는 칠해야 하므로 0이 아닐 때만 색상 인덱스 증가
            {
                colorListIndex++;
                // currentColorList의 범위를 넘어가지 않도록 모듈러 연산
                if (colorListIndex >= currentColorList.Count) {
                    colorListIndex = 1; // 또는 0으로 돌아가거나, 마지막 색을 계속 사용
                }
            }

            // 콜라이더에 색상 적용
            if (colliders[currentColliderIndex] != null) {
                SpriteRenderer sr = colliders[currentColliderIndex].GetComponent<SpriteRenderer>();
                if (sr != null) {
                    sr.color = currentColorList[colorListIndex];
                }
            }
            countColored++; // 색칠한 콜라이더 개수 증가
        }
    }
}
