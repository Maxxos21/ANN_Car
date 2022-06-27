using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DataManager : MonoBehaviour
{
    [Header("Firebase")]
    private Firebase.Database.DatabaseReference dbReference;

    [Header("User Constructor")]
    private string userID;
    [SerializeField] private int initLevel;
    [SerializeField] private int initXP;
    [SerializeField] private int initGold;
    [SerializeField] private int initGem;


    [Header("User Data")]
    public TMP_Text[] GoldText;
    public TMP_Text[] GemText;



    // Start is called before the first frame update
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {
        User newUser = new User(initLevel, initXP, initGold, initGem);
        string json = JsonUtility.ToJson(newUser);
        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }


    public IEnumerator GetGold(Action<int> onCallback)
    {
        var goldData = dbReference.Child("users").Child(userID).Child("gold").GetValueAsync();
        yield return new WaitUntil(predicate: () => goldData.IsCompleted);

        if (goldData != null)
        {
            Firebase.Database.DataSnapshot goldSnapshot = goldData.Result;
            onCallback.Invoke(int.Parse(goldSnapshot.Value.ToString()));
        }
    }

    public IEnumerator GetGem(Action<int> onCallback)
    {
        var gemData = dbReference.Child("users").Child(userID).Child("gem").GetValueAsync();
        yield return new WaitUntil(predicate: () => gemData.IsCompleted);

        if (gemData != null)
        {
            Firebase.Database.DataSnapshot gemSnapshot = gemData.Result;
            onCallback.Invoke(int.Parse(gemSnapshot.Value.ToString()));
        }
    }

    public void GetUserData()
    {
        StartCoroutine(GetGold((int gold) =>
        {
            for (int i = 0; i < GoldText.Length; i++)
            {
                GoldText[i].text = gold.ToString("#,#");
            }
        }));

        StartCoroutine(GetGem((int gem) =>
        {
            for (int i = 0; i < GemText.Length; i++)
            {
                GemText[i].text = gem.ToString("#,#");
            }
        }));
    }

    public void UpdateUser() 
    {
        dbReference.Child("users").Child(userID).Child("gold").SetValueAsync(initGold);
        dbReference.Child("users").Child(userID).Child("gem").SetValueAsync(initGem);
    }
}
