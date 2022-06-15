using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    // Player Stats
    public string Username => _playerData.Username;
    public int Level => _playerData.Level;
    public int XP => _playerData.Xp;
    public int Gold => _playerData.Gold;
    public int Gem => _playerData.Gem;

    // Unity Events
    public UnityEvent OnPlayerUpdated = new UnityEvent();

    public void UpdatePlayer(PlayerData playerData)
    {
        if(!playerData.Equals(_playerData))
        {
            _playerData = playerData;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetGold(int gold)
    {
        if(gold != _playerData.Gold)
        {
            _playerData.Gold = gold;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetGem(int gem)
    {
        if(gem != _playerData.Gem)
        {
            _playerData.Gem = gem;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetXP(int xp)
    {
        if(xp != _playerData.Xp)
        {
            _playerData.Xp = xp;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetLevel(int level)
    {
        if(level != _playerData.Level)
        {
            _playerData.Level = level;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetUsername(string username)
    {
        if(username != _playerData.Username)
        {
            _playerData.Username = username;
            OnPlayerUpdated.Invoke();
        }
    }

    




}
