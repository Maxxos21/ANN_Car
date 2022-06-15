using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerToSave : MonoBehaviour
{

    [SerializeField] private PlayerSaveManager _playerSaveManager;
    [SerializeField] private PlayerBehaviour _player;

    private void Reset()
    {
        _playerSaveManager = FindObjectOfType<PlayerSaveManager>();
    }

    private IEnumerator Start()
    {
        var playerDataTask = _playerSaveManager.LoadPlayer();
        yield return new WaitUntil(() => playerDataTask.IsCompleted);
        var playerData = playerDataTask.Result;
        
        if (playerData.HasValue)
        {
            _player.UpdatePlayer(playerData.Value);
        }

        _player.OnPlayerUpdated.AddListener(HandlePlayerUpdated);
    }

    private void HandlePlayerUpdated()
    {
        _playerSaveManager.SavePlayer(_player.PlayerData);
    }
}
