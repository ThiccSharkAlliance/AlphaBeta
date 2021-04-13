using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rotate : MonoBehaviour
{
    //Turret_Elevation Barrels;
    Turret_Controll Turret;
    Transform Barrels;

    // Start is called before the first frame update
    void Start()
    {
        //Barrels = GetComponentInChildren<Turret_Elevation>();
        Turret = GetComponentInParent<Turret_Controll>();
        Barrels = transform.Find("Gun_elevators");
    }

    void Update()
    {
        Barrels.LookAt(Turret.Closest_Enemy);
        transform.rotation = new Quaternion(0, Barrels.rotation.y, 0, Barrels.rotation.w);
    }
}
