using System;
using System.Collections;
using PlayerScripts;
using TMPro;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
        public int baseMoney;
        public TextMeshProUGUI baseMoneyText;
        
        private void OnTriggerEnter(Collider other)
        {
                if (other.TryGetComponent(out Player player))
                {
                        player.inSafe = true;
                        TransferMoney(player);
                }
        }

        private void OnTriggerExit(Collider other)
        {
                if (other.TryGetComponent(out Player player))
                {
                        player.inSafe = false;
                }
        }

        private void TransferMoney(Player player)
        {
                baseMoney += player.money;
                player.money = 0;
                player.moneyText.text = "Money 0";
                baseMoneyText.text = "Base Money " + baseMoney;
        }
}