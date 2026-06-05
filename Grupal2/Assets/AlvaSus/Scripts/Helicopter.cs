using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Helicopter : BaseUnits
{
    GameObject[] playerTeam;
    Teams teams;
    NavMeshAgent agent;
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] GameObject ShootPoint;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] float coolDown;
    private float next = 4;
    [SerializeField] int speed=5;
    public NetworkVariable<Teams> teamStuff =new NetworkVariable<Teams>();
    [SerializeField] private LayerMask theTarget;
    public virtual void SetTeam(Teams newTeam)
    {
        teamStuff.Value = newTeam;
    }

    public virtual void SetTarget(Transform[] target)
    {
        PatrolPoints = target;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (Time.time < coolDown)
        {
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100, theTarget);
        Helicopter ChopperTarget = null;
        PlayerController playerTarget = null;
        float closestdistance = float.MaxValue;

        foreach(Collider targets in colliders)
        {
            Helicopter helicopter = targets.GetComponent<Helicopter>();
            PlayerController player=targets.GetComponent<PlayerController>();
            if(helicopter==null&&player==null)
            {
                continue;
            }
            Vector3 TargetPosition;
            Teams TargetTeam;
            if(helicopter!=null)
            {
                TargetPosition = helicopter.transform.position;
                TargetTeam = helicopter.teamStuff.Value;
            }
            else
            {
                TargetPosition = player.transform.position;
                TargetTeam = player.teamSide.Value;
            }
            if(TargetTeam== teamStuff.Value)
            {
                continue;
            }
            float distance = (TargetPosition - transform.position).sqrMagnitude;
            if (distance < closestdistance)
            {
                closestdistance = distance;
                if(helicopter!=null)
                {
                    ChopperTarget=helicopter;
                }
                else
                {
                    playerTarget = player;
                }
            }
        }
        Transform target = null;

        if (ChopperTarget != null)
        {
            target = ChopperTarget.transform;
        }
        else if (playerTarget != null)
        {
            target = playerTarget.transform;
        }

        if (target == null)
        {
            return;
        }

        next = Time.time + coolDown;

        GameObject bullet = Instantiate(
            BulletPrefab,
            ShootPoint.transform.position,
            Quaternion.identity);
        Vector3 direction = (target.transform.position - ShootPoint.transform.position).normalized;
        bullet.transform.forward = direction;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        AirBullet airBullet=bullet.GetComponent<AirBullet>();
        airBullet.teamStuff.Value = this.teamStuff.Value;
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
}
