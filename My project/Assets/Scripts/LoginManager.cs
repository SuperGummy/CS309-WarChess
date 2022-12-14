using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using api = API.Service;
using UnityEditor;
using Model;
using Newtonsoft.Json;

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

    public async void OnClickLogin()
    {
        if (_userNameInput is null || _passwordInput is null)
            return;
        var res = await api.POST(
            url: api.Login,
            param: new Dictionary<string, string>
            {
                { "username", _userNameInput },
                { "password", _passwordInput }
            });

        if (res == null)
        {
            EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            return;
        }

        var token = res.Content.ReadAsStringAsync().Result;

        if (token == null)
        {
            EditorUtility.DisplayDialog("Errors", "Login failed", "ok");
            return;
        }

        EditorUtility.DisplayDialog("Congratulations", "Login successfully", "ok");
        PlayerPrefs.SetString("username", _userNameInput);
        PlayerPrefs.SetString("token", token);
        SceneManager.LoadSceneAsync("Start After Login");
    }

    public async void OnClickRegister()
    {
        if (_userNameInput is null || _passwordInput is null)
            return;
        var res = await api.POST(
            url: api.Register,
            param: new Dictionary<string, string>
            {
                { "username", _userNameInput },
                { "password", _passwordInput }
            });

        if (res == null)
        {
            EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            return;
        }

        var content = res.Content.ReadAsStringAsync().Result;
        var model = JsonConvert.DeserializeObject<Model<Account>>(content);

        if (model == null)
        {
            EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (model.code != 200)
        {
            EditorUtility.DisplayDialog("Bad Request", model.msg, "ok");
            return;
        }

        var account = model.data;
        if (account == null)
        {
            EditorUtility.DisplayDialog("Errors", "Register failed", "ok");
        }
        else
        {
            EditorUtility.DisplayDialog("Congratulations", "Register successfully", "ok");
        }
    }
}