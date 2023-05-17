using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class AttackRadius : MonoBehaviour
{
    protected List<IDamageable> _damageables = new List<IDamageable>();
    private CapsuleCollider _collider;
    public int Damage = 1;
    public float AttackDelay = 0.5f;
    public Action<IDamageable> OnAttack;
    protected Coroutine AttackCoroutin;

    protected virtual void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            _damageables.Add(damageable);

            if (AttackCoroutin == null)
            {
                AttackCoroutin = StartCoroutine(Attack());
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            _damageables.Remove(damageable);

            if (_damageables.Count == 0)
            {
                StopCoroutine(AttackCoroutin);
                AttackCoroutin = null;
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        yield return Wait;

        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        while (_damageables.Count > 0)
        {
            for (int i = 0; i < _damageables.Count; i++)
            {
                Transform damageableTransform = _damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = _damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                OnAttack?.Invoke(closestDamageable);
                closestDamageable.TakeDamage(Damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return Wait;

            _damageables.RemoveAll(DisabledDamageables);
        }

        AttackCoroutin = null;
    }

    protected bool DisabledDamageables(IDamageable damageable)
    {
        return damageable != null && !damageable.GetTransform().gameObject.activeSelf;
    }
}