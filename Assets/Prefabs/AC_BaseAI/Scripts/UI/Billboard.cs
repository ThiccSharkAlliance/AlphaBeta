using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    Transform Cam;

    void LateUpdate()
    {
        transform.LookAt(transform.position + Cam.forward);
    }
}
