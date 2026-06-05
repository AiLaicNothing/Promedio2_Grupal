using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class IncreasePlayerSpeed : NetworkBehaviour
{
    [SerializeField] private int increaseSpeed;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(!IsServer) return;

        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.IncreaseSpeed(increaseSpeed);
            DisappearClientRpc();
            StartCoroutine(ReappearCorutine());
        }
    }

    private IEnumerator ReappearCorutine()
    {
        yield return new WaitForSeconds(10);
        ReappearClientRpc();
    }

    [ClientRpc]
    private void DisappearClientRpc()
    {
        meshRenderer.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    [ClientRpc]
    private void ReappearClientRpc()
    {
        meshRenderer.enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
