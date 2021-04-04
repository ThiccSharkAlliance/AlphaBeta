using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    GameObject target;
    public Material blueMat, redMat;
    Camera mainCam;
    Vector3 mainCamTransformForward, mainCamTransformRight;
    const float heightOffGround = 2f;
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
    activeThrusters at;
    Manager manager => FindObjectOfType<Manager>().GetComponent<Manager>();
    float shipRot = 0f;
    Vector3 shipVel = Vector3.zero;
    Transform[] thrusters = new Transform[6];
    Transform shipTrans;
    float rotDiff = 0;

    void Start()
    {
        thrusters[0] = GameObject.Find("Bottom_Thruster").transform;
        thrusters[1] = GameObject.Find("Bottom_Thruster1").transform;
        thrusters[2] = GameObject.Find("Bottom_Thruster2").transform;
        thrusters[3] = GameObject.Find("Bottom_Thruster3").transform;
        thrusters[4] = GameObject.Find("Bottom_Thruster4").transform;
        thrusters[5] = GameObject.Find("Bottom_Thruster5").transform;
        shipTrans = GameObject.Find("Ship").transform;
        at = GetComponent<activeThrusters>();
        mainCam = Camera.main;
        mainCam.GetComponent<CameraControl>().playerPoint = this.gameObject;
        target = transform.Find("Target").gameObject;
        rb = GetComponent<Rigidbody>();
        gunPoint = GameObject.Find("GunPoint").transform;
        targetMat = target.GetComponent<MeshRenderer>().material;
        GameObject VE = GameObject.Find("VoxelEngine");
        VE.GetComponent<VoxelTerrain.Voxel.InfoData.WorldInfo>().Origin = transform;
    }

    void Update()
    {
        at.isMoving = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //cast a ray
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) //if the ray hits something
        {
            target.transform.position = weaponFocus = hit.point;
            shipForward = Vector3.Normalize(new Vector3(weaponFocus.x - transform.position.x, 0f, weaponFocus.z-transform.position.z));

            transform.forward = Vector3.Lerp(transform.forward, shipForward, 0.1f);
           // transform.forward = shipForward;


            if (Input.GetMouseButton(0) && !buildMode)
            {
                if (timeSinceLastBullet > weaponFireRate)
                {
                    timeSinceLastBullet -= weaponFireRate;
                    gunPoint = GameObject.Find("GunPoint").transform;
                    Vector3 aimVector = Vector3.Normalize(weaponFocus - gunPoint.position);
                    GameObject newBullet = GameObject.Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
                    newBullet.GetComponent<Rigidbody>().velocity = aimVector * bulletSpeed;
                }
            }
            if (Input.GetMouseButton(0) && buildMode)
            { 
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
                    target.GetComponent<MeshRenderer>().material = blueMat;
                    targetColour = 1;
                    manager.RebuildSelectableBuildings();////////////////////WORKS BETTER!!!!
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



        rotDiff = transform.rotation.y - shipRot;

        shipRot = transform.rotation.y;



        //print("Rot diff = " + rotDiff);

        if(Physics.Raycast(transform.position, -transform.up, out hit))
        {
            transform.Translate(0f, heightOffGround - hit.distance + (Mathf.Sin(Time.time)*0.2f), 0f);
        }
        shipMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        shipVel = (mainCamTransformForward * shipMovement.z * shipSpeed) +(mainCamTransformRight * shipMovement.x * shipSpeed);

        // ship transform (child) = something transform. rotation & ship Vel.

        Debug.DrawLine(transform.position, transform.position+ transform.forward*5, Color.red);
        Debug.DrawLine(transform.position, transform.position + shipVel.normalized * 5, Color.blue);

        float angleOffset = Vector3.SignedAngle(transform.forward, shipVel.normalized,transform.up) * Mathf.Deg2Rad;
        print(angleOffset);

       

        Vector3 rotOffset = new Vector3(-Mathf.Sin(angleOffset)*15f*shipVel.normalized.magnitude, shipTrans.localRotation.y -90f, -Mathf.Cos(angleOffset)*15f * shipVel.normalized.magnitude);
        shipTrans.localRotation = Quaternion.Slerp(shipTrans.localRotation, Quaternion.Euler(rotOffset),0.1f);
        print(rotOffset);

        CalcTrhusterRot();

    }

    private void FixedUpdate()
    {
        rb.velocity = shipVel;
    }

    void CalcTrhusterRot()
    {
        if (shipVel == Vector3.zero)
        { //ship is not moving
            if(rotDiff > 0.0001f)
            { //ship rotating cc
                thrusters[3].localRotation = thrusters[0].localRotation = Quaternion.Slerp(thrusters[0].localRotation, Quaternion.Euler(315, 0, 0), 0.1f);
                thrusters[4].localRotation = thrusters[1].localRotation = Quaternion.Slerp(thrusters[1].localRotation, Quaternion.Euler(45, 0, 0), 0.1f);
                thrusters[5].localRotation = thrusters[2].localRotation = Quaternion.Slerp(thrusters[2].localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
                print("Ship rotating right?");
            }
            else if(rotDiff < -0.0001f)
            {
                thrusters[3].localRotation = thrusters[0].localRotation = Quaternion.Slerp(thrusters[0].localRotation, Quaternion.Euler(45, 0, 0), 0.1f);
                thrusters[4].localRotation = thrusters[1].localRotation = Quaternion.Slerp(thrusters[1].localRotation, Quaternion.Euler(315, 0, 0), 0.1f);
                thrusters[5].localRotation = thrusters[2].localRotation = Quaternion.Slerp(thrusters[2].localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
                print("Ship rotating left?");
            }
            else
            { //ship static
                thrusters[0].localRotation = Quaternion.Slerp(thrusters[0].localRotation, Quaternion.Euler(30,0,30), 0.1f);
                thrusters[1].localRotation = Quaternion.Slerp(thrusters[1].localRotation, Quaternion.Euler(45,0,0), 0.1f);
                thrusters[2].localRotation = Quaternion.Slerp(thrusters[2].localRotation, Quaternion.Euler(30,0,330), 0.1f);
                thrusters[3].localRotation = Quaternion.Slerp(thrusters[3].localRotation, Quaternion.Euler(330, 0, 30), 0.1f);
                thrusters[4].localRotation = Quaternion.Slerp(thrusters[4].localRotation, Quaternion.Euler(315, 0, 0), 0.1f);
                thrusters[5].localRotation = Quaternion.Slerp(thrusters[5].localRotation, Quaternion.Euler(330, 0, 30), 0.1f);
                print("Ship is static");
            }
        }
        else
        { //ship is moving
            Vector3 svn = shipVel.normalized;
            for (int i = 0; i < 6; i++){
                thrusters[i].rotation = thrusters[i].rotation = Quaternion.Slerp(thrusters[i].rotation, Quaternion.Euler(svn.z*75, 0, -svn.x*75), 0.1f);
            }

        }
    }
}
