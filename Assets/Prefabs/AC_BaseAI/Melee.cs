using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Units
{
    private void FixedUpdate()
    {
        if(Base.Enemies.Count == 0)
        {
            Action_Type = "Patrol";
        }
        if(Base.Enemies.Count != 0)
        {
            Action_Type = "Attack";
        }
    }
}
