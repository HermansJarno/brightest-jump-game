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
    public async Task<PlayerData?> LoadPlayer(DatabaseReference databaseReference){

        var dataSnapshot = await databaseReference.Child("users").Database.GetReference(PlayerPrefs.GetString("uid")).GetValueAsync();
        if(!dataSnapshot.Exists){
            return null;
        }

        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public void writeOrUpdateUser(string userId, string name, long score, DatabaseReference dbReference) {
        User user = new User(name, score);
        string json = JsonUtility.ToJson(user);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void WriteScore(string userId, long score, DatabaseReference dbReference){
        dbReference.Child("users").Child(userId).Child("score").SetValueAsync(score);
    }

    public long GetScore(string userId, DatabaseReference dbReference){
        long score = 0;
        dbReference.Child("users").Child(userId).Child("score").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            DataSnapshot snapshot = task.Result;
                score = (long)snapshot.Value;
            }
        });
        return score;
    }
    
    public async Task<bool> SaveExists(DatabaseReference databaseReference) {
        bool saveExists = false;
        

        await databaseReference.Child("users").Database.GetReference(PlayerPrefs.GetString("uid")).GetValueAsync().ContinueWith(query => {
            saveExists = query.Result.Exists;
            Debug.Log($"Save exists : {saveExists}");

            if(!saveExists){
                string name = "";
                if(PlayerPrefs.GetString("playerName") != null) name = PlayerPrefs.GetString("playerName");
                writeOrUpdateUser(PlayerPrefs.GetString("uid"), name, 0, databaseReference);
            }
        });
        return saveExists;
    }
}
