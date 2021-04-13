using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX;

public class Laser : MonoBehaviour
{     
    public bool isFiring = false;
    public bool isSparking = false;
    public bool isOverheating = false;
    public bool isCooling = false;
    public VisualEffect laserVFX;
    public float sparkTimer;
    public float smokingTimer;
    public float coolingTimer; 


    void Start()
    {
            

    }

    // Update is called once per frame
    void Update()
    {

        //basic muzzle flash and muzzle flash sparks vfx\\
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!isCooling)
            {
                isFiring = true;
                sparkTimer += 1f * Time.deltaTime;
                if (sparkTimer >= 3)
                {
                    isSparking = true;

                }

                if (sparkTimer >= 7)
                {
                    isCooling = true;
                    sparkTimer = 0;
                }
            }
           

        }


        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isFiring = false;
            isSparking = false; 
            sparkTimer = 0f;
           
        }

        if (isFiring)
        {
            laserVFX.SetInt("muzzleFlashSpawn", 10);
        }
        if (!isFiring)
        {
            laserVFX.SetInt("muzzleFlashSpawn", 0);   
        }
        if (isSparking)
        {
            laserVFX.SetInt("MFSparksSpawn", 6);
        }
        if (!isSparking)
        {
            laserVFX.SetInt("MFSparksSpawn", 0);
        }
        if (isOverheating)
        {
            laserVFX.SetInt("smokeSpawn", 100);  
        }
        if (!isOverheating)
        {
            laserVFX.SetInt("smokeSpawn", 0);
        }


        if (isCooling)
        {
            coolingTimer += 1f * Time.deltaTime;
            isOverheating = true;
            isFiring = false;
            isSparking = false; 
        }
        if (!isCooling)
        {
            coolingTimer = 0;
            isOverheating = false;
        }



        if (coolingTimer >= 4)
        {
            isSparking = false;
            isOverheating = false;
            isCooling = false;
            
        }

        if (smokingTimer >= 4) ////// dont think this is needed at all
        {
            smokingTimer = 0;

        }

        
    }
}