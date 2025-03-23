using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private BoardManager boardManager;
    private TimerManager timerManager;
    public int Score { get; private set; }

    void Awake()
    {
        ServiceLocator.Register<GameManager>(this);
    }

    void Start()
    {
        boardManager = ServiceLocator.Get<BoardManager>();
        timerManager = ServiceLocator.Get<TimerManager>();

        StartGame();
    }

    public void StartGame()
    {
        Score = 0;
        boardManager.GenerateBoard();
        timerManager.StartTimer();

        // 점수 업데이트 이벤트 발생 (UIManager는 이를 구독)
        GameEvents.OnScoreUpdated?.Invoke(Score);
    }

    public void EndGame()
    {
        // 게임 종료 시 직접 UI 업데이트 및 이벤트 발생
        GameEvents.OnGameEnd?.Invoke();
    }

    public void UpdateScore(int addScore)
    {
        Score += addScore;
        GameEvents.OnScoreUpdated?.Invoke(Score);
    }

    // 재시작과 종료 함수는 그대로 유지
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 게임 활성 여부는 타이머나 상태 변화에 따라 이벤트로 처리 가능 (간단하게는 Score가 0 이상이면 플레이 중이라고 볼 수도 있음)
    public bool IsGameActive()
    {
        // 예: TimerManager가 동작 중이면 게임이 활성 상태
        return timerManager != null && timerManager.IsActive();
    }
}
