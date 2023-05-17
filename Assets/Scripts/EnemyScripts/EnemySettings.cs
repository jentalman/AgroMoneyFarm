using UnityEngine;

namespace EnemyScripts
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings", order = 1)]
    public class EnemySettings : ScriptableObject
    {
        public float Speed;
        public float Health;
        public float Reward;
        public float AttackCooldown;
        public int MaxEnemies;
    }
}