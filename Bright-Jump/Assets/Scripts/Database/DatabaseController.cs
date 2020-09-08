using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseController : MonoBehaviour
{

    private string dbUrl = "https://brightjump-brightest.firebaseio.com";
    DatabaseReference dbRoot;
    LeaderBoard leaderBoard = new LeaderBoard();
    
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbUrl);

        // Get the root reference location of the database.
        dbRoot = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void UpdateLeaderBoardWithScore(string name, long score){
        leaderBoard.AddScoreToLeaders(name, score, dbRoot.Child("highscores"));
    }

    void GetScoreBoard(){

    }
}
