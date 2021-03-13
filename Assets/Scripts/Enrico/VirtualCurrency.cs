using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class VirtualCurrency : MonoBehaviour
{

    //References
    // PlayFabAuth PFA;
    DEV_LOGIN dEV_LOGIN;
    //Currency Variebles
    protected int sealsCurrrency;
    protected  List<ItemInstance> inventoryItem = new List<ItemInstance>();

    //Properties
    public int SealsCurrency { get { return sealsCurrrency;} set{ sealsCurrrency = value; }}
    public List<ItemInstance> InventoryItems { get { return inventoryItem; } set { inventoryItem = value; }} // to take to inventory class 

    
    private void Awake()
    {
        dEV_LOGIN = gameObject.GetComponent<DEV_LOGIN>();
       // PFA = gameObject.GetComponent<PlayFabAuth>();
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

   

