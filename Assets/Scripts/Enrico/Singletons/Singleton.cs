using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    Singleton singleton;
    private void OnEnable()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            if (singleton != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);

    }//Singleton Enrico
}
