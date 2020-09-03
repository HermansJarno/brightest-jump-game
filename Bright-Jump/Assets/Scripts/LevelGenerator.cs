using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public List<GameObject> bugPrefabs;
    public List<GameObject> cloudPrefabs;
    public GameObject coinPrefab;

    public Transform spawnPoint;

    public Transform spawnBarrier;

    public float minY = 0.2f;
    public float maxY = 1.5f;

    public int numberOfPlatformsPerHeight = 50;

    private int minimumPlatformsPerHeight = 2;

    private Vector3 world;

    // Start is called before the first frame update
    void Start()
    {
       world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnPoint.position.y > spawnBarrier.position.y){
            Vector3 spawnPosition = new Vector3();
            spawnPosition.y = spawnPoint.position.y;
            for (int i = 0; i < numberOfPlatformsPerHeight; i++)
            {
                spawnPosition.y += Random.Range(minY, maxY);
                spawnPosition.x = Random.Range(-(world.x),(world.x));

                Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

                //coins
                int nbr = Random.Range(0,2);
                if(nbr == 1){
                    Instantiate(coinPrefab, new Vector3(spawnPosition.x ,spawnPosition.y + 0.5f, coinPrefab.transform.position.z), Quaternion.identity);
                }
                
                // clouds
                if(i % 3 == 0){
                    int rndIndex = Random.Range(0, cloudPrefabs.Count);
                    Instantiate(cloudPrefabs[rndIndex], new Vector3(spawnPosition.x *-1,spawnPosition.y + Random.Range(1, 4), cloudPrefabs[rndIndex].transform.position.z), Quaternion.identity);
                }

                // bugs
                if(i % 2 == 0 && (spawnPosition.x > 1 || spawnPosition.x < -1)){
                    int rndIndex = Random.Range(0, bugPrefabs.Count);
                    int rnd = Random.Range(0,2);
                    if(rnd == 1){
                        Instantiate(bugPrefabs[rndIndex], new Vector3(spawnPosition.x *-1, spawnPosition.y, bugPrefabs[rndIndex].transform.position.z), Quaternion.identity);
                    } else {
                        GameObject instance = Instantiate(bugPrefabs[rndIndex], new Vector3(spawnPosition.x *-1, spawnPosition.y, bugPrefabs[rndIndex].transform.position.z), Quaternion.identity) as GameObject;
                        instance.transform.localScale = new Vector3(instance.transform.localScale.x * -1, instance.transform.localScale.y, instance.transform.localScale.z);
                    }
                }
            }
            spawnBarrier.position = new Vector3(0, spawnPosition.y,0);
        }
    }
}
