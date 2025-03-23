using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;         // 점수 표시용
    [SerializeField] private TMP_Text timerText;          // 타이머 표시용
    [SerializeField] private TMP_Text remainingCountText; // 남은 조합(또는 개수) 표시용
    [SerializeField] private GameObject resultPanel;      // 결과 패널
    [SerializeField] private TMP_Text finalScoreText;     // 최종 점수 표시용

    void Awake()
    {
        ServiceLocator.Register<UIManager>(this);

        // 이벤트 구독
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
        GameEvents.OnGameEnd += HandleGameEnd;
    }

    void OnDestroy()
    {
        GameEvents.OnScoreUpdated -= HandleScoreUpdated;
        GameEvents.OnGameEnd -= HandleGameEnd;
    }

    private void HandleScoreUpdated(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + newScore;
    }

    private void HandleGameEnd()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);
        if (finalScoreText != null && scoreText != null)
            finalScoreText.text = "Score " + scoreText.text;
    }

    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(time);
            timerText.text = "Time: " + seconds.ToString();
        }
    }

    public void UpdateRemainingCountUI(int count)
    {
        if (remainingCountText != null)
            remainingCountText.text = "Remain: " + count.ToString();
    }

    public void ShowResultScreen(int finalScore)
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + finalScore.ToString();
        }
    }
}
