using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    CanvasSingleton canvasSingleton;
    private void OnEnable()
    {
        if (canvasSingleton == null)
        {
            canvasSingleton = this;
        }
        else
        {
            if (canvasSingleton != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }//Singleton Enrico
}
