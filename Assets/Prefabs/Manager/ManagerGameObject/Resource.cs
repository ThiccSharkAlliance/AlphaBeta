using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PrefabManager", menuName = "ScriptableObjects/New Resource", order = 4)]

public class Resource : ScriptableObject
{
    public string resourceName;
    public int buildingCost;
    public bool unlocked;
    public Sprite iconSprite;
    public GameObject prefab;
    public int damage;
    public int health;
   // [System.Serializable]

}
