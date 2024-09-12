using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchangeObjectSO;
    private float spawnPlateTimer;
    [SerializeField] private float spawnPlateTimerMax = 4f;

    private void Update()
    {
        spawnPlateTimer+= Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) 
        {
            if(!HasKitchenObject()) {
                KitchenObject.SpawnKitchenObject(plateKitchangeObjectSO, this);
            }
        }
    }


}
