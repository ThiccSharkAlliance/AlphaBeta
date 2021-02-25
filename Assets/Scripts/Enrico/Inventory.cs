using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class Inventory : MonoBehaviour
{

    //Instance Refs
    private Manager manager;

    public void GetCatalog()
    {
        manager = FindObjectOfType<Manager>().GetComponent<Manager>();

        GetCatalogItemsRequest catalogItems = new GetCatalogItemsRequest();
        catalogItems.CatalogVersion = "Upgrades";

        PlayFabClientAPI.GetCatalogItems(catalogItems, result =>
        {
            List<CatalogItem> items = result.Catalog;

            foreach (CatalogItem i in items)
            {
                uint cost = i.VirtualCurrencyPrices["SL"];
                
                foreach (var info in manager.resourceInfo)
                {
                    if (info.resourceName == i.DisplayName)
                    {
                        info.buildingCost = (int)cost;
                        
                        Debug.Log("cost " + cost + " name " + i.DisplayName);
                    }
                    
                }
            }
        }, error =>
        {

        });
    }
}

    
