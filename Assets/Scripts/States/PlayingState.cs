using UnityEngine;

public class PlayingState : IGameState
{
    private GameManager gameManager;

    public PlayingState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Entering Playing State");
        // 추가 초기화가 필요하면 여기에 작성 (예: UI 활성화, 입력 활성화 등)
    }

    public void Update()
    {
        // 게임 플레이 상태의 로직 (필요에 따라 Input, 타이머 등 추가)
    }

    public void Exit()
    {
        Debug.Log("Exiting Playing State");
        // 상태 종료 시 클린업 작업
    }
}
