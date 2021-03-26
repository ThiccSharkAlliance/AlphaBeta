using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class DEV_LOGIN : MonoBehaviour
{
    private string hiddeUserName;
    private string hiddenUserPassword;
    private string hiddenUserEmail;

    private VirtualCurrency vC ; 
    private GetPlayerStats gps;
   // private Inventory inventory;

    public LoginWithPlayFabRequest loginRequest;
    public GetPlayerCombinedInfoRequestParams infoRequest;

    private void Awake()
    {
        vC = gameObject.GetComponent<VirtualCurrency>();  // todo INJECT  
        gps = gameObject.GetComponent<GetPlayerStats>();  // todo INJECT  
        //inventory = gameObject.GetComponent<Inventory>(); // todo INJECT  
    }

    // Start is called before the first frame update
    void Start()
    {
        hiddeUserName = PlayerPrefs.GetString("hiddenUserName");
        hiddenUserPassword = PlayerPrefs.GetString("hiddenUserPassword");
        hiddenUserEmail = PlayerPrefs.GetString("hiddenUserEmail");

        Login();
    }

    private void Login()
    {
        string user = "User" + Random.Range(0, 50000);
        string pass = "Pass" + Random.Range(0, 50000);
        string email = "test" + Random.Range(0, 50000) + "@test.com";

        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Username = user;
        request.Password = pass;
        request.Email = email;

        if(hiddeUserName == string.Empty && hiddenUserPassword == string.Empty)
        {
            PlayFabClientAPI.RegisterPlayFabUser(request, result => {

                PlayerPrefs.SetString("hiddenUserName", user);
                PlayerPrefs.SetString("hiddenUserPassword", pass);
                PlayerPrefs.SetString("hiddenUserEmail", email);
                PlayerPrefs.Save();


            }, error => {

                Debug.LogError(error.ErrorMessage);
            
            });
        }
        else
        {
            loginRequest = new LoginWithPlayFabRequest();
            loginRequest.InfoRequestParameters = infoRequest;
            loginRequest.Username = hiddeUserName;
            loginRequest.Password = hiddenUserPassword;

            PlayFabClientAPI.LoginWithPlayFab(loginRequest, result => {
                Debug.Log("Logged In");

                if (gps.gotCurrency == false)
                {
                   // inventory.GetCatalog();
                    vC.GetCatalog();
                
                    gps.FetchCurrency();
                    vC.GetUserInventory();
                
                }

            }, error => {
            
            });
        }
    }

   
}
