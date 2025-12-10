using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float waitingToStartTime = 1f;
    private float countdownToStart = 3f;
    private float gamePlayTimer;
    private float gamePlayTimerMax = 10f;

    

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTime -= Time.deltaTime;
                if (waitingToStartTime <= 0)
                {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStart -= Time.deltaTime;
                if (countdownToStart <= 0)
                {
                    state = State.GamePlaying;
                    gamePlayTimer = gamePlayTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayTimer -= Time.deltaTime;
                if (gamePlayTimer <= 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStart;
    }
    
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    
    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayTimer / gamePlayTimerMax);
    }
}
