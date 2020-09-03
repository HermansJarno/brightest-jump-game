using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 1f;

    private void FixedUpdate() {
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime < 0){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "platform"){
            Destroy(gameObject);
        }else if(other.gameObject.tag == "bug"){
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
