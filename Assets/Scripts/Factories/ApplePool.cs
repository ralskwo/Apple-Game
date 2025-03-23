// ApplePool.cs
using System.Collections.Generic;
using UnityEngine;

public class ApplePool : MonoBehaviour, IAppleFactory
{
    [SerializeField] private GameObject applePrefab;       // Apple 프리팹
    [SerializeField] private RectTransform parentTransform;  // Apple들이 생성될 부모 (예: BoardManager의 boardTransform)
    [SerializeField] private int initialPoolSize = 36;        // 초기 풀 크기

    private Queue<Apple> poolQueue;

    void Awake()
    {
        poolQueue = new Queue<Apple>();
        // IAppleFactory 인터페이스로 자신을 등록합니다.
        ServiceLocator.Register<IAppleFactory>(this);

        // 초기 Apple 객체들을 풀에 미리 생성해둡니다.
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(applePrefab, parentTransform);
            obj.transform.SetAsFirstSibling();
            obj.SetActive(false);
            Apple apple = obj.GetComponent<Apple>();
            poolQueue.Enqueue(apple);
        }
    }

    public Apple CreateApple(Vector2 anchoredPosition, Vector2Int boardPosition)
    {
        Apple apple;
        if (poolQueue.Count > 0)
        {
            apple = poolQueue.Dequeue();
            apple.gameObject.SetActive(true);
        }
        else
        {
            GameObject obj = Instantiate(applePrefab, parentTransform);
            obj.transform.SetAsFirstSibling();
            apple = obj.GetComponent<Apple>();
        }
        RectTransform rt = apple.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPosition;
        apple.SetRandomValue();
        apple.BoardPosition = boardPosition;
        return apple;
    }

    public void ReturnApple(Apple apple)
    {
        // Apple이 제거될 때 호출: Apple을 비활성화 후 풀에 반환합니다.
        apple.gameObject.SetActive(false);
        poolQueue.Enqueue(apple);
    }
}
