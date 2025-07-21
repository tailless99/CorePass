using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    public Color[] colors; // 플레이어, 장애물 색상 바리에이션
    [SerializeField] List<Color> useColorList = new List<Color>(); // 현재 사용 색 리스트, 0번은 무조건 플레이어 색상


    protected override void Awake() {
        base.Awake();

        useColorList.AddRange(colors); // 리스트 초기화
        ShuffleColorList(); // 색 셔플
    }

    // 사용중인 색상 리스트를 바꾸는 기능
    public bool ShuffleColorList() {
        // 현재 플레이어 색을 변수로 저장
        Color originalFirstColor = useColorList[0];
        
        // 색상 리스트 셔플
        for (int i = useColorList.Count - 1; i > 0; i--) {
            // 0부터 i까지의 인덱스 중에서 무작위 인덱스를 선택
            int randomIndex = Random.Range(0, i + 1);

            // 현재 요소와 무작위 인덱스의 요소를 교환
            Color temp = useColorList[i];
            useColorList[i] = useColorList[randomIndex];
            useColorList[randomIndex] = temp;
        }

        // 원본 플레이어 색상이 어디있는지 찾기
        var targetIndex = useColorList.IndexOf(originalFirstColor);

        // 1~4번 요소 사이에 반드시 원본 플레이어 색상이 있도록 변경
        int originColorIndex = Random.Range(1, 5); // 바꿀 색상 인덱스
        Color tempColor = useColorList[targetIndex];
        useColorList[targetIndex] = useColorList[originColorIndex];
        useColorList[originColorIndex] = tempColor;

        return true;
    }

    // 현재 셋팅된 컬러 리스트 반환
    public List<Color> GetUseColorList() => useColorList;
}
