using UnityEngine;
using UnityEngine.InputSystem;
using static GameEndContainer;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 3f;

    [Header("Player Death Settings")]
    [SerializeField] private GameObject deathVFX_Prefab;

    // ������Ʈ
    private PlayerControls playerControls;
    private SpriteRenderer spriteRenderer;
    private Collider2D myCollder;
    private TrailRenderer trailRenderer;
    private Rigidbody2D rb;

    // ����
    private Vector2 moveDir;
    private Vector2 originPos; // ���� ��ġ
    private bool isKnockBack;
    private float maxKnockBackTime = 0.25f;
    private float currKnockBackCoolTime = 0;

    // �÷��̾� �������� ����
    private bool isGameEnd;
    [SerializeField] private float CallGameEndTime = 2f; // ���� ���� ������� �ɸ��� �ð�
    private float GameEndTimer = 0; // ���� ���� Ÿ�̸�


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollder = GetComponent<Collider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerControls = new PlayerControls();

        // �Է�Ű ���ε�
        playerControls.Move.Move.performed += value => InputThouch(value);
        playerControls.Move.Move.canceled += _ => MoveCancle();
    }

    private void Start() {
        // �÷��̾� �� �ʱ�ȭ
        SetColliderColor();
        originPos = transform.position; // ���� ��ġ ��� (1����)
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Update() {
        Move(); // ������ ���
        GameEndEvent(); // ���� ���� ���
    }

    private void InputThouch(InputAction.CallbackContext context) {
        if (context.canceled) {
            moveDir = Vector2.zero;
            return;
        }

        var InputDir = context.ReadValue<Vector2>();
        
        if(InputDir.magnitude < .1f) {
            moveDir = Vector2.zero;
        }
        else
            moveDir = InputDir.normalized;
    }

    private void Move() {
        if (isKnockBack) {
            currKnockBackCoolTime += Time.deltaTime;
            if (currKnockBackCoolTime >= maxKnockBackTime) isKnockBack = false;
            else return;
        }

        rb.linearVelocity = moveDir * moveSpeed;
    }

    private void MoveCancle() {
        moveDir = Vector2.zero;
    }

    public void StartKnockBack(Vector3 dir, float knockBackPower) {
        isKnockBack = true;
        currKnockBackCoolTime = 0;
        rb.AddForce(dir * knockBackPower, ForceMode2D.Impulse);
    }

    public void SetColliderColor() {
        var playerColor = ColorManager.Instance.GetUseColorList()[0];
        spriteRenderer.color = playerColor;
    }

    public void PlayerDeath() {
        // ���� �÷���
        isGameEnd = true;

        // ���� ����Ʈ Ȱ��ȭ
        var deathVFX = Instantiate(deathVFX_Prefab); 
        deathVFX.TryGetComponent<ParticleSystem>(out var particleSystem);
        var mainModule = particleSystem.main;
        mainModule.startColor = ColorManager.Instance.GetUseColorList()[0];

        // �÷��̾� ���� ��ó��
        SoundManager.Instance.GameOver(); // ���� ���� ���
        ToggleGamePlayerActivated(false); // ������Ʈ ��Ȱ��ȭ
        GameManager.Instance.GameOver();
    }

    // ����۽� �÷��̾� ��� �ٽ� Ȱ��ȭ
    public void ToggleGamePlayerActivated(bool isActived) {
        spriteRenderer.enabled = isActived;
        myCollder.enabled = isActived;
        trailRenderer.enabled = isActived;

        transform.position = originPos;
    }

    // �÷��̾� ��� ��, ���� ���� �̺�Ʈ
    private void GameEndEvent() {
        if (!isGameEnd || !GameManager.Instance.GetIsGameOver()) return;
        
        GameEndTimer += Time.deltaTime; // Ÿ�̸� ����

        // ���� ������� ��ٸ��� �ð��� �����ٸ�
        if (GameEndTimer >= CallGameEndTime) {
            // �ߺ� ���� ���� �� �ٽ��ϱ� ����� ���� �ʱ�ȭ
            GameEndTimer = 0;
            isGameEnd = false;

            // ���� ��ü ���࿡ ���� ���� ���� ó��
            UIManager.Instance.ShowGameEndView(true);
        }
    }

    // ���� ���� ����
    public bool GetIsGameEnd() => isGameEnd;
}
