using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BaseSpawner : NetworkBehaviour
{
    [Header("Team")]
    [SerializeField] private Teams team;

    [Header("SpawnPoint")]
    [SerializeField] private Transform spawnpoin;
    [Header("Units")]
    [SerializeField] private GameObject tankprefab;
    [SerializeField] private GameObject supertankprefab;
    [SerializeField] private GameObject helicopterprefab;
    [SerializeField] private GameObject superchopperprefab;



    

    public Teams Team => team;

    public void Spawnpoint(int type)
    {
        if(!IsServer) return;
        GameObject prefab = null;

        switch (type)
        {
            case 1:
                prefab= tankprefab;
                break;
            case 2:
                prefab = supertankprefab;
                break;
            case 3:
                prefab = helicopterprefab;
                break;
            case 4:
                prefab = superchopperprefab;
                break;

        }

        if (prefab == null) return;
        {
            GameObject obj = Instantiate(prefab, spawnpoin.position, spawnpoin.rotation);

            obj.GetComponent<NetworkObject>().Spawn();

            GroundUnit ground =
                obj.GetComponent<GroundUnit>();

            if (ground != null)
            {
                ground.SetTeam(team);

                Base[] bases =
                    FindObjectsByType<Base>(FindObjectsSortMode.None);

                foreach (Base enemyBase in bases)
                {
                    if (enemyBase.Team != team)
                    {
                        ground.SetTarget(enemyBase.transform);
                        break;
                    }
                }
            }

            Helicopter heli=
                obj.GetComponent<Helicopter>();
            if(heli != null)
            {
                heli.SetTeam(team);
            }
        }
    }

}
