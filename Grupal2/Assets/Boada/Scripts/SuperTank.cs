using UnityEngine;

public class SuperTank : GroundUnit
{
    protected override void Start()
    {
        lifeMax = 300f;
        moveSpeed = 2f;

        shootCooldown = 2f;

        base.Start();
    }

    private void Update()
    {
        Shoot();
        MoveToTarget();
    }
}