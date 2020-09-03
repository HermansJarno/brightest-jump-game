using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Transform barrier;
    public Vector2 followOffset;
    public float speed = 3f;
    public float endSceneSpeed = 15f;
    private Vector2 threshold;
    private Rigidbody2D rb;
    private Vector3 endPositionOnDeath;
    private bool startEndScene = false;

    // Start is called before the first frame update
    void Start(){
        threshold = calculateThreshold();
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(player != null && !startEndScene){
            if(barrier.position.y < player.transform.position.y){
                Vector2 follow = player.transform.position;
                float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

                Vector3 newPosition = transform.position;

                if(Mathf.Abs(yDifference) >= threshold.y){
                    newPosition.y = follow.y;
                }
                float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
                transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            }
        }else{
            if(player == null){
                transform.position = Vector3.MoveTowards(transform.position, endPositionOnDeath, endSceneSpeed * Time.deltaTime);
            }
        }
    }

    public void StartEndScene(Vector3 endPosition){
        endPositionOnDeath = endPosition;

        if(transform.position.y < 1.5f) {
            if(GameObject.Find("startplatform") != null){
                Destroy(GameObject.Find("startplatform"));
            }
        }
        startEndScene = true;
    }

    private Vector3 calculateThreshold(){
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
