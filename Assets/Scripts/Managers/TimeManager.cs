using UnityEngine;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float totalTime = 120f;
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
            ServiceLocator.Get<UIManager>().UpdateTimerUI(remainingTime);
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

    public bool IsActive()
    {
        return timerActive;
    }
}
