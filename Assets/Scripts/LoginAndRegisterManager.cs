/*
* File Name: LoginAndRegisterManager.cs
* Author: Lim En Xu Jayson
* Date Created: 21/01/2026
* Description: Manager to handle login and registration processes.
*/
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
public class LoginAndRegisterManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField loginEmailField;
    [SerializeField]
    TMP_InputField loginPasswordField;
    [SerializeField]
    TMP_InputField registerEmailField;
    [SerializeField]
    TMP_InputField registerPasswordField;
    [SerializeField]
    TMP_InputField usernameField;
    [SerializeField]
    TMP_Text loginErrorText;
    [SerializeField]
    TMP_Text registerErrorText;
    [SerializeField]
    GameObject loginPanel;
    [SerializeField]
    GameObject registerPanel;
    void Start()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        ClearLoginFields();
        ClearRegisterFields();
    }
    public void ClearLoginFields()
    {
        loginEmailField.text = "";
        loginPasswordField.text = "";
        loginErrorText.text = "";
    }
    public void ClearRegisterFields()
    {
        registerEmailField.text = "";
        registerPasswordField.text = "";
        usernameField.text = "";
        registerErrorText.text = "";
    }
    public void SwitchPanels()
    {
        bool isLoginActive = loginPanel.activeSelf;
        ClearLoginFields();
        ClearRegisterFields();
        loginPanel.SetActive(!isLoginActive);
        registerPanel.SetActive(isLoginActive);
    }
    public void ShowRegisterError(string message)
    {
        registerErrorText.text = message;
    }

    public void ShowLoginError(string message)
    {
        loginErrorText.text = message;
    }
    public void Login()
    {
        var email = loginEmailField.text;
        var password = loginPasswordField.text;

        
        if (!email.Contains("@") || !email.Contains("."))
        {
            ShowLoginError("Empty or invalid e-mail address");
            return;
        }
        if (password == "")
        {
            ShowLoginError("Password cannot be empty");
            return;
        }
        
        
        else
        {
            ShowLoginError(""); 
        }

        FirebaseAuth
            .DefaultInstance
            .SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                Debug.Log("Login started");
                if (task.IsCanceled || task.IsFaulted)
                {
                    if (task.Exception != null) Debug.Log(task.Exception);
                    ShowLoginError("Error logging in");
                    return;
                }
                Debug.Log("Login successful");
                GameManager.Instance.currentPlayerID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                TransitionManager.Instance.ChangeScene("GameScene");
            });
    }
    public void Signup()
    {
        // Obtain text from input fields
        var email = registerEmailField.text;
        var password = registerPasswordField.text;
        var displayName = usernameField.text;

        // Input validation
        if (!email.Contains("@") || !email.Contains("."))
        {
            ShowRegisterError("Empty or invalid e-mail address");
            return;
        }
        // TODO: More validations
        if (password == "")
        {
            ShowRegisterError("Password cannot be empty");
            return;
        }
        if (password.Length < 6)
        {
            ShowRegisterError("Password must be at least 6 characters long");
            return;
        }
        else
        {
            ShowRegisterError(""); // Clear error
        }

        FirebaseAuth
            .DefaultInstance
            .CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ShowRegisterError("Error signing up");
                    if (task.Exception != null) Debug.Log(task.Exception);
                    return;
                }
                GameManager.Instance.currentPlayerID = task.Result.User.UserId;
                SwitchPanels();
            });
    }
}