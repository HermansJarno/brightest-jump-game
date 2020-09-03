using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public SpriteRenderer referenceSprite;

    void Start()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = referenceSprite.bounds.size.x / referenceSprite.bounds.size.y;

        if(screenRatio >= targetRatio){
            Camera.main.orthographicSize = referenceSprite.bounds.size.y / 2;
        }else{
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = referenceSprite.bounds.size.y / 2 * differenceInSize;
        }
    }
}
