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
using System.Linq.Expressions;
using Firebase;
using System.Collections;
public class LoginAndRegisterManager : MonoBehaviour
{
    /// <summary>
    /// TMP Input Field for login email.
    /// </summary>
    [SerializeField]
    TMP_InputField loginEmailField;
    /// <summary>
    /// TMP Input Field for login password.
    /// </summary>
    [SerializeField]
    TMP_InputField loginPasswordField;
    /// <summary>
    /// TMP Input Field for registration email.
    /// </summary>
    [SerializeField]
    TMP_InputField registerEmailField;
    /// <summary>
    /// TMP Input Field for registration password.
    /// </summary>
    [SerializeField]
    TMP_InputField registerPasswordField;
    /// <summary>
    /// TMP Input Field for username during registration.
    /// </summary>
    [SerializeField]
    TMP_InputField usernameField;
    /// <summary>
    /// TMP Text to display login errors.
    /// </summary>
    [SerializeField]
    TMP_Text loginErrorText;
    /// <summary>
    /// TMP Text to display registration errors.
    /// </summary>
    [SerializeField]
    TMP_Text registerErrorText;
    /// <summary>
    /// GameObject for the login panel.
    /// </summary>
    [SerializeField]
    GameObject loginPanel;
    /// <summary>
    /// GameObject for the registration panel.
    /// </summary>
    [SerializeField]
    GameObject registerPanel;

    /// <summary>
    /// Initializes the login and registration panels.
    /// </summary>
    void Start()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        ClearLoginFields();
        ClearRegisterFields();
    }
    /// <summary>
    /// Clears all input fields and error messages.
    /// </summary>
    public void ClearLoginFields()
    {
        loginEmailField.text = "";
        loginPasswordField.text = "";
        loginErrorText.text = "";
    }
    /// <summary>
    /// Clears all registration input fields and error messages.
    /// </summary>
    public void ClearRegisterFields()
    {
        registerEmailField.text = "";
        registerPasswordField.text = "";
        usernameField.text = "";
        registerErrorText.text = "";
    }
    /// <summary>
    /// Switches between the login and registration panels. 
    /// </summary>
    public void SwitchPanels()
    {
        bool isLoginActive = loginPanel.activeSelf;
        ClearLoginFields();
        ClearRegisterFields();
        loginPanel.SetActive(!isLoginActive);
        registerPanel.SetActive(isLoginActive);
    }
    /// <summary>
    /// Displays registration error messages.
    /// </summary>
    /// <param name="message"></param>
    public void ShowRegisterError(string message)
    {
        registerErrorText.text = message;
    }
    /// <summary>
    /// Displays login error messages.
    /// </summary>
    /// <param name="message"></param>

    public void ShowLoginError(string message)
    {
        loginErrorText.text = message;
    }
    /// <summary>
    /// Handles user login process.
    /// </summary>
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
                    var error = task.Exception.GetBaseException() as FirebaseException;
                    var errorCode = (AuthError)error.ErrorCode;
                    switch (errorCode)
                    {
                        case AuthError.InvalidEmail:
                            ShowLoginError("Invalid email address");
                            break;
                        case AuthError.WrongPassword:
                            ShowLoginError("Incorrect password");
                            break;
                        case AuthError.UserNotFound:
                            ShowLoginError("No account found with this email");
                            break;
                        default:
                            ShowLoginError(errorCode.ToString());
                            break;
                    }
                    if (task.Exception != null) Debug.Log(task.Exception);
                    
                    return;
                }
                Debug.Log("Login successful");
                GameManager.Instance.currentPlayerID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                DatabaseManager.Instance.RetrieveUser(GameManager.Instance.currentPlayerID);
                TransitionManager.Instance.ChangeScene("MenuScene");
            });
    }
    /// <summary>
    /// Developer login for testing purposes.
    /// </summary>
    public void DevLogin()
    {
        var email = loginEmailField;
        var password = loginPasswordField;
        email.text="test@gmail.com";
        password.text="testing";
        Login();
    }
    /// <summary>
    /// Handles user registration process.
    /// </summary>
    public void Signup()
    {
        // Obtain text from input fields
        var email = registerEmailField.text;
        var password = registerPasswordField.text;
        var displayName = usernameField.text;

        
        if (!email.Contains("@") || !email.Contains("."))
        {
            ShowRegisterError("Empty or invalid e-mail address");
            return;
        }
        
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
            ShowRegisterError(""); 
        }

        FirebaseAuth
            .DefaultInstance
            .CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    var error = task.Exception.GetBaseException() as FirebaseException;
                    var errorCode = (AuthError)error.ErrorCode;
                    switch (errorCode)
                    {
                        case AuthError.EmailAlreadyInUse:
                            ShowRegisterError("Email already in use");
                            break;
                        case AuthError.InvalidEmail:
                            ShowRegisterError("Invalid email address");
                            break;
                        case AuthError.WeakPassword:
                            ShowRegisterError("Weak password");
                            break;
                        default:
                            ShowRegisterError(errorCode.ToString());
                            break;
                    }
                    if (task.Exception != null) Debug.Log(task.Exception);
                    return;
                }
                GameManager.Instance.currentPlayerID = task.Result.User.UserId;
                DatabaseManager.Instance.CreateUser(GameManager.Instance.currentPlayerID, displayName);
                SwitchPanels();
            });
    }
    
}