using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    PlayerDataManager playerDataManager;

    public InputField inputField;
    private void Start() {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
    }

    public void SetText(string text){
        inputField.text = text;
    }

    public void UpdateText(string input){
        playerDataManager.UpdatePlayer(new PlayerData(input, playerDataManager.PlayerData.score));
    }
}
