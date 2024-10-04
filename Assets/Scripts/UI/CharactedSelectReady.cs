using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharactedSelectReady : NetworkBehaviour
{
    public static CharactedSelectReady Instace {get; private set;}
    private Dictionary<ulong, bool> playerReadyDictionary;
    private void Awake()
    {
        Instace = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    public void SetPlayerReady()
    {
        Debug.Log("Setting player ready...");
        SetPlayerReadyServerRpc();
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
            Loader.LoadNetworkScene(Loader.Sence.GameScene);
        }
    }
}
