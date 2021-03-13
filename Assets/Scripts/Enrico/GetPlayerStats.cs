using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class GetPlayerStats : MonoBehaviour
{
    //References
    private Manager manager;
    private VirtualCurrency virtualCurrency;
    // private PlayFabAuth PFA;  ////// TO UNCOMMENT FOR THE LOGIN PANEL NOW USE DEV_LOGIN!!!!! 
    private DEV_LOGIN dEV_LOGIN; 
    private Inventory inventory;
    
    //Variables
    public bool gotCurrency = false;
    private string sceneName = "ShipControlTestScene"; // Change with final game scene name.

    private void OnEnable()
    {
        // Instance refs---- -
         virtualCurrency = gameObject.GetComponent<VirtualCurrency>();

        /// PFA = gameObject.GetComponent<PlayFabAuth>();
        dEV_LOGIN = gameObject.GetComponent<DEV_LOGIN>();

        manager = FindObjectOfType<Manager>();
        inventory = gameObject.GetComponent<Inventory>();

    }

    //private void Awake()
    //{
    //    //Instance refs-----
    //    virtualCurrency = gameObject.GetComponent<VirtualCurrency>();
        
    //    /// PFA = gameObject.GetComponent<PlayFabAuth>();
    //    dEV_LOGIN = gameObject.GetComponent<DEV_LOGIN>();
       
    //    manager = FindObjectOfType<Manager>();
    //    inventory = gameObject.GetComponent<Inventory>();
    //    ///---------------
    //}

    private void Update()
    {
        // CheckStatsAndLoadScene();

       
    }

    public void FetchCurrency()
    {
        
        if (!gotCurrency)
        {
            //PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result => {

            //    virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
            //    gotCurrency = true;

            //}, error => {
            //    Debug.Log("Error ritriving you currency");

            //}, null);

            PlayFabClientAPI.LoginWithPlayFab(dEV_LOGIN.loginRequest, result2 => {
               
                virtualCurrency.SealsCurrency = result2.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
                gotCurrency = true;

            }, error => {
               // Debug.Log("Error ritriving you currency");
                Debug.LogError(error.ErrorMessage);

            }, null);
        }
    }

    //UNCOMMENT FOR FINAL!!!

    //private void CheckStatsAndLoadScene()
    //{
    //    if (gotCurrency == true)
    //    {
    //        if (sceneName != SceneManager.GetActiveScene().name)
    //        {
    //            SceneManager.LoadScene("ShipControlTestScene");
    //            gotCurrency = false;
                
    //        }
    //    }
    //}
}
