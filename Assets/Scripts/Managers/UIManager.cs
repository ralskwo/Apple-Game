using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text finalScoreText;

    void Awake()
    {
        ServiceLocator.Register<UIManager>(this);

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
            finalScoreText.text = "Score" + scoreText.text.Split(':')[1];
    }

    public void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(time);
            timerText.text = "Time: " + seconds;
        }
    }
}
