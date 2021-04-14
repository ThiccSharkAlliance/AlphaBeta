using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Bullet : MonoBehaviour
{
    [SerializeField]
    float Speed;

    Transform Target;

    Turret_Controll Turret;

    Rigidbody rb;

    public int Damage;

    [SerializeField]
    float Life_Time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Turret = GetComponentInParent<Turret_Controll>();
        Target = Turret.Closest_Enemy;
        Damage = Turret.Bullet_Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
            //Debug.Log(Target.position);
        }
        else
        {
            rb.velocity = Vector3.forward * Speed;
        }

        Life_Time -= Time.deltaTime;
        if (Life_Time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
