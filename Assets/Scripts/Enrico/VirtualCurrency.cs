using System.IO;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class VirtualCurrency : MonoBehaviour
{

    //References
    PlayFabAuth PFA;
    //private DEV_LOGIN dEV_LOGIN;
    private GetPlayerStats gps;
    private Manager manager;
    private Resources allResources;

    //Currency Variebles
    protected int sealsCurrrency;
    protected List<ItemInstance> inventoryItem = new List<ItemInstance>();

    //Properties
    public int SealsCurrency { get { return sealsCurrrency; } set { sealsCurrrency = value; } }
    public List<ItemInstance> InventoryItems { get { return inventoryItem; } set { inventoryItem = value; } } // to take to inventory class 


    private void Awake()
    {
        gps = gameObject.GetComponent<GetPlayerStats>();
        //dEV_LOGIN = gameObject.GetComponent<DEV_LOGIN>();
         PFA = gameObject.GetComponent<PlayFabAuth>();
    }


    public void GetCatalog()
    {
        manager = FindObjectOfType<Manager>().GetComponent<Manager>();
        allResources = manager.allResources;
        GetCatalogItemsRequest catalogItems = new GetCatalogItemsRequest();
        catalogItems.CatalogVersion = "Upgrades";
        PlayFabClientAPI.GetCatalogItems(catalogItems, result =>
        {
            List<CatalogItem> items = result.Catalog;
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
                        if (jsonSet != null)
                        {
                            info.damage = jsonSet.Damage;
                            info.health = jsonSet.Health;
                        }
                    }
                }
            }
        }, error =>{ });
    }

    public void GetUserInventory()
    {
        GetUserInventoryRequest userInventory = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult => { InventoryItems = inventoryResult.Inventory; }, error => { });
    }

    public void PurchaseUpgrade(string itemID, int price)
    {
        int itemCount = 0;
        bool hasItem = false;
        PurchaseItemRequest purchaseRequest = new PurchaseItemRequest();
        purchaseRequest.CatalogVersion = "Upgrades";
        purchaseRequest.ItemId = itemID;
        purchaseRequest.VirtualCurrency = "SL";
        purchaseRequest.Price = price;
        GetUserInventoryRequest userInventory = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult => 
        {
            InventoryItems = inventoryResult.Inventory;
            foreach (ItemInstance item in inventoryResult.Inventory)
            {
                if (item.ItemId == purchaseRequest.ItemId && itemCount == 0)
                {
                    hasItem = true;
                    itemCount++;
                    ConsumeLootItem(item);
                }
            }
            if (!hasItem)
            {
                PlayFabClientAPI.PurchaseItem(purchaseRequest, result =>{
                    SealsCurrency -= price;
                    ConsumeItem(purchaseRequest.ItemId);
                }, error => { Debug.Log(error.ErrorMessage + " " + itemID); });
            }
        }, error => { });
    }

    public void ConsumeItem(string itemInstance)
    {
        ConsumeItemRequest consumeItemRequest = new ConsumeItemRequest();
        GetUserInventoryRequest userInventory = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(userInventory, inventoryResult =>
        {
            InventoryItems = inventoryResult.Inventory;
            foreach (ItemInstance item in InventoryItems)
            {
                if(item.ItemId != itemInstance) { return; }
                consumeItemRequest.ItemInstanceId = item.ItemInstanceId;
                consumeItemRequest.ConsumeCount = 1;
                PlayFabClientAPI.ConsumeItem(consumeItemRequest, consumeReq => { }, error => { Debug.Log(error.ErrorMessage); });
            }
        }, error => { });
    }

    public void GrantLootBox()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.CatalogVersion = "Upgrades";
        request.ItemId = "10ItemDrop";
        request.VirtualCurrency = "SL";
        request.Price = 0;
        PlayFabClientAPI.PurchaseItem(request, result => { }, error => { Debug.Log(error.ErrorMessage); });
    }

    public void ConsumeLootItem(ItemInstance itemInstance)
    {
        ConsumeItemRequest consumeItemRequest = new ConsumeItemRequest();
        consumeItemRequest.ItemInstanceId = itemInstance.ItemInstanceId;
        consumeItemRequest.ConsumeCount = 1;
        PlayFabClientAPI.ConsumeItem(consumeItemRequest, consumeReq => { }, error => { Debug.Log(error.ErrorMessage); });
    }

    public void AddCurrency()
    {
        AddUserVirtualCurrencyRequest currencyRequest = new AddUserVirtualCurrencyRequest();
        currencyRequest.VirtualCurrency = "SL";
        currencyRequest.Amount = 10000;
        PlayFabClientAPI.AddUserVirtualCurrency(currencyRequest, result => { }, error => { Debug.Log(error.ErrorMessage); });
        gps.RefreshCurrency();
    }

}