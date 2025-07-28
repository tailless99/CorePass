using UnityEngine;

public class ResumeProductionContainer : MonoBehaviour
{
    [SerializeField] private GameObject child;

    // 다음 연출 활성화
    public void NextProdectStart() => child.SetActive(true);

    public void EndProduction() {
        child.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
