using UnityEngine;
using Unity.Netcode;

public class PlayerBullet : NetworkBehaviour
{
    private float damage;
    private Teams teamSide;
    private Vector3 direction;
    private Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        rb.linearVelocity = direction * 22f;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetTeam(Teams team)
    {
        teamSide = team;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player.teamSide.Value == teamSide)
            {
                return;
            }
            else
            {
                player.TakeDamage(damage);
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
        else if (other.gameObject.CompareTag("GroundUnit"))
        {
            GroundUnit unit = other.GetComponent<GroundUnit>();

            if (unit.TeamNetwork.Value == teamSide)
            {
                return;
            }
            else
            {
                unit.TakeDamage(damage);
            }
        }
        else if (other.gameObject.CompareTag("asd"))
        {
            Helicopter unit = other.GetComponent<Helicopter>();

            if (unit.teamStuff.Value == teamSide)
            {
                return;
            }
            else
            {
                unit.TakeDamage(damage);
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
