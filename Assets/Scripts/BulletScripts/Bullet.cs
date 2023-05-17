using System;
using UnityEngine;

namespace BulletScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private const string DisableMethodName = "AutoDisable";
        
        public float AutoDestroyTime = 5f;
        public float MoveSpeed = 2f;
        public int Damage = 5;
        public Rigidbody bulletRigidbody;
        public Action<Bullet> Disable;

        private void Awake()
        {
            bulletRigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            CancelInvoke(DisableMethodName);
            Invoke(DisableMethodName, AutoDestroyTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable;

            if (other.TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(Damage);
            }

            Disable?.Invoke(this);
        }

        private void AutoDisable()
        {
            Disable?.Invoke(this);
        }
    }
}