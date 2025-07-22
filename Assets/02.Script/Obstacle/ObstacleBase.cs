using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    private bool isStart;

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
        
        if (!isStart) {
            isStart = true;
            return;
        }
        
        // ȸ���ӵ� ����
        rollSpeed = Random.Range(0.02f, 0.09f); 

        // ���� ��ġ
        foreach(var spawnPos in spawnPoints) {
            var ranVal = Random.Range(0, 101);
            bool isSpawned = ranVal <= spawnProbability ? true : false;
            if (isSpawned) {
                spawnPos.TryGetComponent<CoinArea>(out var coinArea);
                
                int coinIndex = Random.Range(0, 5); // ������ ������ �ε��� ���� ����
                coinArea?.ActiveCoin(coinIndex); // ������Ʈ Ǯ�� ����ؼ� ������ Ȱ��ȭ
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

        // ������ ������ ���� �������� ���� (3��)
        var ranStartIndex = Random.Range(0, colliders.Length); // ���� ���� ���� �ε���

        // ���� �������� ������ �ݶ��̴� �ε������� ���� (�ߺ� ó�� ����)
        HashSet<int> safeZoneIndices = new HashSet<int>();

        for (int i = 0; i < 3; i++) {
            int safeZoneIndex = (ranStartIndex + i) % colliders.Length;
            safeZoneIndices.Add(safeZoneIndex); // HashSet�� �߰�

            // ���� ���� �ݶ��̴� ����
            if (colliders[safeZoneIndex] != null) {
                SpriteRenderer sr = colliders[safeZoneIndex].GetComponent<SpriteRenderer>();
                if (sr != null) {
                    sr.color = currentColorList[0]; // ù ��° �������� ĥ�ϱ�
                }
                Collider2D col2D = colliders[safeZoneIndex].GetComponent<Collider2D>();
                if (col2D != null) {
                    col2D.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
                }
            }
        }

        // 3. ������ �ݶ��̴� 3ĭ�� �ٸ� ������ ĥ�ϱ�
        int colorListIndex = 1; // �������� ���� �������� ����
        int countColored = 0;   // �������븦 �����ϰ� ��ĥ�� �ݶ��̴� ����

        for (int i = 0; i < colliders.Length; i++) {
            // ���� ��ȸ�ϴ� �ݶ��̴� �ε��� (ranStartIndex �������� �����ϵ��� ����)
            int currentColliderIndex = (ranStartIndex + 3 + i) % colliders.Length; // �������� �������� �����ϵ���

            // ���� ���� �ݶ��̴��� ���� ������ ���ϸ� �ǳʶݴϴ�.
            if (safeZoneIndices.Contains(currentColliderIndex)) {
                continue; // ���� ������ �̹� ĥ������ �ǵ帮�� ����
            }

            // 3���� ��� ���� ����
            if (countColored % 3 == 0 && countColored != 0) // ù 3���� ĥ�ؾ� �ϹǷ� 0�� �ƴ� ���� ���� �ε��� ����
            {
                colorListIndex++;
                // currentColorList�� ������ �Ѿ�� �ʵ��� ��ⷯ ����
                if (colorListIndex >= currentColorList.Count) {
                    colorListIndex = 1; // �Ǵ� 0���� ���ư��ų�, ������ ���� ��� ���
                }
            }

            // �ݶ��̴��� ���� ����
            if (colliders[currentColliderIndex] != null) {
                SpriteRenderer sr = colliders[currentColliderIndex].GetComponent<SpriteRenderer>();
                if (sr != null) {
                    sr.color = currentColorList[colorListIndex];
                }
            }
            countColored++; // ��ĥ�� �ݶ��̴� ���� ����
        }
    }
}
