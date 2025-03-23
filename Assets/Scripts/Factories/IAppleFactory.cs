// IAppleFactory.cs
using UnityEngine;

public interface IAppleFactory
{
    // 주어진 위치와 보드 위치 정보를 기반으로 Apple 객체를 생성하여 반환합니다.
    Apple CreateApple(Vector2 anchoredPosition, Vector2Int boardPosition);
}
