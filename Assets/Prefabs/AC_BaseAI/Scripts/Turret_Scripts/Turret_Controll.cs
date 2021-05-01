using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controll : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();

    public Transform Closest_Enemy;

    [SerializeField]
    Transform Closest_Enemy_Placeholder;

    public float Range = 2750f;
    SphereCollider collider;

    Fuzzy_Coordination Fuzzy_AI;

    [SerializeField]
    GameObject Bullet_Prefab;

    [SerializeField]
    GameObject[] Spawn_Positions;
    int i = 0;

    [SerializeField]
    public int Bullet_Damage;

    private void OnValidate()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = Range;
        Fuzzy_AI = GetComponentInParent<Fuzzy_Coordination>();
    }

    void Start()
    {
        Closest_Enemy = Closest_Enemy_Placeholder;
        Closest_Enemy.position = Vector3.forward;
        collider = GetComponent<SphereCollider>();
        collider.radius = Range;
        InvokeRepeating("Update_Closest_Enemy", 1f, 1f);
        InvokeRepeating("Shoot", 1f, 2f);
    }

    void Update()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i] == null)
            {
                if (Closest_Enemy == null)
                {
                    if (Enemies.Count == 1)
                    {
                        Closest_Enemy = Closest_Enemy_Placeholder;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Closest_Enemy = Enemies[i + 1].transform;
                        }
                        else
                        {
                            Closest_Enemy = Enemies[i - 1].transform;
                        }
                    }
                }
                Enemies.RemoveAt(i);
            }

            if (Closest_Enemy == null)
            {
                if (Enemies.Count == 0)
                {
                    Closest_Enemy = Closest_Enemy_Placeholder;
                }
                else
                {
                    Closest_Enemy = Enemies[i].transform;
                }
            }
        }

        if (Enemies.Count == 0)
        {
            Closest_Enemy = Closest_Enemy_Placeholder;
        }
    }

    void Update_Closest_Enemy()
    {
        if (Enemies.Count != 0)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                /*if (Enemies[i] == null)
                {
                    if (Closest_Enemy == null)
                    {
                        if (Enemies.Count == 1)
                        {
                            Closest_Enemy = Closest_Enemy_Placeholder;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                Closest_Enemy = Enemies[i + 1].transform;
                            }
                            else
                            {
                                Closest_Enemy = Enemies[i - 1].transform;
                            }
                        }
                    }
                    Enemies.RemoveAt(i);
                    return;
                }

                if (Closest_Enemy == null)
                {
                    if (Enemies.Count == 0)
                    {
                        Closest_Enemy = Closest_Enemy_Placeholder;
                    }
                    else
                    {
                        Closest_Enemy = Enemies[i].transform;
                    }
                }*/

                if (Vector3.Distance(gameObject.transform.position, Enemies[i].transform.position) < Vector3.Distance(gameObject.transform.position, Closest_Enemy.transform.position))
                {
                    Closest_Enemy = Enemies[i].transform;
                }
            }
        }
    }

    void Shoot()
    {
        if (Enemies.Count != 0)
        {
            GameObject GO = Instantiate(Bullet_Prefab, Spawn_Positions[i].transform.position, Quaternion.identity);
            GO.transform.parent = this.gameObject.transform;
            i++;
            if (i == Spawn_Positions.Length)
            {
                i = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == Fuzzy_AI.Enemy)
        {
            Enemies.Add(other.gameObject);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.gameObject.tag == Fuzzy_AI.Enemy)
        {
            Enemies.Remove(other.gameObject);
        }*/
    }
}
