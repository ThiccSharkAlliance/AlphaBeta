using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX; 


public class activeThrusters : MonoBehaviour

{
    public bool isMoving = false;
    public bool isBoost = false; 
    public VisualEffect[] thrustersVFX;
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
        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    isMoving = true;
           
        //}
        //if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //   isMoving = false;
        //}

        if (isMoving)
        {
            
            foreach(VisualEffect v in thrustersVFX)
            {
                v.SetInt("thrusterSpawn", 16);
                v.SetInt("backlightSpawn", 11111);
            }
            
        }
        if (!isMoving)
        {
            foreach (VisualEffect v in thrustersVFX)
            {
                v.SetInt("thrusterSpawn", 0);
                v.SetInt("backlightSpawn", 0);
            }
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

        foreach (VisualEffect v in thrustersVFX)
        {
            if (isBoost)
            {
                v.SetInt("powerboostSpawn", 4000);
            }
            if (!isBoost)
            {
                v.SetInt("powerboostSpawn", 0);
            }
        }
            





    }
}

