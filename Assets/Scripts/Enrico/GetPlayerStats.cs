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
    public GameObject playFabCanvas;
    public GameObject _mainCamera;

    //Variables
    [SerializeField]
    private int scene;
    public bool gotCurrency;

    private void OnEnable()
    {
        virtualCurrency = gameObject.GetComponent<VirtualCurrency>();
        PFA = gameObject.GetComponent<PlayFabAuth>();
        manager = FindObjectOfType<Manager>();
    }

    private void Update() => CheckStatsAndLoadScene(); 

    public void FetchCurrency()
    {
        if (gotCurrency) return; 

        PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result =>
        {
            virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
            gotCurrency = true;
        }, error =>{Debug.Log("Error ritriving you currency");}, null);
    }

    private void CheckStatsAndLoadScene()
    {
        if (gotCurrency != true) return; 
        StartCoroutine(LoadScene());
        gotCurrency = false;
    }

    public void RefreshCurrency()
    {
        PlayFabClientAPI.LoginWithPlayFab(PFA.loginRequest, result =>
        {virtualCurrency.SealsCurrency = result.InfoResultPayload.UserVirtualCurrency["SL"]; // fetch the currency value.
        }, error =>{Debug.Log("Error ritriving you currency");}, null);
    }

    private IEnumerator LoadScene()
    {
        _mainCamera.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone) yield return null;
        manager.virtualCurrency = FindObjectOfType<VirtualCurrency>();
        manager.OnStart();
        StartCoroutine(manager.CheckTimer());
        playFabCanvas.SetActive(true);
    }

}
