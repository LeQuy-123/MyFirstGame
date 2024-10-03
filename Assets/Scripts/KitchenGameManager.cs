using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPauseDictionary;

    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPauseAction;
    public event EventHandler OnLocalUnPauseAction;
    public event EventHandler OnLocalPlayerReadyChange;
    public event EventHandler OnMultiplayerUnPauseAction;
    public event EventHandler OnMultiplayerPauseAction;

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
    private float gamePlayerTimerMax =  90f;
    private bool isLocalGamePause = false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPauseDictionary = new Dictionary<ulong, bool>();
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerPauseAction?.Invoke(this, EventArgs.Empty);
        } 
        else
        {
            Time.timeScale = 1f;
            OnMultiplayerUnPauseAction?.Invoke(this, EventArgs.Empty);
        }
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
        if(isLocalGamePause) 
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
        PauseGameServerRpc();
        OnLocalPauseAction.Invoke(this, EventArgs.Empty);
        isLocalGamePause = true;
    }
    public void UnPauseGame()
    {
        UnPauseGameServerRpc();
        OnLocalUnPauseAction.Invoke(this, EventArgs.Empty);
        isLocalGamePause = false;
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
    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams rpcParams = default)
    {
        playerPauseDictionary[rpcParams.Receive.SenderClientId] = true;
        CheckGamePauseState();
    }
    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams rpcParams = default)
    {
        playerPauseDictionary[rpcParams.Receive.SenderClientId] = false;
        CheckGamePauseState();
    }
    private void CheckGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPauseDictionary.ContainsKey(clientId) && playerPauseDictionary[clientId])
            {
                isGamePaused.Value = true;
                return;
            }
        }
        isGamePaused.Value = false;
    }   
}
