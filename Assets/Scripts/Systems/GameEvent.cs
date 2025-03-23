using System;

public static class GameEvents
{
    // 점수 업데이트 (새 점수를 전달)
    public static Action<int> OnScoreUpdated;
    // 게임 종료 이벤트
    public static Action OnGameEnd;
    // (필요한 다른 이벤트들도 추가 가능)
}
