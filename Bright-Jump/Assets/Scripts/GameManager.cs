using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject prefabEndScene;

    [SerializeField]
    private int coinsCollected = 0;

    public void AddCoin(){
        coinsCollected++;
        GameObject.FindGameObjectWithTag("coinText").GetComponent<Text>().text = coinsCollected.ToString();
    }

    public void EndGame(Vector3 EndPosition){
        prefabEndScene.transform.position = new Vector3(EndPosition.x, EndPosition.y, 5f);
        prefabEndScene.transform.Find("ScoreText").GetComponent<Text>().text = coinsCollected.ToString();
    }
}
