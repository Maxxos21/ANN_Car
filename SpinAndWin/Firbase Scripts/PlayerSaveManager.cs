using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;



public class PlayerSaveManager : MonoBehaviour
{
    private const string PLAYER_KEY = "Player";
    private Firebase.Database.DatabaseReference _database;

    private void Start()
    {
        _database = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SavePlayer(PlayerData player)
    {
        PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(player));
        _database.Child(PLAYER_KEY).SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public async Task<PlayerData?> LoadPlayer()
    {
        var dataSnapshot = await _database.Child(PLAYER_KEY).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> SaveExists()
    {
        var dataSnapshot = await _database.Child(PLAYER_KEY).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void DeleteSave()
    {
        _database.Child(PLAYER_KEY).RemoveValueAsync();
    }


}
