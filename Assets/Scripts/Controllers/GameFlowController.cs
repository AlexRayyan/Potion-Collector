using UnityEngine;
using System;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    private string sessionId;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    private void Start()
    {
        sessionId = Guid.NewGuid().ToString();
        float timestamp = Time.time;

        GameEvents.GameStartedEvent(timestamp, sessionId);
    }

    [ContextMenu("QuitGame")]
    public void QuitGame()
    {
        float timestamp = Time.time;
        GameEvents.GameEndedEvent(timestamp, ScoreManager.Instance.totalScore);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PauseGame()
    {
        GameEvents.GamePausedEvent(Time.time);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        GameEvents.GameResumedEvent(Time.time);
    }
}
