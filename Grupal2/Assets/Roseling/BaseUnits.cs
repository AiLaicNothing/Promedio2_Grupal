using UnityEngine;

public abstract class BaseUnits : NetworkBehaviour, IDamageable
{
    public float lifeMax = 100f;
    public float lifeAct;
    public float damage = 10f;

    protected virtual void Start()
    {
        lifeAct = lifeMax;
    }

    public virtual void TakeDamage(float damage)
    {
        lifeAct -= damage;

        if (lifeAct <= 0f)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {

        Destroy(gameObject);
    }
}