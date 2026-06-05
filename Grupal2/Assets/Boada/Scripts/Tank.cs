using UnityEngine;

public class Tank : GroundUnit
{
    protected override void Start()
    {
        shootCooldown = 1f;

        base.Start();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        Shoot();
        MoveToTarget();
    }
}