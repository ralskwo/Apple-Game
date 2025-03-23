// AppleFactory.cs
using UnityEngine;

public class AppleFactory : MonoBehaviour, IAppleFactory
{
    [SerializeField] private GameObject applePrefab;       // Apple 프리팹
    [SerializeField] private RectTransform parentTransform;  // Apple들이 생성될 부모 (예: BoardManager의 boardTransform)

    void Awake()
    {
        // 인터페이스 타입으로 자신을 등록합니다.
        ServiceLocator.Register<IAppleFactory>(this);
    }

    public Apple CreateApple(Vector2 anchoredPosition, Vector2Int boardPosition)
    {
        // 지정된 부모 아래에 Apple 프리팹을 생성
        GameObject appleObj = Instantiate(applePrefab, parentTransform);
        appleObj.transform.SetAsFirstSibling();  // 항상 첫 번째 자식으로 배치
        RectTransform rt = appleObj.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPosition;

        Apple apple = appleObj.GetComponent<Apple>();
        apple.SetRandomValue();
        apple.BoardPosition = boardPosition;
        return apple;
    }
}
