using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnPauseAction;
    public event EventHandler OnUnPauseAction;

    private enum State {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    private State state;
    private float countDownToStartTimer = 3f;
    private float gamePlayerTimer;
    private float gamePlayerTimerMax =60f;
    private bool isGamePause = false;
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if(isGamePause) 
        {
            UnPauseGame();
        } 
        else 
        {
            PauseGame();
        }
    }

   
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayerTimer = gamePlayerTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayerTimer -= Time.deltaTime;
                if (gamePlayerTimer < 0f)
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
    public bool IsCountDownToStartActive()
    {
        return state == State.CountDownToStart;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer;
    }

    public float GetCountDownPlayTimer()
    {
        return 1 - gamePlayerTimer / gamePlayerTimerMax;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePause = true;
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        isGamePause = false;
        OnUnPauseAction?.Invoke(this, EventArgs.Empty);
    }
}
