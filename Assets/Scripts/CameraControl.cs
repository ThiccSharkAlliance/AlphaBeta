using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 CameraOffset;
    public GameObject playerPoint;
    // Start is called before the first frame update
    void Start()
    {
        CameraOffset = new Vector3(-16, 20, -16);
        //CameraOffset =  transform.position - playerPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPoint.transform.position + CameraOffset;
    }
}
