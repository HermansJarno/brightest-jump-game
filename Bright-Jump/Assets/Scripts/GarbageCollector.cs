using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    private Transform trashbin;

    private void Start() {
        trashbin = GameObject.FindGameObjectWithTag("trashbin").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(trashbin.position.y > transform.position.y){
            Destroy(gameObject);
        }
    }
}
