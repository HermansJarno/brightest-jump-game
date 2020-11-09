using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;

public class AuthHandler : MonoBehaviour
{
    FirebaseApp app;
    Firebase.Auth.FirebaseAuth auth;

    Task<GoogleSignInUser> signIn;

    Task<GoogleSignInUser> signInSilent;

    public GameObject loginButton;
    public GameObject authUI;

    private void Start(){
        FixDependencies();

        //loginButton.GetComponent<Button>().interactable = false;
        
        /*
        signInSilent = GoogleSignIn.DefaultInstance.SignInSilently();
        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser> ();

        signInSilent.ContinueWith (task => {
            if (task.IsCanceled) {
                signInCompleted.SetCanceled ();
                loginButton.GetComponent<Button>().interactable = true;
            } else if (task.IsFaulted) {
                signInCompleted.SetException (task.Exception);
                loginButton.GetComponent<Button>().interactable = true;
            } else {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential (((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync (credential).ContinueWith (authTask => {
                    if (authTask.IsCanceled) {
                        signInCompleted.SetCanceled();
                    } else if (authTask.IsFaulted) {
                        signInCompleted.SetException(authTask.Exception);
                    } else {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);

                        FirebaseUser user = auth.CurrentUser;
                        if(user != null){
                            string name = user.DisplayName;
                            string email = user.Email;
                            System.Uri photo_url = user.PhotoUrl;
                            // The user's Id, unique to the Firebase project.
                            // Do NOT use this value to authenticate with your backend server, if you
                            // have one; use User.TokenAsync() instead.
                            string uid = user.UserId;
                            PlayerPrefs.SetString("uid", uid);
                            GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>().enabled = true;
                            loginButton.SetActive(false);
                            afterAuthUI.SetActive(true);
                        } 
                    }
                });
            }
        });*/
    }

    void FixDependencies (){
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void SignIn(){
        if(app == null){
            FixDependencies();
        }

        if(Application.isEditor){
            PlayerPrefs.SetString("uid", "9mr9iiRuiXU0WQ0AuY2ZSWD5SU93");
            GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>().enabled = true;
            loginButton.SetActive(false);
            authUI.SetActive(true);
        }else{
            if(Application.internetReachability == NetworkReachability.NotReachable) {
                loginButton.transform.Find("InternetConnection").gameObject.SetActive(true);
            } else {
                loginButton.transform.Find("InternetConnection").gameObject.SetActive(false);
                GoogleSignIn.Configuration = new GoogleSignInConfiguration {
                RequestIdToken = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "174605211674-o3okbtcfskpsa9jl5486irfldcnjr0ru.apps.googleusercontent.com"
                };

                signIn = GoogleSignIn.DefaultInstance.SignIn ();

                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser> ();
                signIn.ContinueWith (task => {
                    if (task.IsCanceled) {
                        signInCompleted.SetCanceled ();
                    } else if (task.IsFaulted) {
                        signInCompleted.SetException (task.Exception);
                    } else {
                        Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential (((Task<GoogleSignInUser>)task).Result.IdToken, null);
                        auth.SignInWithCredentialAsync (credential).ContinueWith (authTask => {
                            if (authTask.IsCanceled) {
                                signInCompleted.SetCanceled();
                            } else if (authTask.IsFaulted) {
                                signInCompleted.SetException(authTask.Exception);
                            } else {
                                signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);

                                FirebaseUser user = auth.CurrentUser;
                                if(user != null){
                                    string name = user.DisplayName;
                                    string email = user.Email;
                                    System.Uri photo_url = user.PhotoUrl;
                                    // The user's Id, unique to the Firebase project.
                                    // Do NOT use this value to authenticate with your backend server, if you
                                    // have one; use User.TokenAsync() instead.
                                    string uid = user.UserId;
                                    PlayerPrefs.SetString("uid", uid);
                                    if(!GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>().isActiveAndEnabled){
                                        GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>().enabled = true;
                                    }else{
                                        GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>().Initiate();
                                    }
                                    loginButton.SetActive(false);
                                    //authUI.SetActive(true);
                                } 
                            }
                        });
                    }
                });
            }
        }
    }

    public void SignOut() {
        if(auth != null){
            auth.SignOut();
        } 

        if(signIn != null){
            GoogleSignIn.DefaultInstance.SignOut();
        } 

        auth = null;
        signIn = null;
        app = null;

        loginButton.SetActive(true);
        authUI.SetActive(false);
    }
}
