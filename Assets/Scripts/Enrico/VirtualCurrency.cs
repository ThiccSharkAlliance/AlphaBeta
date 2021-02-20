﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class VirtualCurrency : MonoBehaviour
{
    //Currency
    private static int sealsCurrrency;
    private static List<ItemInstance> inventoryItem = new List<ItemInstance>();

    public static int SealsCurrency { get { return sealsCurrrency;} set{ sealsCurrrency = value; }}
    public static List<ItemInstance> InventoryItems { get { return inventoryItem; } set { inventoryItem = value; }}


    PlayFabAuth PFA;
    
    private void Awake()
    {
        
        PFA = gameObject.GetComponent<PlayFabAuth>();
    }

    public void FetchCurrency()
    {
        PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result => {
           
            SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currncy value.
            Debug.Log(SealsCurrency); 

        }, error => {
            Debug.Log("Error ritriving you currency");
           
        }, null);
    }
    
    public void PurchaseUpgrade(string itemID, int price)
    {

        PurchaseItemRequest purchaseRequest = new PurchaseItemRequest();
        purchaseRequest.CatalogVersion = "Upgrades";
        purchaseRequest.ItemId = itemID;
        purchaseRequest.VirtualCurrency = "SL";
        purchaseRequest.Price = price;

        GetUserInventoryRequest  userInventory = new GetUserInventoryRequest();

        
        PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult => 
        {
            InventoryItems = inventoryResult.Inventory;
           
            bool hasItem = false;
            foreach (ItemInstance items in InventoryItems )
            {
                if(InventoryItems.Count == 0)
                {
                    hasItem = false;
                }
                if(items.ItemId == itemID)
                {
                    // item alredy in inventory/purchaesed
                    hasItem = true;
                    Debug.LogWarning("Item " + items.DisplayName + " Already purchased");
                }
                else
                {
                    hasItem = false;
                    Debug.Log(hasItem);
                }
            }
            if (hasItem == false)
            {

                PlayFabClientAPI.PurchaseItem(purchaseRequest, result =>{
                    SealsCurrency -= price;
                    Debug.Log(SealsCurrency);

                }, error => { Debug.Log(error.ErrorMessage); });
            }
        }, error => { Debug.Log(error.ErrorMessage); }); 
    }
}

   

