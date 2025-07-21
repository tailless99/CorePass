using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour {
    [Header("Object Settings")]
    [SerializeField] private float scaleDownSpeed = 0.1f;

    [Header("Collider Settings")]
    [SerializeField] private GameObject[] colliders;    // ���� ���� ��ü

    [Header("Coin Settings")]
    //[SerializeField] private GameObject[] CoinPrefabs;      // ���� �����յ�
    [SerializeField] private Transform[] spawnPoints;       // ������ ��ȯ�� ��ġ
    [SerializeField] private float spawnProbability = 50;   // ���� ���� Ȯ�� 

    private List<Color> currentColorList = new List<Color>(); // ���� ������� ���� ����Ʈ
    private float rollSpeed = .05f;
    private float rotateDir;    // T: ���� ȸ��, F : ������ ȸ��
    public Vector3 myLocalScale;

    private void Start() {
        SetRatateDir(false);
    }

    private void OnEnable() {
        InitializeObstacle();
        SetColliderColor();
    }

    // ��� �� �ʱ�ȭ ����
    private void InitializeObstacle() {
        // ���� ������ ���
        myLocalScale = transform.localScale;

        // ȸ���ӵ� ����
        rollSpeed = Random.Range(0.02f, 0.09f); 

        // ���� ��ġ
        foreach(var spawnPos in spawnPoints) {
            var ranVal = Random.Range(0, 101);
            bool isSpawned = ranVal <= spawnProbability ? true : false;
            if (isSpawned) {
                spawnPos.TryGetComponent<CoinArea>(out var coinArea);
                
                int coinIndex = Random.Range(0, 3); // ������ ���� �ε��� ���� ����
                coinArea?.ActiveCoin(coinIndex); // ������Ʈ Ǯ�� ����ؼ� ���� Ȱ��ȭ
                //Instantiate(CoinPrefabs[coinIndex], spawnPos);
            }
        }
    }

    private void Update() {
        transform.Rotate(Vector3.forward * rollSpeed * rotateDir);
        transform.localScale -= Vector3.one * scaleDownSpeed * Time.deltaTime;
    }

    // T: ���� ȸ��, F : ������ ȸ��
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

    // ��� ������ ������ ����Ʈȭ
    public void SetColliderColor() {
        var colorManager = ColorManager.Instance;
        if (colorManager == null) return;

        currentColorList = colorManager?.GetUseColorList();
        ChangedColliderColor();
    }

    // �ݶ��̴� �� ����
    private void ChangedColliderColor() {
        // ���� �� �ݶ��̴� �ʱ�ȭ
        foreach (var collider in colliders) {
            collider.GetComponent<SpriteRenderer>().color = currentColorList[1];
            collider.GetComponent<Collider2D>().enabled = true;
        }

        // ���� ���� ������������ �ٲٱ�
        var ranIndex = Random.Range(0, colliders.Length);
        int currentIndex = 0;

        // ranIndex�� �� ���� �� ���� �ε����� ���� �������� ����
        for (int i = 0; i < 3; i++) {
            // colliders.Length�� �̿��Ͽ� �ε����� �迭 ������ ����� �ʵ��� ����
            currentIndex = (ranIndex + i) % colliders.Length;

            colliders[currentIndex].GetComponent<SpriteRenderer>().color = currentColorList[0];
            colliders[currentIndex].GetComponent<Collider2D>().enabled = false;
        }

        // 0�� �̹� ������ ĥ������, �� ������ 1���� ����
        int colorListIndex = 1;
        int loofCount = 0;
        /*
        foreach (var collider in colliders) {
            // ��������� ������ ���� �ƴ� �ٸ� �ݶ��̴��� 3�� ������ �ٸ� ������ ����
            if (collider.GetComponent<SpriteRenderer>().color != currentColorList[0]) {
                // 4��° �������� colorList�� �ε������� ����
                if (loofCount != 0 && loofCount % 3 == 0) colorListIndex++;
                collider.GetComponent<SpriteRenderer>().color = currentColorList[colorListIndex];
            }
            loofCount++;
        }
        */

        int startIndex = currentIndex + 1; // �������� ���� �ε���

        // �������븦 ������ ������ ��� �� ����
        for (int i = 0; i < colliders.Length; i++) {
            // colliders.Length�� �̿��Ͽ� �ε����� �迭 ������ ����� �ʵ��� ����
            currentIndex = (startIndex + i) % colliders.Length;
            if (currentIndex == ranIndex) break;

            if (loofCount != 0 && loofCount % 3 == 0) colorListIndex++;
            colliders[currentIndex].GetComponent<SpriteRenderer>().color = currentColorList[colorListIndex];
            loofCount++;
        }
    }
}
