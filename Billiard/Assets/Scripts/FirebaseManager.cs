using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    //Firebase var
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //User Data variables
    [Header("UserData")]
    public TMP_InputField usernameField1;
    public TMP_InputField usernameField2;
    public TMP_InputField genderField;
    public TMP_InputField countryField;
    public TMP_InputField rankField;
    
    public string id= StaticToken.token;

    void Start() {
        StartCoroutine(LoadUserData());
    }
    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
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
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveDataButton()
    {
        StartCoroutine(UpdateUsernameDatabase(usernameField1.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField2.text));

        StartCoroutine(UpdateGenderDatabase(genderField.text));
        StartCoroutine(UpdateCountryDatabase(countryField.text));
        StartCoroutine(LoadUserData());
    }
    private IEnumerator LoadUserData()
    {
        rankField.text = "Rank: ?";
        //Get the currently logged in user data
        Task<DataSnapshot> DBTask = DBreference.Child("player").Child(id).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            Debug.Log("No data exists");
            usernameField1.text = "None";
            usernameField2.text = "None";
            genderField.text = "None";
            countryField.text = "None";
        }
        else
        {
            //Data has been retrieved
            Debug.Log("Data has been retrieved");
            DataSnapshot snapshot = DBTask.Result;
            usernameField1.text = snapshot.Child("name").Value.ToString();
            usernameField2.text = snapshot.Child("name").Value.ToString();
            genderField.text = snapshot.Child("gender").Value.ToString();
            countryField.text = snapshot.Child("country").Value.ToString();
        }
    }
    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        Task DBTask = DBreference.Child("player").Child(id).Child("name").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }
    private IEnumerator UpdateGenderDatabase(string _gender)
    {
        //Set the currently logged in user username in the database
        Task DBTask = DBreference.Child("player").Child(id).Child("gender").SetValueAsync(_gender);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database gender is now updated
        }
    }
    private IEnumerator UpdateCountryDatabase(string _country)
    {
        //Set the currently logged in user username in the database
        Task DBTask = DBreference.Child("player").Child(id).Child("country").SetValueAsync(_country);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database country is now updated
        }
    }
    public void SignOutButton()
    {
        auth.SignOut();
        SceneManager.LoadScene(1);
    }

    public void CheckToken() {
        Debug.Log(id);
    }
}
