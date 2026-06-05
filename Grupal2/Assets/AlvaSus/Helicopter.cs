using UnityEngine;
using UnityEngine.AI;

public class Helicopter : EnemyBase
{
    GameObject[] PlayerTeam;
    GameObject target;
    NavMeshAgent agent;
    public void ChooseTarget()
    {
        PlayerTeam = GameObject.FindGameObjectsWithTag("Player");
        GameObject choose = PlayerTeam[0];
        int position = 0;

        foreach (GameObject player in PlayerTeam)
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
    }
    private void Move()
    {
        agent.destination = target.transform.position;
    }
}
