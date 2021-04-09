using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    bool enabledOnce;
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
        if (!enabledOnce) { gameObject.SetActive(false); }
        enabledOnce = true;
        

    }//Singleton Enrico
}
