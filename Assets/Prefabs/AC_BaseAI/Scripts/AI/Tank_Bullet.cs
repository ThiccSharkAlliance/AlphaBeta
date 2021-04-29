using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Bullet : MonoBehaviour
{
    [SerializeField]
    float Speed;

    Transform Target;

    Tank_Shooting Tank;

    Rigidbody rb;

    public int Damage;

    [SerializeField]
    float Life_Time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Tank = GetComponentInParent<Tank_Shooting>();
        Target = Tank.Closest_Enemy;
        Damage = Tank.Bullet_Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector3.forward * Speed;
        }

        Life_Time -= Time.deltaTime;
        if(Life_Time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
