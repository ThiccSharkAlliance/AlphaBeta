using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deselect : MonoBehaviour
{
    AI[] Objects;
    // Start is called before the first frame update
    void Start()
    {
        Objects = FindObjectsOfType<AI>();
    }

    private void OnMouseDown()
    {
        foreach(AI a in Objects)
        {
            a.Selected = 0;
        }
    }
}
