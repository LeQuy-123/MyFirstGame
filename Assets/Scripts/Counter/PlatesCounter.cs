using System;
using System.Collections;
using System.Collections.Generic;
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
        spawnPlateTimer+= Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) 
        {
            spawnPlateTimer = 0;
            if(plateSpawnAmount < plateSpawnAmountMax) 
            {
                plateSpawnAmount++;
                OnPlateSpawn.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (plateSpawnAmount > 0)
        {
            if (!player.HasKitchenObject())
            {
                plateSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchangeObjectSO,player);
                OnPlatePickUp.Invoke(this, EventArgs.Empty);
            }
            else
            {
            }
        }
    }
}
