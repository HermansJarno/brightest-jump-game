using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

public class PlayerSaveManager 
{
    public PlayerSaveManager(string dbUrl){
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbUrl);
    }

    public async Task<PlayerData?> LoadPlayer(DatabaseReference databaseReference){
        var dataSnapshot = await databaseReference.Child("users").Child(PlayerPrefs.GetString("uid")).GetValueAsync();
        if(!dataSnapshot.Exists){
            return null;
        }

        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public void writeOrUpdateUser(string userId, string name, long score, DatabaseReference dbReference) {
        PlayerData playerData = new PlayerData(name, score);
        string json = JsonUtility.ToJson(playerData);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        OverWriteNameInHighScores(userId, name, dbReference);
    }

    public void WriteScore(string userId, long score, DatabaseReference dbReference){
        dbReference.Child("users").Child(userId).Child("score").SetValueAsync(score);
    }

    public void WriteName(string userId, string name, DatabaseReference dbReference){
        dbReference.Child("users").Child(userId).Child("name").SetValueAsync(name);
        OverWriteNameInHighScores(userId, name, dbReference);
    }

    public async Task<bool> SaveExists(DatabaseReference databaseReference) {
        bool saveExists = false;

        await databaseReference.Child("users").Child(PlayerPrefs.GetString("uid")).GetValueAsync().ContinueWith(query => {
            saveExists = query.Result.Exists;

            if(!saveExists){
                writeOrUpdateUser(PlayerPrefs.GetString("uid"), "", 0, databaseReference);
            }
        });
        return saveExists;
    }

    public void OverWriteNameInHighScores(string uid, string name, DatabaseReference dbReference){
        dbReference.Child("highscores").RunTransaction(mutableData => {
            List<object> highscores = mutableData.Value as List<object>;
            List<object> tempHighscores = new List<object>();

            if (highscores == null) {
                // abort
                Debug.Log("err");
            } else {
                //object highscore = null;
                foreach (var child in highscores) {
                    if (!(child is Dictionary<string, object>)) continue;
                    string childUid = (string)((Dictionary<string, object>)child)["uid"];
                    if (childUid == uid){
                        Dictionary<string, object> highscore = (Dictionary<string, object>)child;
                        highscore["name"] = name;
                        Debug.Log(highscore["name"]);
                        Debug.Log(highscore["score"]);
                        Debug.Log(highscore["uid"]);


                        //highscores.Remove(child);
                        //tempHighscores.Add(highscore);
                        tempHighscores.Add(highscore);
                    }else{
                        tempHighscores.Add(child);
                    }
                }
            }

            mutableData.Value = tempHighscores;
            return TransactionResult.Success(mutableData);
            //return TransactionResult.Abort();

        });
    }
}
