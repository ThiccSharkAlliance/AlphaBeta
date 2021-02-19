using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bake_NavMesh : MonoBehaviour
{
    public NavMeshSurface Surface;

    // Start is called before the first frame update
    void Start()
    {
        Surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
