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

    protected NavMeshAgent agent;

    protected Transform targetTransform;

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
            return;

        agent.SetDestination(targetTransform.position);
    }
}