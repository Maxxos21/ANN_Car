using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{

    [Header("Firebase")]
    public Firebase.DependencyStatus dependencyStatus;
    public Firebase.Auth.FirebaseAuth auth; 
    public FirebaseDatabase firebaseDatabase;  

    [Header("Registration")]
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField username;
    public TMP_Text errorText;

    [Header("Login")]
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    public TMP_Text warningLoginText;

    [Header("Player Information")]
    public int experience = 0;

    //Singleton
    public static LoginManager instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        firebaseDatabase = new FirebaseDatabase();
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(email.text, password.text, username.text));
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLogin.text, passwordLogin.text));
    }

    private IEnumerator Register(string email, string password, string username)
    {
        if (username == "")
        {
            errorText.text = "Missing Username";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            
            if (RegisterTask.Exception != null)
            {
                Firebase.FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as Firebase.FirebaseException;
                Firebase.Auth.AuthError errorCode = (Firebase.Auth.AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case Firebase.Auth.AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case Firebase.Auth.AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case Firebase.Auth.AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case Firebase.Auth.AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                errorText.text = message;
            }
            else
            {   
                // User is now registered
                Firebase.Auth.FirebaseUser user = RegisterTask.Result;

                if (user != null)
                {
                    Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                    {
                        DisplayName = username
                    };

                    Debug.Log(profile.DisplayName);
                    var ProfileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        Firebase.FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as Firebase.FirebaseException;
                        Firebase.Auth.AuthError errorCode = (Firebase.Auth.AuthError)firebaseEx.ErrorCode;
                        errorText.text = "Username Set Failed!";
                    }
                    else
                    {
                        UIManager.instance.ChangeUI(12);
                    }
                }
            }
        }
    }
    
    private IEnumerator Login(string email, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            Firebase.FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as Firebase.FirebaseException;
            Firebase.Auth.AuthError errorCode = (Firebase.Auth.AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case Firebase.Auth.AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case Firebase.Auth.AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case Firebase.Auth.AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case Firebase.Auth.AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case Firebase.Auth.AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            Firebase.Auth.FirebaseUser user = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            warningLoginText.text = "";
            UIManager.instance.ChangeUI(0);
        }
    }

    public void ResetPasswordButton()
    {
        StartCoroutine(ResetPassword(emailLogin.text));
    }

    private IEnumerator ResetPassword(string email)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                warningLoginText.text = "Enter valid email to reset password";
                return;
            }
            if (task.IsFaulted)
            {
                warningLoginText.text = "Enter valid email to reset password";
                return;
            }
            else
            {
                warningLoginText.text = "Password reset email sent";
            }
        });
        yield return null;
    }
}

