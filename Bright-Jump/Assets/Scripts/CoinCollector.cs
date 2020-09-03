using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "coin"){
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().AddCoin();
        }
    }
}
