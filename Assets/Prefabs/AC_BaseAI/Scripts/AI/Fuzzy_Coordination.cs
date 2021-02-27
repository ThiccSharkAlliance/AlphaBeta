using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuzzy_Coordination : MonoBehaviour
{
    public enum Status { Defensive, Offensive};
    [SerializeField]
    public Status Active_Status;

    [SerializeField]
    public GameObject Enemy_Base;
    public List<Transform> Enemy_Turrets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Active_Status = Status.Defensive;
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy_Base.transform.childCount != 0)
        {
            if (Enemy_Turrets.Count <= Enemy_Base.transform.childCount)
            {
                for (int i = 0; i < Enemy_Base.transform.childCount; i++)
                {
                    if (Enemy_Base.transform.GetChild(i).tag == "Turret")
                    {
                        Enemy_Turrets.Add(Enemy_Base.transform.GetChild(i));
                    }
                }
            }
        }
    }
}
