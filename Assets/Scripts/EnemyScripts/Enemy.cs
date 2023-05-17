using System;
using System.Collections;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public float _speed;
        public float _health;
        public Action<Enemy> Disable;
        public Player _target;
        public float _dropChance;
        
        
        private float _reward;
        private float _attackCooldown;
        private EnemySettings _enemySettings;
        private Coroutine _lookCoroutine;


        public void Init(EnemySettings enemySettings)
        {
            _enemySettings = enemySettings;
            _speed = enemySettings.Speed;
            _health = enemySettings.Health;
            _reward = enemySettings.Reward;
            _attackCooldown = enemySettings.AttackCooldown;
            _target = GameObject.FindWithTag("Player").GetComponent<Player>();


        }

        /*public void StartRotating()
        {
            if (_lookCoroutine != null)
            {
                StopCoroutine(_lookCoroutine);
            }

            _lookCoroutine = StartCoroutine(LookAt(_target.transform));
        }

        private IEnumerator LookAt(Transform target)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
            float time = 0;

            while (time < 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

                time += Time.deltaTime * 2;
                yield return null;
            }
            
        }*/

        private void Update()
        {
            if (_target.inSafe) return;
            transform.position = Vector3.MoveTowards(this.transform.position, _target.transform.position,
                _speed * Time.deltaTime);
            
            transform.LookAt(_target.transform);

        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            Debug.Log("Enemy take damage");
            if (_health <= 0)
            {
                Disable?.Invoke(this);
            }
        }

        public void ResetHealth()
        {
            _health = _enemySettings.Health;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void DropLoot()
        {

            var randomNumber = Random.Range(0, 100);

            if (randomNumber <= _dropChance)
            {
                var prefab = Resources.Load<GameObject>("Reward");
                Instantiate(prefab, transform.position, Quaternion.identity);
            }

        }
        
    }
}