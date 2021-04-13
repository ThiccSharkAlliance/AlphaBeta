using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    float Movement_Speed;

    [SerializeField]
    float Vertical_Speed;
    Vector3 Vertical_Movement;

    public int Selected;

    Rigidbody rb;

    public Camera Cam;

    public NavMeshAgent Agent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Selected = 0;
        Agent.angularSpeed = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Agent.angularSpeed == 0)
            {
                Agent.angularSpeed = Movement_Speed;
            }
            else
            {
                Agent.angularSpeed = 0;
            }

            Ray Ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if(Physics.Raycast(Ray, out Hit))
            {
                Agent.SetDestination(Hit.point);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Agent.angularSpeed == 0)
        {
            Idle();
        }
        if(Agent.angularSpeed != 0 && Selected == 1)
        {
            Active();
        }
    }

    void Idle()
    {
        if(this.transform.position.y <= 1.10)
        {
            Vertical_Movement = new Vector3(0, Vertical_Speed, 0);
        }
        if(this.transform.position.y >= 1.80)
        {
            Vertical_Movement = new Vector3(0, -Vertical_Speed, 0);
        }
        rb.MovePosition(transform.position + Vertical_Movement * Time.fixedDeltaTime);
        //Debug.Log(Vertical_Movement);
    }

    void Active()
    {
        Ray Ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Physics.Raycast(Ray, out Hit))
        {
            Agent.SetDestination(Hit.point);
        }
    }


    private void OnMouseDown()
    {
        Selected = 1;
    }
}
