using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footStepTimer;
    private float footStepTimerMax = .2f;
    
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        footStepTimer -= Time.deltaTime;
        if ( footStepTimer <= 0)
        {
            footStepTimer = footStepTimerMax;
            if(player.IsWalking())  SoundManager.Instance.PlayWalkingsound(player.transform.position, 1f);
        }
    }
}
