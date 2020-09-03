using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpHeight = 10f;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            if(other.relativeVelocity.y <= 0){
                Rigidbody2D rig = other.collider.GetComponent<Rigidbody2D>();
                if(rig != null){
                    Vector2 velocity = rig.velocity;
                    velocity.y = jumpHeight;
                    rig.velocity = velocity;
                }
            }
        }
    }
}
