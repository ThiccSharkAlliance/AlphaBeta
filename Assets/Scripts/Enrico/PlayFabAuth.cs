using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabAuth : MonoBehaviour
{
    // Ref to login input fields.
    public InputField userName;
    public InputField userPassword;
    public InputField userEmail;

    //Message to dispaly on login panel.
    public Text message;
    
    public bool isAuthenticated = false;

    LoginWithPlayFabRequest loginRequest;
    RegisterPlayFabUserRequest registerRequest;


    private void Start()
    {
        userEmail.gameObject.SetActive(false);

    }

    // login function calle by the button.
    public void Login()
    {
        loginRequest = new LoginWithPlayFabRequest();

        loginRequest.Username = userName.text;
        loginRequest.Password = userPassword.text;

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, result => {
            // If the asccount is found.
            isAuthenticated = true;


            message.text = "Welcome " + userName.text + " ! Connecting...";

        }, error => {
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
