using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 3f;

    [Header("Player Death Settings")]
    [SerializeField] private GameObject deathVFX_Prefab;

    // ������Ʈ
    private PlayerControls playerControls;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // ����
    private Vector2 moveDir;
    private bool isKnockBack;
    private float maxKnockBackTime = 0.25f;
    private float currKnockBackCoolTime = 0
        ;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls();

        // �Է�Ű ���ε�
        playerControls.Move.Move.performed += value => InputThouch(value);
        playerControls.Move.Move.canceled += _ => MoveCancle();
    }

    private void Start() {
        // �÷��̾� �� �ʱ�ȭ
        SetColliderColor();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Update() {
        Move();
    }

    private void InputThouch(InputAction.CallbackContext context) {
        var InputDir = context.ReadValue<Vector2>();
        moveDir = InputDir.normalized;
    }

    private void Move() {
        if (isKnockBack) {
            currKnockBackCoolTime += Time.deltaTime;

            if (currKnockBackCoolTime >= maxKnockBackTime) isKnockBack = false;
            else return;
        }

        if (moveDir == Vector2.zero) {
            rb.linearVelocity = Vector2.zero;
            return;
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
        var deathVFX = Instantiate(deathVFX_Prefab); // ���� ����Ʈ Ȱ��ȭ
        deathVFX.TryGetComponent<ParticleSystem>(out var particleSystem);
        var mainModule = particleSystem.main;
        mainModule.startColor = ColorManager.Instance.GetUseColorList()[0];

        SoundManager.Instance.GameOver(); // ���� ���� ���
        gameObject.SetActive(false);
    }
}
