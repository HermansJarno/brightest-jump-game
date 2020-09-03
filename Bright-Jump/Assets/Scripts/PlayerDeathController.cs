using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    private Transform trashbin;

    Vector3 endPositionOnDeath;
    Vector3 world;

    private void Start() {
        world = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        trashbin = GameObject.FindGameObjectWithTag("trashbin").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(trashbin.position.y > transform.position.y){
            EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "bug"){
            gameObject.GetComponent<PlayerController>().StopMoving();
        }
    }

    private void EndGame(){
        endPositionOnDeath = new Vector3(0, Camera.main.transform.position.y - (world.y*2), 0);
        Camera.main.gameObject.GetComponent<CameraController>().StartEndScene(endPositionOnDeath);
        GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().EndGame(endPositionOnDeath);
        Destroy(gameObject);
    }
}
