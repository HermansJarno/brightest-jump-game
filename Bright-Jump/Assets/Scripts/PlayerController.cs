using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject prefabBullet;
    public float jumpHeight = 10f;
    public float shootForce = 500f;
    public float moveSpeedModifier = 0.5f;
	Rigidbody2D rig;
	float dirX;
    bool moveAllowed = false;
    bool facingLeft = false;
    bool dead = false;

    GameObject instance;

    void Start()
    {
        rig = GetComponent<Rigidbody2D> ();

                    Vector3 wrld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));

        Debug.Log(wrld.x);
    }

    public void StopMoving(){
        moveAllowed = false;
        dead = true;
    }

    private void FixedUpdate() 
    {
        if (moveAllowed)
        {
            // Inverse facing if needed
            if(Input.acceleration.x < 0 && !facingLeft){
                Inverse();
                facingLeft = true;
            } else if(Input.acceleration.x > 0 && facingLeft){
                Inverse();
                facingLeft = false;
            }

            // Moving
            dirX = Input.acceleration.x * moveSpeedModifier;
		    rig.velocity = new Vector2 (rig.velocity.x + dirX, rig.velocity.y);

            //Teleport to other side of screen if needed
            Vector3 wrld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));

            if(transform.position.x > wrld.x){
                transform.position = new Vector3(-(wrld.x), transform.position.y, transform.position.z);
            } else if(transform.position.x < -(wrld.x)){
                transform.position = new Vector3(wrld.x , transform.position.y, transform.position.z);
            }

            //Shooting
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
                GameObject instance = Instantiate(prefabBullet, shootPoint.position, prefabBullet.transform.rotation) as GameObject;
                instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, shootForce));
                Physics2D.IgnoreCollision(instance.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            }
        } else {
            if(Input.touchCount > 0 && !dead){
                moveAllowed = true;
                rig.gravityScale = 1f;
            }else if(dead){
                Die();
                dead = false; // we only need to die once
            }
        }
    }

    private void Inverse(){
        rig.transform.localScale = new Vector3(rig.transform.localScale.x * -1, rig.transform.localScale.y, rig.transform.localScale.z);
        transform.rotation = Quaternion.Inverse(transform.rotation);
    }

    private void Die(){
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rig.transform.localScale = new Vector3(rig.transform.localScale.x, rig.transform.localScale.y * -1, rig.transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "platform"){
            if(rig.velocity.y <= 0){
                if(rig != null){
                    Vector2 velocity = rig.velocity;
                    velocity.y = jumpHeight;
                    rig.velocity = velocity;
                }
            }
        }
    }
}
