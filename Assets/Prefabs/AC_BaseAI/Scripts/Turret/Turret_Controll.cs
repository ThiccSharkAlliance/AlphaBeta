using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controll : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();

    public Transform Closest_Enemy;

    public float Range = 2750f;
    SphereCollider collider;

    [SerializeField]
    string Enemy;

    private void OnValidate()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = Range;
    }

    void Start()
    {
        Closest_Enemy.position = new Vector3(Range, 0, 0);
        collider = GetComponent<SphereCollider>();
        collider.radius = Range;
    }

    void Update()
    {
        if (Enemies != null)
        {
            InvokeRepeating("Update_Closest_Enemy", 1f, 1f);
        }
        else
        {
            Closest_Enemy.position = new Vector3(Range, 0, 0);
        }
    }

    void Update_Closest_Enemy()
    {
        foreach(GameObject GO in Enemies)
        {
            if(Vector3.Distance(gameObject.transform.position, GO.transform.position) < Vector3.Distance(gameObject.transform.position, Closest_Enemy.transform.position))
            {
                Closest_Enemy = GO.transform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Enemy)
        {
            Enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Enemy)
        {
            Enemies.Remove(other.gameObject);
        }
    }
}
