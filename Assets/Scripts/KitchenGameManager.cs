using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    private Dictionary<ulong, bool> playerReadyDictionary;
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnPauseAction;
    public event EventHandler OnUnPauseAction;
    public event EventHandler OnLocalPlayerReadyChange;

    private enum State {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayerTimer = new NetworkVariable<float>(0f);
    private float gamePlayerTimerMax =  10f;
    private bool isGamePause = false;
    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChange?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
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
        if(!IsServer) return;
        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if (countDownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayerTimer.Value = gamePlayerTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayerTimer.Value -= Time.deltaTime;
                if (gamePlayerTimer.Value < 0f)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountDownToStartActive()
    {
        return state.Value == State.CountDownToStart;
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }
    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer.Value;
    }

    public float GetCountDownPlayTimer()
    {
        return 1 - gamePlayerTimer.Value / gamePlayerTimerMax;
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
    public void ReloadGame()
    {
        Loader.LoadScene(Loader.Sence.GameScene);
        Time.timeScale = 1f;
    }
    public bool GetIsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        playerReadyDictionary[rpcParams.Receive.SenderClientId] = true;
        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }
        if (allClientsReady)
        {
            state.Value = State.CountDownToStart;
        }
    }
}
