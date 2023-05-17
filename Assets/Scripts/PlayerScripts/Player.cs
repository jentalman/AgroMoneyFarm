using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, IDamageable
    {
        public int money;
        public float health;
        public Action onPlayerDead;

        public TextMeshProUGUI moneyText;
        public bool inSafe = true;
        
        
        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("Enemy take damage");
            if (health <= 0)
            {
                onPlayerDead?.Invoke();
                Destroy(gameObject);
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void PickUpReward(Reward reward)
        {
            money += reward.RewardValue;
            moneyText.text = "Money " + money;
        }
    }
}