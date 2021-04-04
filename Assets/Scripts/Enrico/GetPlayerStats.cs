using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class GetPlayerStats : MonoBehaviour
{
   // public GameObject playFabCanvas;
    //References
    private Manager manager;
    private VirtualCurrency virtualCurrency;
    private PlayFabAuth PFA;  ////// TO UNCOMMENT FOR THE LOGIN PANEL NOW USE DEV_LOGIN!!!!! 
                              // private DEV_LOGIN dEV_LOGIN; 

    [SerializeField]
    int scene;
    //Variables
    public bool gotCurrency = false;
    private string sceneName = "ShipControlTestScene"; // Change with final game scene name.

    private void OnEnable()
    {
        // Instance refs---- -
         virtualCurrency = gameObject.GetComponent<VirtualCurrency>();
         PFA = gameObject.GetComponent<PlayFabAuth>();
       // dEV_LOGIN = gameObject.GetComponent<DEV_LOGIN>();
        manager = FindObjectOfType<Manager>();
        //playFabCanvas.SetActive(true);
    }

    #region TO UNCOMMENT FOR FINAL
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
         CheckStatsAndLoadScene();
    }
    #endregion

    public void FetchCurrency()
    {
        if (gotCurrency) { return; }

        PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result =>
        {

            virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
            gotCurrency = true;

        }, error =>
        {
            Debug.Log("Error ritriving you currency");

        }, null);

        #region TO UNCOMMENT FOR FINAL
        //PlayFabClientAPI.LoginWithPlayFab(dEV_LOGIN.loginRequest, result2 => {
        //    virtualCurrency.SealsCurrency = result2.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
        //    gotCurrency = true;
        //}, error => { Debug.LogError(error.ErrorMessage); }, null);
        #endregion
    }

    #region TO UNCOMMENT FOR FINAL
    //UNCOMMENT FOR FINAL!!!

    private void CheckStatsAndLoadScene()
    {
        if (gotCurrency == true)
        {
            //playFabCanvas.SetActive(false);
            //gotCurrency = false;
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                //SceneManager.LoadScene("ShipControlTestScene");


                StartCoroutine(LoadScene());
                gotCurrency = false;


            }
        }
    }
    #endregion

    public void RefreshCurrency()
    {
        #region TO UNCOMMENT FOR FINAL
        PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result =>
        {

            virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
            gotCurrency = true;

        }, error =>
        {
            Debug.Log("Error ritriving you currency");

        }, null);
        #endregion

        //PlayFabClientAPI.LoginWithPlayFab(dEV_LOGIN.loginRequest, result2 => {
        //    virtualCurrency.SealsCurrency = result2.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
        //}, error => { Debug.LogError(error.ErrorMessage); }, null);
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
        {
            yield return null;
        }
        manager.virtualCurrency = FindObjectOfType<VirtualCurrency>();
        manager.OnStart();
        StartCoroutine(manager.CheckTimer());
    }

}
