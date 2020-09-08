using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject prefabEndScene;

    [SerializeField]
    private int coinsCollected = 0;

    private DatabaseController databaseController;
    
    private void Start() {
        databaseController = GameObject.Find("DatabaseController").GetComponent<DatabaseController>();
    }

    public void AddCoin(){
        coinsCollected++;
        GameObject.FindGameObjectWithTag("coinText").GetComponent<Text>().text = coinsCollected.ToString();
    }

    public void EndGame(Vector3 EndPosition){
        PlayerDataManager playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();

        long scoreDatabase = playerDataManager.GetScorePlayer();
        prefabEndScene.transform.position = new Vector3(EndPosition.x, EndPosition.y, 5f);
        prefabEndScene.transform.Find("ScoreText").GetComponent<Text>().text = "Score: " + coinsCollected.ToString();
        if(scoreDatabase != 0) prefabEndScene.transform.Find("ScoreText").GetComponent<Text>().text += "\nHighScore: " + scoreDatabase.ToString();
        databaseController.UpdateLeaderBoardWithScore(PlayerPrefs.GetString("PlayerName"), coinsCollected);
    }
}
