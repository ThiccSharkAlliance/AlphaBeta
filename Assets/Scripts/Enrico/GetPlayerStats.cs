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
    private PlayFabAuth PFA;
    private Inventory inventory;
    
    //Variables
    public bool gotCurrency = false;
    private string sceneName = "ShipControlTestScene"; // Change with final game scene name.
   
    private void Awake()
    {
        //Instance refs-----
        virtualCurrency = gameObject.GetComponent<VirtualCurrency>();
        PFA = gameObject.GetComponent<PlayFabAuth>();
        manager = FindObjectOfType<Manager>();
        inventory = gameObject.GetComponent<Inventory>();
        ///---------------
    }

    private void Update()
    {
        CheckStatsAndLoadScene();
    }

    public void FetchCurrency()
    {
        if (!gotCurrency)
        {
            PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result => {

                virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
                gotCurrency = true;

            }, error => {
                Debug.Log("Error ritriving you currency");

            }, null);
        }
    }

    private void CheckStatsAndLoadScene()
    {
        if (gotCurrency == true)
        {
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene("ShipControlTestScene");
                gotCurrency = false;
                
            }
        }
    }
}
