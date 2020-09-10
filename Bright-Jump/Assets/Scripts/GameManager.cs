using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject prefabEndScene;

    [SerializeField]
    private long coinsCollected = 0;

    private DatabaseController databaseController;

    
    private void Start() {
        databaseController = GameObject.Find("DatabaseController").GetComponent<DatabaseController>();
        prefabEndScene.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    public void AddCoin(){
        coinsCollected++;
        GameObject.FindGameObjectWithTag("coinText").GetComponent<Text>().text = coinsCollected.ToString();
    }

    public void EndGame(Vector3 EndPosition){
        PlayerDataManager playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
        databaseController.UpdateLeaderBoardWithScore(playerDataManager.PlayerData.name, coinsCollected);
        StartCoroutine(HandleUI(playerDataManager, EndPosition));
    }

    private IEnumerator HandleUI(PlayerDataManager playerDataManager, Vector3 EndPosition){
        yield return new WaitUntil(() => databaseController.highscoresRetrieved);

        Text textFieldScore = prefabEndScene.transform.Find("ScoreText").GetComponent<Text>();
        Text textFieldHighscore = prefabEndScene.transform.Find("HighscoreText").GetComponent<Text>();

        bool newHighscore = false;
        if(databaseController.highscores.Count < databaseController.LeaderBoard.MaxScores){
            newHighscore = true;
        }else{
            foreach(LeaderBoardEntry highscore in databaseController.highscores){
                if(highscore.score < coinsCollected){
                    newHighscore = true;
                }
            }
        }

        long previousHighScore = playerDataManager.Score;
        prefabEndScene.transform.position = new Vector3(EndPosition.x, EndPosition.y, 5f);
        textFieldScore.text = "Score: " + coinsCollected.ToString();
        if(newHighscore){
            textFieldScore.text += "\nGlobal new highscore: " + coinsCollected.ToString();
            if(previousHighScore > coinsCollected){
                textFieldScore.text += "\nPersonal highscore: " + previousHighScore.ToString();
            }
        } else if (coinsCollected > previousHighScore) {
            textFieldScore.text += "\nPersonal new highscore: " + coinsCollected.ToString();
        } else if(previousHighScore > coinsCollected){
            textFieldScore.text += "\nPersonal highscore: " + previousHighScore.ToString();
        }

        int index = 1;
        foreach(LeaderBoardEntry highscore in databaseController.highscores){
            if(databaseController.highscores.IndexOf(highscore) == 0) textFieldHighscore.text = "Highscores:\n";
            textFieldHighscore.text += "\n" + index + ". " + highscore.name + " - score : " + highscore.score;
            index++;
        }

        if(coinsCollected > previousHighScore) playerDataManager.UpdatePlayer(new PlayerData(playerDataManager.PlayerData.name, coinsCollected));
    }
}
