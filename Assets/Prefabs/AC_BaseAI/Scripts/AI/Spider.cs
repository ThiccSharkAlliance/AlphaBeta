using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Units
{
    private void FixedUpdate()
    {
        if (Fuzzy_AI.Active_Status == Fuzzy_Coordination.Status.Defensive)
        {
            if (Base.Enemies.Count == 0)
            {
                Action_Type = "Idle";
            }
            if (Base.Enemies.Count != 0)
            {
                Action_Type = "Protect";
            }
        }
        if (Fuzzy_AI.Active_Status == Fuzzy_Coordination.Status.Offensive)
        {
            Action_Type = "Attack";
        }
    }
}
