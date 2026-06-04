using UnityEngine;

public class SuperTank : GroundUnit
{
    protected override void Start()
    {
        lifeMax = 300f;

        moveSpeed = 2f; 

        base.Start();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        MoveToTarget();
    }
}