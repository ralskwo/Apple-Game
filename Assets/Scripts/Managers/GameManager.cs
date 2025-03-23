using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private BoardManager boardManager;
    private UIManager uiManager;
    private TimerManager timerManager;
    private IGameState currentState;

    public int Score { get; private set; }
    public UIManager UIManager => uiManager;

    void Awake()
    {
        ServiceLocator.Register<GameManager>(this);
    }

    void Start()
    {
        boardManager = ServiceLocator.Get<BoardManager>();
        uiManager = ServiceLocator.Get<UIManager>();
        timerManager = ServiceLocator.Get<TimerManager>();

        ChangeState(new PlayingState(this));
        StartGame();
    }

    public void StartGame()
    {
        Score = 0;
        boardManager.GenerateBoard();
        uiManager.UpdateScoreUI(Score);
        timerManager.StartTimer();

        GameEvents.OnScoreUpdated?.Invoke(Score);
    }

    public void EndGame()
    {
        ChangeState(new EndState(this));
    }

    public void UpdateScore(int addScore)
    {
        Score += addScore;
        uiManager.UpdateScoreUI(Score);
        GameEvents.OnScoreUpdated?.Invoke(Score);
    }

    // 추가된 메소드: 현재 상태가 PlayingState라면 게임이 활성 상태로 판단
    public bool IsGameActive()
    {
        return currentState is PlayingState;
    }

    void Update()
    {
        if (currentState != null)
            currentState.Update();
    }

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
            currentState.Enter();
    }

    // 게임 재시작: 현재 씬을 다시 로드합니다.
    public void RestartGame()
    {
        // 필요한 초기화 작업 (ServiceLocator.Clear() 등) 후 재시작할 수도 있습니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 게임 종료: 애플리케이션 종료를 호출합니다.
    public void ExitGame()
    {
        // 에디터 환경에서는 StopPlaying
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
