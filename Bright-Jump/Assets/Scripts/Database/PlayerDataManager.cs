using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class PlayerDataManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    public PlayerData PlayerData => _playerData;
    public int Name => _playerData.name;
    public int Score => _playerData.score;

    public PlayerSaveManager playerSaveManager = new PlayerSaveManager();

    private Coroutine _coroutine;

    private string dbUrl = "https://brightjump-brightest.firebaseio.com";
    DatabaseReference dbRoot;

    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbUrl);

        // Get the root reference location of the database.
        dbRoot = FirebaseDatabase.DefaultInstance.RootReference;

        if(_coroutine == null){
            _coroutine = StartCoroutine(LoadPlayer());
        }
        DontDestroyOnLoad(gameObject);
    }

    public void UpdatePlayer(PlayerData playerData){
        if(!playerData.Equals(_playerData)){
            if(playerData.score > _playerData.score){
                playerSaveManager.WriteScore(PlayerPrefs.GetString("uid"), _playerData.score, dbRoot);
            }
            _playerData = playerData;
        }
    }

    public long GetScorePlayer(){
        return playerSaveManager.GetScore(PlayerPrefs.GetString("uid"), dbRoot);
    }
    
    public IEnumerator LoadPlayer(){
        var saveExistsTask = playerSaveManager.SaveExists(dbRoot);
        yield return new WaitUntil(() => saveExistsTask.IsCompleted);
        if(saveExistsTask.Result){
            var playerDataLoadTask = playerSaveManager.LoadPlayer(dbRoot);
            yield return new WaitUntil(() => playerDataLoadTask.IsCompleted);
            var playerData = playerDataLoadTask.Result;
        
            if(playerData.HasValue){
                UpdatePlayer(playerData.Value);
            }
        }

        _coroutine = null;
    }
}
