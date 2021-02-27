using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Units : MonoBehaviour
{
    [SerializeField]
    public Base_Range Base;
    //public Transform Player;

    public NavMeshAgent Agent;

    public string Action_Type = "Patrol";

    [SerializeField]
    float Action_Range;

    public Vector3 Destination;

    public Fuzzy_Coordination Fuzzy_AI;

    Transform Turret;

    void Start()
    {
        Action();
        Fuzzy_AI = GetComponentInParent<Fuzzy_Coordination>();
    }

    void Update()
    {
        if (Action_Type == "Patrol" && ((Vector3.Distance(Agent.transform.position, Destination) <= 4) || (Agent.transform.position.x == Destination.x && Agent.transform.position.z == Destination.z)))
        {
            Action();
        }
        if (Action_Type == "Protect")
        {
            Action();
        }
        if (Action_Type == "Attack")
        {
            Action();
        }
        if (Action_Type == "Harvest" && ((Vector3.Distance(Agent.transform.position, Destination) <= 7) || (Vector3.Distance(Agent.transform.position, Base.transform.position) <= 4)))
        {
            Action();
        }
    }

    public void Action()
    {
        switch (Action_Type)
        {
            case "Patrol":
                {
                    float X_Pos = Random.Range(Random.Range(Base.transform.position.x, Base.transform.position.x + Base.Area_Of_Control), (Random.Range(Base.transform.position.x, Base.transform.position.x - Base.Area_Of_Control)));
                    float Z_Pos = Random.Range(Random.Range(Base.transform.position.z, Base.transform.position.z + Base.Area_Of_Control), (Random.Range(Base.transform.position.z, Base.transform.position.z - Base.Area_Of_Control)));

                    Destination = new Vector3(X_Pos, Base.transform.position.y, Random.Range(Base.transform.position.z, Z_Pos));

                    Agent.SetDestination(Destination);

                    return;
                }
            case "Protect":
                {
                    Destination = new Vector3 (Base.Enemies[0].transform.position.x, Base.Enemies[0].transform.position.y, Base.Enemies[0].transform.position.z);

                    Agent.SetDestination(Destination);

                    return;
                }
            case "Attack":
                {
                    if (Turret == null)
                    {
                        Destination = Fuzzy_AI.Enemy_Base.transform.position;

                        Agent.SetDestination(Destination);

                        if (Fuzzy_AI.Enemy_Turrets != null)
                        {
                            foreach (Transform Tr in Fuzzy_AI.Enemy_Turrets)
                            {
                                if (Vector3.Distance(transform.position, Tr.position) < Action_Range)
                                {
                                    Turret = Tr;
                                }
                            }
                        }
                    }
                    else
                    {
                        Destination = Turret.position;

                        Agent.SetDestination(Destination);
                    }

                    return;
                }
            case "Harvest":
                {
                    if (Vector3.Distance(Agent.transform.position, Base.transform.position) < Vector3.Distance(Agent.transform.position, Destination))
                    {
                        Agent.SetDestination(Destination);
                    }
                    else
                    {
                        Agent.SetDestination(Base.transform.position);
                    }
                    return;
                }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Agent.transform.position, Action_Range);
    }
}
