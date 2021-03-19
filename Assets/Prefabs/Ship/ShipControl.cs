using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    GameObject target;
    public Material blueMat, redMat;
    Camera mainCam;
    Vector3 mainCamTransformForward, mainCamTransformRight;
    const float heightOffGround = 0.4f;
    Vector3 shipMovement;
    Rigidbody rb;
    float shipSpeed = 5f;
    Vector3 shipForward;
    public LayerMask IgnoreMe;
    public bool invertMask;
    public GameObject bulletPrefab;
    Vector3 weaponFocus;
    float weaponFireRate = 0.1f;
    float bulletSpeed = 20f;
    float timeSinceLastBullet;
    Transform gunPoint;
    Material targetMat;
    int targetColour = 0; //0 = red, 1 = blue
    bool buildMode = false; //0= fire 1=building

    Manager manager => FindObjectOfType<Manager>().GetComponent<Manager>();  


    void Start()
    {
        mainCam = Camera.main;
        mainCam.GetComponent<CameraControl>().playerPoint = this.gameObject;
        target = transform.Find("Target").gameObject;
        rb = GetComponent<Rigidbody>();
        gunPoint = GameObject.Find("GunPoint").transform;
        targetMat = target.GetComponent<MeshRenderer>().material;
        
    }

    void Update()
    {
        Vector2 screenPos = Input.mousePosition; //Get the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //cast a ray
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit)) //if the ray hits something
        {
            weaponFocus = hit.point;
            target.transform.position = weaponFocus = hit.point;
            shipForward = Vector3.Normalize(new Vector3(weaponFocus.x - transform.position.x, 0f, weaponFocus.z-transform.position.z));
            transform.forward = shipForward;
            if (Input.GetMouseButton(0) && !buildMode)
            {
                if (timeSinceLastBullet > weaponFireRate)
                {
                    //print("bang!");
                    timeSinceLastBullet -= weaponFireRate;
                    gunPoint = GameObject.Find("GunPoint").transform;
                    Vector3 aimVector = Vector3.Normalize(weaponFocus - gunPoint.position);
                    GameObject newBullet = GameObject.Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
                    newBullet.GetComponent<Rigidbody>().velocity = aimVector * bulletSpeed;
                }
            }
            if (Input.GetMouseButton(0) && buildMode)
            { //attemptToBuild
                print("Can I build it?");
                buildMode = !manager.TryToBuild();
                if (!buildMode)
                {
                    manager.RebuildSelectableBuildings();
                    manager.MakeBuildSelectorVisible();
                }

            }
            if (Input.GetMouseButton(1))
            {
                
                if (targetColour != 1)
                {
                   // print("sET CLOUR BLUE");
                    target.GetComponent<MeshRenderer>().material = blueMat;
                    targetColour = 1;
                    manager.RebuildSelectableBuildings();
                    manager.MakeBuildSelectorVisible();
                    buildMode = !buildMode;

                }
            }
            else
            {
                if (targetMat != redMat)
                {
                    target.GetComponent<MeshRenderer>().material = redMat;
                    targetColour = 0;
                }

            }

        }
        timeSinceLastBullet += Time.deltaTime;


        mainCamTransformForward = Vector3.Normalize(new Vector3(mainCam.transform.forward.x, 0f, mainCam.transform.forward.z));
        mainCamTransformRight = Vector3.Normalize(new Vector3(mainCam.transform.right.x, 0f, mainCam.transform.right.z));

        if(Physics.Raycast(transform.position, -transform.up, out hit))
        {
            transform.Translate(0f, heightOffGround - hit.distance, 0f);
        }
        shipMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        //  rb.velocity = new Vector3(mainCamTransformForward.x*shipMovement.x,0f,mainCamTransformForward.z*shipMovement.z)*shipSpeed;
        rb.velocity = (mainCamTransformForward * shipMovement.z * shipSpeed) + (mainCamTransformRight * shipMovement.x * shipSpeed);
    
    }
}
