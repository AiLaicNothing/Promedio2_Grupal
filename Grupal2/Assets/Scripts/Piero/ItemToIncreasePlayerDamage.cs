using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class ItemToIncreasePlayerDamage : NetworkBehaviour
{
    [SerializeField] private float damageIncrease;
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
            BaseUnits baseUnits = other.GetComponent<BaseUnits>();
            baseUnits.damage += damageIncrease;
            HideElementClientRpc();

            StartCoroutine(ReturnCoroutine());
        }
    }

    private IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSeconds(10);
        ShowItemClientRpc();
    }

    [ClientRpc]
    private void HideElementClientRpc()
    {
        meshRenderer.enabled = false;
    }

    [ClientRpc]
    private void ShowItemClientRpc()
    {
        meshRenderer.enabled = true;
    }
}