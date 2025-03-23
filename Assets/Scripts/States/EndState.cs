using UnityEngine;

public class EndState : IGameState
{
    private GameManager gameManager;

    public EndState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Entering End State");
        // 게임 종료 시 UI 표시 (결과 화면 등)
        gameManager.UIManager.ShowResultScreen(gameManager.Score);
    }

    public void Update()
    {
        // 종료 상태에서는 특별한 업데이트가 필요 없을 수 있음
    }

    public void Exit()
    {
        // 종료 상태를 벗어날 때 처리할 내용 (필요하면 작성)
    }
}
