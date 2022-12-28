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
using TMPro;

public class LoginManager : MonoBehaviour
{
    private String _userNameInput;

    private String _passwordInput;
    [SerializeField] private TextMeshProUGUI message;

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
            message.text = "Network errors, please check your network!";
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            message.text = res.ReasonPhrase;
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            return;
        }

        var token = res.Content.ReadAsStringAsync().Result;

        if (token == null)
        {
            message.text = "Login failed, please check username and password.";
            message.color = Color.red;
            EditorUtility.DisplayDialog("Errors", "Login failed", "ok");
            return;
        }
        //EditorUtility.DisplayDialog("Congratulations", "Login successfully", "ok");
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
            message.text = "Network errors, please check your network!";
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            message.text = res.ReasonPhrase;
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            return;
        }

        var content = res.Content.ReadAsStringAsync().Result;
        var model = JsonConvert.DeserializeObject<Model<Account>>(content);

        if (model == null)
        {
            message.text = "Network errors, please check your network!";
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (model.code != 200)
        {
            message.text = model.msg;
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Bad Request", model.msg, "ok");
            return;
        }

        var account = model.data;
        if (account == null)
        {
            message.text = "Register failed, please check username and password: Password length is 6-16.";
            message.color = Color.red;
            //EditorUtility.DisplayDialog("Errors", "Register failed", "ok");
        }
        else
        {
            message.text = "Register successful!";
            message.color = Color.green;
            //EditorUtility.DisplayDialog("Congratulations", "Register successfully", "ok");
        }
    }
}