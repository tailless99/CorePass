using NUnit.Framework;
using System.Collections.Generic;
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

        // 회전속도 랜덤
        rollSpeed = Random.Range(0.02f, 0.09f); 

        // 코인 배치
        foreach(var spawnPos in spawnPoints) {
            var ranVal = Random.Range(0, 101);
            bool isSpawned = ranVal <= spawnProbability ? true : false;
            if (isSpawned) {
                spawnPos.TryGetComponent<CoinArea>(out var coinArea);
                
                int coinIndex = Random.Range(0, 3); // 생성할 코인 인덱스 랜덤 생성
                coinArea?.ActiveCoin(coinIndex); // 오브젝트 풀을 사용해서 코인 활성화
                //Instantiate(CoinPrefabs[coinIndex], spawnPos);
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

        // 랜덤 구역 안전구역으로 바꾸기
        var ranIndex = Random.Range(0, colliders.Length);
        int currentIndex = 0;

        // ranIndex와 그 다음 두 개의 인덱스를 안전 구역으로 변경
        for (int i = 0; i < 3; i++) {
            // colliders.Length를 이용하여 인덱스가 배열 범위를 벗어나지 않도록 조정
            currentIndex = (ranIndex + i) % colliders.Length;

            colliders[currentIndex].GetComponent<SpriteRenderer>().color = currentColorList[0];
            colliders[currentIndex].GetComponent<Collider2D>().enabled = false;
        }

        // 0은 이미 위에서 칠했으니, 그 다음인 1부터 염색
        int colorListIndex = 1;
        int loofCount = 0;
        /*
        foreach (var collider in colliders) {
            // 안전지대로 설정한 색이 아닌 다른 콜라이더는 3개 단위로 다른 색으로 염색
            if (collider.GetComponent<SpriteRenderer>().color != currentColorList[0]) {
                // 4번째 루프마다 colorList의 인덱스값을 증가
                if (loofCount != 0 && loofCount % 3 == 0) colorListIndex++;
                collider.GetComponent<SpriteRenderer>().color = currentColorList[colorListIndex];
            }
            loofCount++;
        }
        */

        int startIndex = currentIndex + 1; // 안전지대 다음 인덱스

        // 안전지대를 제외한 나머지 블록 색 변경
        for (int i = 0; i < colliders.Length; i++) {
            // colliders.Length를 이용하여 인덱스가 배열 범위를 벗어나지 않도록 조정
            currentIndex = (startIndex + i) % colliders.Length;
            if (currentIndex == ranIndex) break;

            if (loofCount != 0 && loofCount % 3 == 0) colorListIndex++;
            colliders[currentIndex].GetComponent<SpriteRenderer>().color = currentColorList[colorListIndex];
            loofCount++;
        }
    }
}
