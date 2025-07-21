using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 3f;

    [Header("Player Death Settings")]
    [SerializeField] private GameObject deathVFX_Prefab;

    // 컴포넌트
    private PlayerControls playerControls;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // 변수
    private Vector2 moveDir;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls();

        // 입력키 바인딩
        playerControls.Move.Move.performed += value => InputThouch(value);
        playerControls.Move.Move.canceled += _ => MoveCancle();
    }

    private void Start() {
        // 플레이어 색 초기화
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
        if (moveDir == Vector2.zero) {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveDir * moveSpeed;
    }

    private void MoveCancle() {
        moveDir = Vector2.zero;
    }

    public void SetColliderColor() {
        var playerColor = ColorManager.Instance.GetUseColorList()[0];
        spriteRenderer.color = playerColor;
    }

    public void PlayerDeath() {
        var deathVFX = Instantiate(deathVFX_Prefab); // 죽음 이펙트 활성화
        deathVFX.TryGetComponent<ParticleSystem>(out var particleSystem);
        var mainModule = particleSystem.main;
        mainModule.startColor = ColorManager.Instance.GetUseColorList()[0];

        SoundManager.Instance.GameOver(); // 죽음 사운드 재생
        gameObject.SetActive(false);
    }
}
