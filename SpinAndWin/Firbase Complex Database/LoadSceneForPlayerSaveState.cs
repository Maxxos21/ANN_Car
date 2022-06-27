using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneForPlayerSaveState : MonoBehaviour
{

    [SerializeField] private PlayerSaveManager _playerSaveManager;
    [SerializeField] private string _sceneForSaveExists;
    [SerializeField] private string _sceneForNoSave;


    private Coroutine _coroutine;

    public void Trigger ()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator LoadSceneCoroutine()
    {
        var saveExistsTask = _playerSaveManager.SaveExists();
        yield return new WaitUntil(() => saveExistsTask.IsCompleted);
        if (saveExistsTask.Result)
        {
            SceneManager.LoadScene(_sceneForSaveExists);
        }
        else
        {
            SceneManager.LoadScene(_sceneForNoSave);
        }

        _coroutine = null;
    }
}
