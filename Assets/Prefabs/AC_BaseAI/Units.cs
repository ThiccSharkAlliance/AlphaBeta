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

    Vector3 Destination;

    void Start()
    {
        Debug.Log(Action_Type);
        Action();
    }

    void Update()
    {
        if (Action_Type == "Patrol" && Vector3.Distance(Agent.transform.position, Destination) <=  3)
        {
            Action();
        }
        if(Action_Type == "Attack")
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
            case "Attack":
                {
                    Destination = new Vector3 (Base.Enemies[0].transform.position.x, Base.Enemies[0].transform.position.y, Base.Enemies[0].transform.position.z);

                    Agent.SetDestination(Destination);

                    return;
                }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Action_Range);
    }
}
