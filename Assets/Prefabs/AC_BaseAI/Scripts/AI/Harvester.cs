using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : Units
{
    [SerializeField]
    public List<Transform> Resources = new List<Transform>();

    public Transform Closest_Resource;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (Resources != null)
        {
            if (Closest_Resource == null)
            {
                Closest_Resource = Resources[0];
                foreach(Transform t in Resources)
                {
                    if(Vector3.Distance(Agent.transform.position, t.position) < Vector3.Distance(Agent.transform.position, Closest_Resource.position))
                    {
                        Closest_Resource = t;
                    }
                }
            }
            Destination = Closest_Resource.position;
            Action_Type = "Harvest";
        }
    }
}
