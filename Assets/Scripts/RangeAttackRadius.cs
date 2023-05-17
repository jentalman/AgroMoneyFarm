using System;
using System.Collections;
using BulletScripts;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class RangeAttackRadius : AttackRadius
{
        public Bullet BulletPrefab;
        public Transform BulletSpawnPosition;
        public LayerMask Mask;
        

        [SerializeField] private ObjectPool<Bullet> BulletPool;
        private float SpherecastRadius = 0.1f;
        private RaycastHit Hit;
        private IDamageable targetDamageable;

        protected override void Awake()
        {
                base.Awake();
                
                BulletPool = new ObjectPool<Bullet>(CreateBullet, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false,
                        10, 100);
        }

        protected override IEnumerator Attack()
        {
                WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

                yield return Wait;

                while (_damageables.Count > 0)
                {
                        for (int i = 0; i < _damageables.Count; i++)
                        {
                                if (HasLineOfSightTo(_damageables[i].GetTransform()))
                                {
                                        targetDamageable = _damageables[i];
                                        OnAttack?.Invoke(_damageables[i]);
                                        break;
                                }
                        }

                        if (targetDamageable != null)
                        {
                                BulletPool.Get();
                        }

                        yield return Wait;

                        _damageables.RemoveAll(DisabledDamageables);
                }

                AttackCoroutin = null;
        }

        private bool HasLineOfSightTo(Transform getTransform)
        {
                return true;
        }

        private void OnDestroyObject(Bullet obj)
        {
                Destroy(obj);
        }

        private void OnReturnToPool(Bullet obj)
        {
                obj.gameObject.SetActive(false);
                obj.bulletRigidbody.velocity = Vector3.zero;
        }

        private void OnTakeFromPool(Bullet obj)
        {
                obj.gameObject.SetActive(true);
                obj.Damage = Damage;
                obj.transform.position = BulletSpawnPosition.position;
                obj.bulletRigidbody.AddForce((targetDamageable.GetTransform().position - transform.position).normalized * obj.MoveSpeed, ForceMode.VelocityChange);
        }

        private Bullet CreateBullet()
        {
                var bulletPrefab = Instantiate("Bullet");

                if (bulletPrefab.TryGetComponent<Bullet>(out var bullet))
                {
                        bullet.Disable += ReturnBulletPool;
                        return bullet;
                }
                else
                {
                        throw new Exception("Bullet lost script");
                }
        }

        private void ReturnBulletPool(Bullet obj)
        {
                BulletPool.Release(obj);
        }

        private static GameObject Instantiate(string path)
        {
                var prefab = Resources.Load<GameObject>(path);
                return Object.Instantiate(prefab);
        }
}