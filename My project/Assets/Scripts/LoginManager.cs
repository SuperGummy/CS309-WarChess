using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    private String _userNameInput;
    private String _passwordInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetUserNameInput(string input)
    {
        _userNameInput = input;
    }

    public void GetPasswordInput(string input)
    {
        _passwordInput = input;
    }

    public void OnClickLogin()
    {
        if (_userNameInput is null || _passwordInput is null)
            return;
        // TODO: Login within the server;
        bool loginSuccess = false;
        if (loginSuccess)
        {
            SceneManager.LoadSceneAsync("Start After Login");
        }
        else
        {
            // TODO: show login failed message;
        }
    }

    public void OnClickRegister()
    {
        if (_userNameInput is null || _passwordInput is null)
            return;
        // TODO: Register within the server;
        bool registerSuccess = false;
        if (registerSuccess)
        {
            // TODO: show register success message;
        }
        else
        {
            // TODO: show register failed message;
        }
    }
}
