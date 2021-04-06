using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject playerPoint;

    [SerializeField] private Vector3 CameraOffset = new Vector3(-13, 10, -13);
    [SerializeField] private GameObject _parent;
    [SerializeField] private int _maxDist = 50;
    [SerializeField] private int _minDist = 15;
    [SerializeField] private int _zoomSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        //CameraOffset = new Vector3(-13, 10, -13);
        //CameraOffset =  transform.position - playerPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerPoint) return;
    
        var playerPos = playerPoint.transform.position;

        _parent.transform.position = playerPos;
        
        transform.LookAt(playerPos);
        
        if (Input.GetKey(KeyCode.E)) _parent.transform.Rotate(Vector3.up, 1, Space.World);
        else if (Input.GetKey(KeyCode.Q)) _parent.transform.Rotate(Vector3.up, -1, Space.World);

        if (Input.mouseScrollDelta.y > 0 && Vector3.Distance(transform.position, _parent.transform.position) > _minDist)
        {
            var direction = _parent.transform.position - transform.position;
            transform.position += direction * (Time.deltaTime * _zoomSpeed);
        }
        
        if (Input.mouseScrollDelta.y < 0 && Vector3.Distance(transform.position, _parent.transform.position) < _maxDist)
        {
            var direction = _parent.transform.position - transform.position;
            transform.position -= direction * (Time.deltaTime * _zoomSpeed);
        }
        
    }
}
