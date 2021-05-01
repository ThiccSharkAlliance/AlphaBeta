using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Shooting : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();

    public Transform Closest_Enemy;

    [SerializeField]
    Transform Closest_Enemy_Placeholder;

    public Transform Barrels;

    Fuzzy_Coordination Fuzzy_AI;

    [SerializeField]
    GameObject Bullet_Prefab;

    [SerializeField]
    GameObject[] Spawn_Positions;
    int i = 0;

    [SerializeField]
    public int Bullet_Damage;

    // Start is called before the first frame update
    void Start()
    {
        Fuzzy_AI = GetComponentInParent<Fuzzy_Coordination>();
        Barrels = transform.Find("Gun_elevators");
        Closest_Enemy = Closest_Enemy_Placeholder;
        InvokeRepeating("Update_Closest_Enemy", 1f, 1f);
        InvokeRepeating("Shoot", 1f, 2f);
    }

    // Update is called once per frame
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

        Barrels.LookAt(Closest_Enemy);

        if (Closest_Enemy != Closest_Enemy_Placeholder)
        {
            transform.rotation = new Quaternion(0, Barrels.rotation.y, 0, Barrels.rotation.w);
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
