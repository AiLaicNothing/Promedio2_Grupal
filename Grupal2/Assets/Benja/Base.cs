using Unity.Netcode;
using UnityEngine;

public class Base : NetworkBehaviour
{
    [SerializeField] private Teams team;
    public Teams Team => team;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entro a la base");

        if (!IsServer)
            return;

        GroundUnit unit = other.GetComponent<GroundUnit>();

        if (unit == null)
            return;

        Debug.Log("Entro un tanque");

        if (unit.TeamNetwork.Value == team)
            return;

        Debug.Log("Fin del juego");

        GameManager.Instance.EndGame(unit.TeamNetwork.Value);
    }



}
