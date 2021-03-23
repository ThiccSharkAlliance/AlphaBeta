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
    GetPlayerStats gps;

    //Currency Variebles
    protected int sealsCurrrency;
    protected  List<ItemInstance> inventoryItem = new List<ItemInstance>();

    //Properties
    public int SealsCurrency { get { return sealsCurrrency;} set{ sealsCurrrency = value; }}
    public List<ItemInstance> InventoryItems { get { return inventoryItem; } set { inventoryItem = value; }} // to take to inventory class 

    
    private void Awake()
    {
        gps = gameObject.GetComponent<GetPlayerStats>();
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

            PlayFabClientAPI.PurchaseItem(purchaseRequest, result =>{
                SealsCurrency -= price;
                Debug.Log(" PURCHASED");
            }, error => { Debug.Log(error.ErrorMessage); });
    }

    public void GetUserInventory()
    {
        GetUserInventoryRequest userInventory = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult => 
        { 
            InventoryItems = inventoryResult.Inventory;
            Debug.Log("UPDATED INv"); 

        }, error => {
        
        });

        
    }

    public void ConsumeItem()
    {
        ConsumeItemRequest itemRequest = new ConsumeItemRequest();

        //GetUserInventory();

        Debug.Log("calledConsume");

        GetUserInventoryRequest userInventory = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult =>
        {
            InventoryItems = inventoryResult.Inventory;
            Debug.Log("UPDATED INv");

            foreach (ItemInstance item in InventoryItems)
            {
                if(item.ItemId == "Wall")
                {
                    Debug.Log("INNNNNNN");
                    itemRequest.ItemInstanceId = item.ItemInstanceId;
                    itemRequest.ConsumeCount = 1;
                    Debug.Log(itemRequest.ItemInstanceId);

                    PlayFabClientAPI.ConsumeItem(itemRequest, consumeReq =>
                    {
                        Debug.Log("CONSUMED");
                        //gps.RefreshCurrency();

                    }, error =>
                    {

                    });
                }
            }
        }, error => {

        });
    }

    public void AddCurrency()
    {
        AddUserVirtualCurrencyRequest currencyRequest = new AddUserVirtualCurrencyRequest();
        currencyRequest.VirtualCurrency = "SL";
        currencyRequest.Amount = 10000;

        PlayFabClientAPI.AddUserVirtualCurrency(currencyRequest, result => {
            Debug.Log("adding money");


        }, error => {
            Debug.LogError(error.ErrorMessage);
        });
            gps.RefreshCurrency();
    }

}

        //PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult => 
        //{
        //    InventoryItems = inventoryResult.Inventory;

        //   // bool hasItem = false;
        //   // foreach (ItemInstance items in InventoryItems) ////////////  For First Welcome Gift. 
        //   // {
        //   //     if (InventoryItems.Count == 0)
        //   //     {
        //   //         hasItem = false;
        //   //     }
        //   //     if (items.ItemId == itemID)
        //   //     {
        //   //         // item alredy in inventory/purchaesed
        //   //         hasItem = true;
        //   //         Debug.LogWarning("Item " + items.DisplayName + " Already purchased");
        //   //         //ConsumeItem();
        //   //     }
        //   //     else
        //   //     {
        //   //         hasItem = false;
        //   //         Debug.Log(hasItem);
        //   //     }
        //   // }
        //   //// if (hasItem == false)
        //   // {
        //    //}
        //}, error => { Debug.Log(error.ErrorMessage); }); 
   

