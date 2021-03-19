using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX; 


public class activeThrusters : MonoBehaviour

{
    public bool isMoving = false;
    public bool isBoost = false; 
    public VisualEffect thrustersVFX;
    private int TS;
    
    
    void Start()
    {
        //thrustersVFX = GetComponent<VisualEffect>();
      // TS = thrustersVFX.GetInt("backlightSpawn");


    
    }

    // Update is called once per frame
    void Update()
    {

        //basic movement thruster vfx\\
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isMoving = true;
           
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
           isMoving = false;
        }

        if (isMoving)
        {
            thrustersVFX.SetInt("thrusterSpawn", 16);
            thrustersVFX.SetInt("backlightSpawn", 11111);
        }
        if (!isMoving)
        {
            thrustersVFX.SetInt("thrusterSpawn", 0);
            thrustersVFX.SetInt("backlightSpawn", 0);
        }



        //power boost thruster vfx\\

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            isBoost = true;

        }
        else
        {
            isBoost = false; 
        }


        if (isBoost)
        {
            thrustersVFX.SetInt("powerboostSpawn", 4000);
        }
        if (!isBoost)
        {
            thrustersVFX.SetInt("powerboostSpawn", 0);
        }





    }
}

