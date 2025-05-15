using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleSignInManager : MonoBehaviour
{
    public string webClientId;
    private FirebaseAuth auth;
    private FirebaseUser user;
    public string userId;
    private GoogleSignInConfiguration configuration;

    public static GoogleSignInManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized");
            }
            else
            {
                Debug.LogError("Firebase dependency error: " + status);
            }
        });
    }

    public void SignInWithGoogle()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Google Sign-In failed.");
                return;
            }

            var googleUser = task.Result;
            Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(authTask =>
            {
                if (authTask.IsCanceled || authTask.IsFaulted)
                {
                    Debug.LogError("Firebase auth failed.");
                    return;
                }

                user = authTask.Result;
                Debug.Log("Signed in as: " + user.DisplayName);
                SceneManager.LoadScene("Gameplay");
            });
        });
#else
        Debug.Log("Simulating Google Sign-In (Editor Mode)");

        string fakeDisplayName = "Test User";
        string fakeEmail = "testuser@example.com";

        Debug.Log($"Signed in as: {fakeDisplayName} ({fakeEmail})");

        OnFakeSignIn(fakeDisplayName, fakeEmail);
#endif
    }

#if UNITY_EDITOR
    private void OnFakeSignIn(string name, string email)
    {
        Debug.Log("Fake sign-in successful.");
        SceneManager.LoadScene("Gameplay");
    }
#endif
}
