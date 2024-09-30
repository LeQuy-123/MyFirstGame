using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlatePickUp;

    [SerializeField] private KitchenObjectSO plateKitchangeObjectSO;
    private float spawnPlateTimer;
    [SerializeField] private float spawnPlateTimerMax = 4f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;

    private void Update()
    {
        if (IsServer) {
            spawnPlateTimer += Time.deltaTime;
            if (KitchenGameManager.Instance.IsGamePlaying() && spawnPlateTimer > spawnPlateTimerMax)
            {
                spawnPlateTimer = 0;
                if (plateSpawnAmount < plateSpawnAmountMax)
                {
                    SpawnPlateServerRpc();
                }
            }
        }
        
    }
    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }
    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        plateSpawnAmount++;
        OnPlateSpawn.Invoke(this, EventArgs.Empty);
    }
    public override void Interact(Player player)
    {
        if (plateSpawnAmount > 0)
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(plateKitchangeObjectSO,player);
                PickupPlateServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PickupPlateServerRpc()
    {
        PickupPlateClientRpc();
    }
    [ClientRpc]
    private void PickupPlateClientRpc()
    {
        plateSpawnAmount--;
        OnPlatePickUp.Invoke(this, EventArgs.Empty);
    }
}
