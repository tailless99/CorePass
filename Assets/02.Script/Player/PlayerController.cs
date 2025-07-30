using UnityEngine;
using UnityEngine.InputSystem;
using static GameEndContainer;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 3f;

    [Header("Player Death Settings")]
    [SerializeField] private GameObject deathVFX_Prefab;

    // 컴포넌트
    private PlayerControls playerControls;
    private SpriteRenderer spriteRenderer;
    private Collider2D myCollder;
    private TrailRenderer trailRenderer;
    private Rigidbody2D rb;

    // 변수
    private Vector2 moveDir;
    private Vector2 originPos; // 시작 위치
    private bool isKnockBack;
    private float maxKnockBackTime = 0.25f;
    private float currKnockBackCoolTime = 0;

    // 플레이어 죽음관련 변수
    private bool isGameEnd;
    [SerializeField] private float CallGameEndTime = 2f; // 게임 엔딩 실행까지 걸리는 시간
    private float GameEndTimer = 0; // 게임 엔딩 타이머


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollder = GetComponent<Collider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerControls = new PlayerControls();

        // 입력키 바인딩
        playerControls.Move.Move.performed += value => InputThouch(value);
        playerControls.Move.Move.canceled += _ => MoveCancle();
    }

    private void Start() {
        // 플레이어 색 초기화
        SetColliderColor();
        originPos = transform.position; // 시작 위치 기록 (1번만)
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Update() {
        Move(); // 움직임 기능
        GameEndEvent(); // 게임 오버 기능
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
        // 죽음 플래그
        isGameEnd = true;

        // 죽음 이펙트 활성화
        var deathVFX = Instantiate(deathVFX_Prefab); 
        deathVFX.TryGetComponent<ParticleSystem>(out var particleSystem);
        var mainModule = particleSystem.main;
        mainModule.startColor = ColorManager.Instance.GetUseColorList()[0];

        // 플레이어 죽음 후처리
        SoundManager.Instance.GameOver(); // 죽음 사운드 재생
        ToggleGamePlayerActivated(false); // 컴포넌트 비활성화
        GameManager.Instance.GameOver();
    }

    // 재시작시 플레이어 기능 다시 활성화
    public void ToggleGamePlayerActivated(bool isActived) {
        spriteRenderer.enabled = isActived;
        myCollder.enabled = isActived;
        trailRenderer.enabled = isActived;

        transform.position = originPos;
    }

    // 플레이어 사망 후, 게임 종료 이벤트
    private void GameEndEvent() {
        if (!isGameEnd || !GameManager.Instance.GetIsGameOver()) return;
        
        GameEndTimer += Time.deltaTime; // 타이머 증가

        // 게임 엔드까지 기다리는 시간을 지난다면
        if (GameEndTimer >= CallGameEndTime) {
            // 중복 실행 방지 및 다시하기 기능을 위해 초기화
            GameEndTimer = 0;
            isGameEnd = false;

            // 게임 전체 진행에 대해 게임 오버 처리
            UIManager.Instance.ShowGameEndView(true);
        }
    }

    // 게임 종료 여부
    public bool GetIsGameEnd() => isGameEnd;
}
