using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;

public class DatabaseController : MonoBehaviour
{

    private string dbUrl = "https://brightjump-brightest.firebaseio.com";
    DatabaseReference dbRoot;
    LeaderBoard leaderBoard = new LeaderBoard();

    public List<LeaderBoardEntry> highscores;

    private Coroutine _coroutine;

    public bool highscoresRetrieved = false;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbUrl);

        // Get the root reference location of the database.
        dbRoot = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void UpdateLeaderBoardWithScore(string name, long score){
        highscoresRetrieved = false;
        leaderBoard.AddScoreToLeaders(PlayerPrefs.GetString("uid"), name, score, dbRoot);
        if(_coroutine == null){
            _coroutine = StartCoroutine(GetHighScores());
        }
    }

    public IEnumerator GetHighScores(){
        yield return new WaitUntil(() => !leaderBoard.writingHighscores);
        var highscoresTask = leaderBoard.GetScores(dbRoot);
        yield return new WaitUntil(() => highscoresTask.IsCompleted);

        Debug.Log(highscoresTask.Result.GetRawJsonValue());

        if(highscoresTask != null){
            LeaderBoardEntries tempBoard = JsonUtility.FromJson<LeaderBoardEntries>("{\"leaderBoardEntries\":" + highscoresTask.Result.GetRawJsonValue() + "}");
            highscores = tempBoard.leaderBoardEntries.OrderByDescending(obj => obj.score).ToList();

            highscoresRetrieved = true;
        }

        _coroutine = null;
    }

    public LeaderBoard LeaderBoard{
        get { return leaderBoard; }
    }
}
