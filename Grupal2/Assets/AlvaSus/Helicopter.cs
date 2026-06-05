using System;
using UnityEngine;
using UnityEngine.AI;

public class Helicopter : EnemyBase
{
    GameObject[] PlayerTeam;
    GameObject target;
    NavMeshAgent agent;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject shootPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int speed=5;
    public void ChooseTarget()
    {
        PlayerTeam = GameObject.FindGameObjectsWithTag("Player");
        GameObject choose = PlayerTeam[0];
        int position = 0;

        foreach (GameObject go in PlayerTeam)
        {
            float targetDistance = Vector3.Distance(transform.position, choose.transform.position);
            float thisDistance = Vector3.Distance(transform.position, PlayerTeam[position].transform.position);
            if (targetDistance > thisDistance)
            {
                choose = PlayerTeam[position];
            }
            position++;
        }
        target = choose;
        transform.LookAt(target.transform);
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        ChooseTarget();
        Move();
        Shooting();
    }
    private void Move()
    {
        agent.destination = target.transform.position;
    }
    private void Shooting()
    {
        PlayerTeam = GameObject.FindGameObjectsWithTag("Player");
        GameObject target = PlayerTeam[0];
        int position = 0;

        foreach (GameObject go in PlayerTeam)
        {
            float targetDistance = Vector3.Distance(transform.position, target.transform.position);
            float thisDistance = Vector3.Distance(transform.position, PlayerTeam[position].transform.position);
            if (targetDistance > thisDistance)
            {
                target = PlayerTeam[position];
            }
            position++;
        }
        transform.LookAt(target.transform);
        Shoot();
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.forward * speed;
    }
}
