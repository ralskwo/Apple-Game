// BoardManager.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    // applePrefab는 ApplePool에서 관리하므로 여기서는 제거합니다.
    [SerializeField] private int rows = 6;
    [SerializeField] private int cols = 6;
    [SerializeField] private float spacing = 1.0f;
    public RectTransform boardTransform;

    private Apple[,] apples;

    void Awake()
    {
        ServiceLocator.Register<BoardManager>(this);
        if (boardTransform == null)
            boardTransform = GetComponent<RectTransform>();
    }

    public void GenerateBoard()
    {
        // Apple 생성은 IAppleFactory (즉, ApplePool)을 통해 처리합니다.
        IAppleFactory appleFactory = ServiceLocator.Get<IAppleFactory>();

        float boardWidth = (cols - 1) * spacing;
        float boardHeight = (rows - 1) * spacing;
        Vector2 computedStartPos = new Vector2(-boardWidth / 2, -boardHeight / 2);

        apples = new Apple[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector2 pos = computedStartPos + new Vector2(j * spacing, i * spacing);
                Apple apple = appleFactory.CreateApple(pos, new Vector2Int(i, j));
                apples[i, j] = apple;
            }
        }
    }

    public void RemoveApples(List<Apple> selectedApples)
    {
        // AppleFactory를 ApplePool로 가져와 사용합니다.
        IAppleFactory factory = ServiceLocator.Get<IAppleFactory>();
        ApplePool applePool = factory as ApplePool;
        foreach (var apple in selectedApples)
        {
            apple.PlayRemovalAnimation();
            // 애니메이션이 완료된 후 풀에 반환하도록 할 수 있으나, 여기서는 즉시 반환합니다.
            applePool.ReturnApple(apple);
            apples[apple.BoardPosition.x, apple.BoardPosition.y] = null;
        }
    }

    public Apple[,] BoardArray
    {
        get { return apples; }
    }
}
