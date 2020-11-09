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
    public string Name => _playerData.name;
    public long Score => _playerData.score;

    private PlayerSaveManager playerSaveManager;

    public GameObject afterAuthUI;
    private string dbUrl = "https://brightjump-brightest.firebaseio.com";

    private Coroutine _coroutine;

    private bool initialLoad = true;

    DatabaseReference dbRoot;    

    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbUrl);
        playerSaveManager = new PlayerSaveManager(dbUrl);

        // Get the root reference location of the database.
        dbRoot = FirebaseDatabase.DefaultInstance.RootReference;

        if(_coroutine == null){
            _coroutine = StartCoroutine(LoadPlayer());
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Initiate(){
        if(_coroutine == null){
            _coroutine = StartCoroutine(LoadPlayer());
        }
    }

    public void UpdatePlayer(PlayerData playerData){
        if(!playerData.Equals(_playerData) && !initialLoad){
            if(playerData.score > _playerData.score && playerData.name != _playerData.name){
                playerSaveManager.writeOrUpdateUser(PlayerPrefs.GetString("uid"), playerData.name, playerData.score, dbRoot);
            }
            else if(playerData.score > _playerData.score){
                playerSaveManager.WriteScore(PlayerPrefs.GetString("uid"), playerData.score, dbRoot);
            }
            else if(playerData.name != _playerData.name){
                playerSaveManager.WriteName(PlayerPrefs.GetString("uid"), playerData.name, dbRoot);
            }
        }

        _playerData = playerData;

        if(!afterAuthUI.activeSelf){
            afterAuthUI.SetActive(true);
        }
        
        if(afterAuthUI != null){
            if(playerData.name != "") afterAuthUI.GetComponent<PlayerDataUI>().SetText(playerData.name);
            initialLoad = false;
        }
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
