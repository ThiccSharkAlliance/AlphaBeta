using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PrefabManager", menuName = "ScriptableObjects/BaseData", order = 2)]

public class PlayerBaseData : ScriptableObject
{
    public GameObject basePlate;
    public GameObject playerShip;
    public Vector3 basePlatePostion;
    public BuildingInfo[] baseBuildings;
    public float shipHealth;
    public float shipDamage;
// public Resources resourceList;


    [System.Serializable]
    public class BuildingInfo 
    {
        
        public Vector3 buildingPosOffset;
        public float buildingRot;
        public Resource type;
       // public Manager.ResourceInfo type;
    }
}
