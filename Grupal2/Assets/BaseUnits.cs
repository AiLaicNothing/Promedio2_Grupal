using UnityEngine;

public abstract class BaseUnits : MonoBehaviour, IDamageable
{
    public int lifeMax = 100f;
    public int lifeAct;
    public int damage = 10f;

    protected virtual void Start()
    {
        lifeAct = lifeMax;
    }

    public virtual void TakeDamage(float damage)
    {
        lifeAct -= damage;

        if (lifeAct <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {

        Destroy(gameObject);
    }
}