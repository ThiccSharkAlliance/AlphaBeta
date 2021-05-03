using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SharkWeapons : MonoBehaviour
{
    VisualEffect sharkdoken;
    public float timer;
    public bool charged;
    public float charging;
    // Start is called before the first frame update
    void Start()
    {

        sharkdoken = GetComponent<VisualEffect>();
        sharkdoken.SetFloat("fireprojectile", 0);
        sharkdoken.SetFloat("chargerate", 0);
        //sharkdoken.initialEventName = "Charge";


        timer = 0f;
        charged = false;
        charging = 0f;
        

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sharkdoken.SetFloat("chargerate", 11);
           
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            timer = 0f;
           
        }
        if (charged == true)
        {
            timer = 0f;
            sharkdoken.SetFloat("fireprojectile", 1);

        }
    }
}
