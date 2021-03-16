using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAuth : MonoBehaviour
{
    PlayFabAuth PFA;
    // Ref to login input fields.
    public InputField userName;
    public InputField userPassword;
    public InputField userEmail;

    //Message to dispaly on login panel.
    public Text message;
    
    public bool isAuthenticated = false;

    public LoginWithPlayFabRequest loginRequest;
    public RegisterPlayFabUserRequest registerRequest;
    public GetPlayerCombinedInfoRequestParams infoRequest;

    private VirtualCurrency vC;
    private GetPlayerStats gps;
    private Inventory inventory;

    private void OnEnable()
    {
        if (PFA == null)
        {
            PFA = this;
        }
        else
        {
            if (PFA != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }//Singleton

    private void Awake()
    {
        vC = gameObject.GetComponent<VirtualCurrency>();  // todo INJECT  
        gps = gameObject.GetComponent<GetPlayerStats>();  // todo INJECT  
        inventory = gameObject.GetComponent<Inventory>(); // todo INJECT  
    }

    private void Start()
    {
        userEmail.gameObject.SetActive(false);
    }

    // login function called by the button.
    public void Login()
    {
        loginRequest = new LoginWithPlayFabRequest();

        loginRequest.Username = userName.text;
        loginRequest.Password = userPassword.text;
        loginRequest.InfoRequestParameters = infoRequest;

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, result =>
        {
            // If the asccount is found.
            isAuthenticated = true;
            message.text = "Welcome " + userName.text + " ! Connecting...";

            //vC.FetchCurrency();
            // vC.PurchaseUpgrade("Upgrade", 50);
            
            if(gps.gotCurrency == false)
            {
                gps.FetchCurrency();
                inventory.GetCatalog();
            }
            

        }, error =>
        {
            //If the Account is not found.
            isAuthenticated = false;

            if (error.ErrorMessage == "User not found")
            {
                userEmail.gameObject.SetActive(true);
                message.text = "Failed to login. " + error.ErrorMessage + ".\nPlease enter your email to register.";
            }

        }, null);
    }

    public void Register()
    {
        registerRequest = new RegisterPlayFabUserRequest();

        registerRequest.Username = userName.text;
        registerRequest.Password = userPassword.text;
        registerRequest.Email = userEmail.text;

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, result => {


            message.text = "Your account has been created!";
        
        }, error => {

            userEmail.gameObject.SetActive(true);
            message.text = "Faild to create your account " + error.ErrorMessage;

        });
    }
}
