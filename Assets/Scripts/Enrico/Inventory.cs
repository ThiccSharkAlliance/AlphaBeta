﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.Json;
using PlayFab.ClientModels;

public class Inventory : MonoBehaviour
{

    //Instance Refs
    private Manager manager;
    Resources allResources;

    public void GetCatalog()
    {
        manager = FindObjectOfType<Manager>().GetComponent<Manager>();
        allResources = manager.allResources;
        foreach(var res in allResources.resourceInfo)
        {
            print(res.resourceName);
        }
        GetCatalogItemsRequest catalogItems = new GetCatalogItemsRequest();
        catalogItems.CatalogVersion = "Upgrades";

        PlayFabClientAPI.GetCatalogItems(catalogItems, result =>
        {
            List<CatalogItem> items = result.Catalog;
           // result.CustomData //////////////////////////////////TO TEST WITH THIS and with more keys per item
            foreach (CatalogItem i in items)
            {
                uint cost = i.VirtualCurrencyPrices["SL"];
                
                ///////JSON STUFF HERE/////////////////////////////
                string json = i.CustomData;                                                 // Store cloudData into a string
                File.WriteAllText(Application.dataPath + "/savedJson.json", json);         //Write a json file to assets
                string read = File.ReadAllText(Application.dataPath + "/savedJson.json"); //Read from json file and store it
                JsonDataHolder jsonSet = JsonUtility.FromJson<JsonDataHolder>(read);     // Apply the read data and set the data the json class.
                ////////////////////////////////////////////////
                
                foreach (var info in allResources.resourceInfo)
                {
                    if (info.resourceName == i.DisplayName)
                    {
                        info.buildingCost = (int)cost;
                        
                        ////Apply Json Values to the player.
                        if(jsonSet != null)
                        {
                            info.damage = jsonSet.Damage;
                        }
                        Debug.Log("cost " + cost + " name " + i.DisplayName);
                    }
                    
                }
            }
        }, error =>
        {
            
        });
    }
}

    
