using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Units
{
    Tank_Shooting Shoot;

    public override void Start()
    {
        //base.Start();
        Action();
        Fuzzy_AI = GetComponentInParent<Fuzzy_Coordination>();
        Shoot = GetComponentInChildren<Tank_Shooting>();

        HP = GetComponentInChildren<Health_Bar>();
        HP.Set_Max_Health(Health);
    }

    private void FixedUpdate()
    {
        if(Fuzzy_AI == null)
        {
            Fuzzy_AI = FindObjectOfType<Fuzzy_Coordination>();
            /*if (transform.parent == null)
            {
                Debug.Log("Doesn't have parent");
                Fuzzy_AI = FindObjectOfType<Fuzzy_Coordination>();
            }
            else
            {
                Fuzzy_AI = GetComponentInParent<Fuzzy_Coordination>();
            }*/
        }
        if(Base == null)
        {
            /*if (transform.parent == null)
            {
                Base = FindObjectOfType<Base_Range>();
            }
            else
            {
                Base = GetComponentInParent<Base_Range>();
            }*/
            Base = FindObjectOfType<Base_Range>();
        }

        /*if (Shoot.Enemies.Count != 0)
        {
            Action_Type = "Shoot";
        }
        else
        {
            if (Fuzzy_AI.Active_Status == Fuzzy_Coordination.Status.Defensive)
            {
                if (Base.Enemies.Count != 0)
                {
                    Action_Type = "Protect";
                }
                else
                {
                    Action_Type = "Idle";
                }
            }

            if (Fuzzy_AI.Active_Status == Fuzzy_Coordination.Status.Offensive)
            {
                Action_Type = "Attack";
            }
        }*/
    }

}
