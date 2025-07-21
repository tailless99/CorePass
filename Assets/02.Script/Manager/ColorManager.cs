using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    public Color[] colors; // �÷��̾�, ��ֹ� ���� �ٸ����̼�
    [SerializeField] List<Color> useColorList = new List<Color>(); // ���� ��� �� ����Ʈ, 0���� ������ �÷��̾� ����


    protected override void Awake() {
        base.Awake();

        useColorList.AddRange(colors); // ����Ʈ �ʱ�ȭ
        ShuffleColorList(); // �� ����
    }

    // ������� ���� ����Ʈ�� �ٲٴ� ���
    public bool ShuffleColorList() {
        // ���� �÷��̾� ���� ������ ����
        Color originalFirstColor = useColorList[0];
        
        // ���� ����Ʈ ����
        for (int i = useColorList.Count - 1; i > 0; i--) {
            // 0���� i������ �ε��� �߿��� ������ �ε����� ����
            int randomIndex = Random.Range(0, i + 1);

            // ���� ��ҿ� ������ �ε����� ��Ҹ� ��ȯ
            Color temp = useColorList[i];
            useColorList[i] = useColorList[randomIndex];
            useColorList[randomIndex] = temp;
        }

        // ���� �÷��̾� ������ ����ִ��� ã��
        var targetIndex = useColorList.IndexOf(originalFirstColor);

        // 1~4�� ��� ���̿� �ݵ�� ���� �÷��̾� ������ �ֵ��� ����
        int originColorIndex = Random.Range(1, 5); // �ٲ� ���� �ε���
        Color tempColor = useColorList[targetIndex];
        useColorList[targetIndex] = useColorList[originColorIndex];
        useColorList[originColorIndex] = tempColor;

        return true;
    }

    // ���� ���õ� �÷� ����Ʈ ��ȯ
    public List<Color> GetUseColorList() => useColorList;
}
