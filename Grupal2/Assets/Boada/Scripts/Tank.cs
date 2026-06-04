using UnityEngine;

public class Tank : GroundUnit
{
    private void Update()
    {
        if (!IsServer)
            return;

        MoveToTarget();
    }
}