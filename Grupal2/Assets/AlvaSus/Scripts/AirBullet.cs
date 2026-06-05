using Unity.Netcode;
using UnityEngine;

public class AirBullet : NetworkBehaviour
{
    public NetworkObject owner;
    public int teamId;
    Helicopter helicopter;
    PlayerController player;
    [SerializeField] int damage;
    public NetworkVariable<Teams> teamStuff = new NetworkVariable<Teams>();
    Teams teams;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Destroy(gameObject, 3f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
        {
            return;
        }
        NetworkObject networkObject = other.GetComponent<NetworkObject>();
        if (networkObject == owner)
        {
            return;
        }
        if (other.TryGetComponent<PlayerController>(out player))
        {
            if(player.teamSide.Value==teamStuff.Value)
            {
                return;
            }
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            if (helicopter.teamStuff.Value == teamStuff.Value)
            {
                return;
            }
            helicopter = other.GetComponent<Helicopter>();
            helicopter.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
