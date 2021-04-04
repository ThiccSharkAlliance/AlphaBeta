using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Range : MonoBehaviour
{
    [SerializeField]
    public float Area_Of_Control;

    public List<GameObject> Enemies = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Enemies.Remove(other.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Area_Of_Control);
    }
}
