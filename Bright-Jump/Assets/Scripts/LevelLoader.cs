using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void Quit(){
        Application.Quit();
    }
}
