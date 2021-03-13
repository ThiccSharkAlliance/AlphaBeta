using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementIndicatorSingleton : MonoBehaviour
{
    PlacementIndicatorSingleton placementIndicatorSingleton;
    private void OnEnable()
    {
        if (placementIndicatorSingleton == null)
        {
            placementIndicatorSingleton = this;
        }
        else
        {
            if (placementIndicatorSingleton != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }//Singleton Enrico
}
