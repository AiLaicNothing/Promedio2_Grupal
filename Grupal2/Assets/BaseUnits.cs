using Unity.Netcode;
using UnityEngine;

public abstract class BaseUnits : MonoBehaviour, IDamageable
{
    public float lifeMax = 100f;
    public NetworkVariable<float> lifeAct = new();
    public float damage = 10f;

    protected virtual void Start()
    {
        lifeAct.Value = lifeMax;
    }

    public virtual void TakeDamage(float damage)
    {
        lifeAct.Value -= damage;

        if (lifeAct.Value <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {

        GetComponent<NetworkObject>().Despawn();
    }
}