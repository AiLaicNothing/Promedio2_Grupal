using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float damage = 10f;

    [Header("Combat")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float targetSearchRadius = 8f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float transformSpeed = 8f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Models")]
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject transformModel;

    private readonly NetworkVariable<float> currentHp = new NetworkVariable<float>();

    public readonly NetworkVariable<bool> transformedState = new NetworkVariable<bool>();

    public readonly NetworkVariable<Teams> teamSide = new NetworkVariable<Teams>();

    private Rigidbody rb;
    private bool isTransformed;

    public override void OnNetworkSpawn()
    {
        Debug.Log("Player Spawned");

        rb = GetComponent<Rigidbody>();

        if (IsServer)
        {
            currentHp.Value = maxHp;
            teamSide.Value = PlayerTeamManager.Instance.GetNextTeam();
        }

        transformedState.OnValueChanged += OnTransformedChanged;
        ApplyTransformState(transformedState.Value);
    }

    public override void OnNetworkDespawn()
    {
        transformedState.OnValueChanged -= OnTransformedChanged;
    }

    private void Update()
    {
        if (!IsOwner) return;

        HandleTransformInput();
        HandleRotation();
        ShootServerRpc();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            BuyUnitServerRpc(1, 100);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            BuyUnitServerRpc(2, 200);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            BuyUnitServerRpc(3, 150);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            BuyUnitServerRpc(4, 300);
    }





    [ServerRpc]
    private void BuyUnitServerRpc(int type, int cost)
    {
        Teams myTeam = teamSide.Value;

        int teamID = myTeam == Teams.Red ? 1 : 2;

        if (!EconomyManager.Instance.SpendMoney(teamID, cost))
            return;

        BaseSpawner[] spawners =
            FindObjectsByType<BaseSpawner>(FindObjectsSortMode.None);

        foreach (BaseSpawner spawner in spawners)
        {
            if (spawner.Team == myTeam)
            {
                spawner.Spawnpoint(type);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Movement();
    }

    private void HandleTransformInput()
    {
        bool wantsTransform = Input.GetKey(KeyCode.LeftShift);

        if (wantsTransform == isTransformed) return;

        ApplyTransformState(wantsTransform);
        SetTransformStateServerRpc(wantsTransform);
    }

    [ServerRpc]
    private void SetTransformStateServerRpc(bool state)
    {
        transformedState.Value = state;
    }

    private void OnTransformedChanged(bool previousValue, bool newValue)
    {
        ApplyTransformState(newValue);
    }

    private void ApplyTransformState(bool state)
    {
        isTransformed = state;
        normalModel.SetActive(!state);
        transformModel.SetActive(state);
    }

    private void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Debug.Log($"X:{inputX} Y:{inputY}");

        Vector3 moveDir = new Vector3(inputX, 0f, inputY).normalized;

        float desiredSpeed = isTransformed ? transformSpeed : moveSpeed;

        Vector3 velocity = moveDir * desiredSpeed;

        Debug.Log("Velocity: " + velocity);

        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void HandleRotation()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        Vector3 lookDirection = moveInput;

        if (!isTransformed)
        {
            Transform target = FindClosestTarget();

            if (target != null)
            {
                lookDirection = target.position - transform.position;
                lookDirection.y = 0f;
            }
        }

        if (lookDirection.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(lookDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private Transform FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, targetSearchRadius, targetLayer);

        Transform closest = null;
        float closestSqrDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float sqrDist = (hit.transform.position - transform.position).sqrMagnitude;

            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closest = hit.transform;
            }
        }

        return closest;
    }

    [ServerRpc]
    private void ShootServerRpc()
    {

        if (isTransformed) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Transform target = FindClosestTarget();


        if (isTransformed == true)
        {
            Debug.Log("Cant shoot is transformed");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.GetComponent<NetworkObject>().Spawn();

        Vector3 dir;

        if (target != null)
        {
            dir = (target.position - firePoint.position);
        }
        else
        {
            dir = firePoint.transform.forward;
        }

        bullet.transform.forward = dir;

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<PlayerBullet>().SetTeam(teamSide.Value);
        bullet.GetComponent<PlayerBullet>().SetDamage(damage);

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = dir * 22f;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer) return;

        currentHp.Value -= damage;

        if (currentHp.Value <= 0)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }

    public void Heal(float ammount)
    {
        if (!IsServer) return;

        currentHp.Value = Mathf.Clamp(currentHp.Value + ammount, 0, maxHp);
    }

    public void IncreaseDamage(float ammount)
    {
        damage += ammount;
    }

    public void IncreaseSpeed(float ammount)
    {
        moveSpeed += ammount;
        transformSpeed += ammount;
    }
}
