using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Range : MonoBehaviour
{
    [SerializeField]
    public float Area_Of_Control;
    SphereCollider collider;

    public List<GameObject> Enemies = new List<GameObject>();

    [SerializeField]
    string Enemy;

    private void OnValidate()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = Area_Of_Control - (Area_Of_Control/100 * 75);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Area_Of_Control);
    }
}
