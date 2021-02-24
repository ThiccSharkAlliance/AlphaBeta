using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class GetPlayerStats : MonoBehaviour
{
    //References
    private VirtualCurrency virtualCurrency;
    private PlayFabAuth PFA;

    //Variables
    public bool gotCurrency = false;
    private string sceneName = "ShipControlTestScene"; // Change with final game scene.
   
    private void Awake()
    {
       virtualCurrency = gameObject.GetComponent<VirtualCurrency>();
        PFA = gameObject.GetComponent<PlayFabAuth>();
    }

    public void FetchCurrency()
    {
        if (!gotCurrency)
        {
            PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result => {

               virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currncy value.
                Debug.Log(virtualCurrency.SealsCurrency);
                gotCurrency = true;

            }, error => {
                Debug.Log("Error ritriving you currency");

            }, null);
        }
    }

    private void Update()
    {
        CheckStatsAndLoadScene();
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
