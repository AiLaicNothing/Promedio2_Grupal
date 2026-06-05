using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float damage;
    private Teams teamSide;

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
            }
        }
    }
}
