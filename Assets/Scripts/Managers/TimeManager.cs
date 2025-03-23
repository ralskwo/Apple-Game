using UnityEngine;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float totalTime = 120f;
    private UIManager _uiManager;
    private GameManager _gameManager;

    private UIManager uiManager
    {
        get
        {
            if (_uiManager == null)
                _uiManager = ServiceLocator.Get<UIManager>();
            return _uiManager;
        }
    }

    private GameManager gameManager
    {
        get
        {
            if (_gameManager == null)
                _gameManager = ServiceLocator.Get<GameManager>();
            return _gameManager;
        }
    }

    private float remainingTime;
    private bool timerActive = false;

    void Awake()
    {
        ServiceLocator.Register<TimerManager>(this);
    }

    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        remainingTime = totalTime;
        timerActive = true;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (timerActive && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            uiManager.UpdateTimerUI(remainingTime);
            yield return null;
        }

        if (remainingTime <= 0)
        {
            timerActive = false;
            GameEvents.OnGameEnd?.Invoke();
        }
    }

    public void StopTimer()
    {
        timerActive = false;
    }
}
