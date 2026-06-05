using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public abstract class GroundUnit : BaseUnits
{
    [Header("Team")]
    public NetworkVariable<Teams> TeamNetwork =
    new NetworkVariable<Teams>();

    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 3.5f;

    private float nextShootTime;

    protected NavMeshAgent agent;

    [SerializeField] protected Transform targetTransform;

    [SerializeField] private Transform FirePoint;

    [SerializeField] private GameObject BulletPrefab;

    [SerializeField] private LayerMask Target;

    [Header("Attack")]
    [SerializeField] protected float shootCooldown = 1f;

    protected void Update()
    {

    }
    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
    }

    public virtual void SetTeam(Teams newTeam)
    {
        TeamNetwork.Value = newTeam;
    }

    public virtual void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    protected virtual void MoveToTarget()
    {
        if (targetTransform == null)
        {
            Debug.Log("Target NULL");
            return;
        }

        if (agent == null)
        {
            Debug.Log("Agent NULL");
            return;
        }

        agent.SetDestination(targetTransform.position);
    }

    public void Shoot()
    {
        if (Time.time < nextShootTime)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 100, Target);

        GroundUnit target = null;
        float closestdistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("GroundUnit"))
            {
                GroundUnit groundunit = collider.GetComponent<GroundUnit>();

                if (groundunit.TeamNetwork.Value == TeamNetwork.Value)
                {
                    continue;
                }

                float distance = (collider.transform.position - transform.position).sqrMagnitude;

                if (distance < closestdistance)
                {
                    closestdistance = distance;
                    target = groundunit;
                }
            }
        }

        if (!target)
        {
            return;
        }

        nextShootTime = Time.time + shootCooldown;

        GameObject bullet = Instantiate(
            BulletPrefab,
            FirePoint.position,
            Quaternion.identity);

        Vector3 direction =
            (target.transform.position - FirePoint.position).normalized;

        bullet.transform.forward = direction;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * 20;
        }
    }
}