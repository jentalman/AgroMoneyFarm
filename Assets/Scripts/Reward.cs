using System;
using PlayerScripts;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public int RewardValue;
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            player.PickUpReward(this);
            Destroy(gameObject);
        }
    }
}