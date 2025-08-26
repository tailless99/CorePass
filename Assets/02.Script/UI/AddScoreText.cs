using TMPro;
using UnityEngine;

public class AddScoreText : MonoBehaviour
{
    [SerializeField] private float upSpeed = 1f;
    [SerializeField] private float upAnimDuration;
    [SerializeField] private TextMeshProUGUI myText;

    private float timer = 0f;

    public Vector3 offset;
    private GameObject player;


    private void Awake() {
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    private void OnEnable() {
        if(player == null)
            player = FindAnyObjectByType<PlayerController>().gameObject;

        transform.position = Camera.main.WorldToScreenPoint(player.transform.position + offset);
        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, 1);
        timer = 0f; // 타이머 초기화
    }

    private void Update() {
        if(upAnimDuration >= timer) {
            timer += Time.deltaTime;

            transform.position += Vector3.up * upSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, myText.color.a - (.5f * Time.deltaTime));
        }
        else {
            EndPlayAnimation();
        }
    }

    public void EndPlayAnimation() {
        this.gameObject.SetActive(false);
    }
}
