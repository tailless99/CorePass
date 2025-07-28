using UnityEngine;

public class ResumeProductionContainer : MonoBehaviour
{
    [SerializeField] private GameObject child;

    // ���� ���� Ȱ��ȭ
    public void NextProdectStart() => child.SetActive(true);

    public void EndProduction() {
        child.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
