using Unity.VisualScripting;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);

    public Transform GetTransform();
}