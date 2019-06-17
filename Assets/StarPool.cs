using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPool : MonoBehaviour
{
    public int poolSize = 10;
    public GameObject obstaclePrefab;

    public float maxYPos = 1.4f;
    public float minYPos = -1.8f;
    public float maxXPos = 9f;
    public float minXPos = -9f;


    private GameObject[] stars;
    private Vector2 starPoolPosition = new Vector2(-40, -40);
    private float timeSinceLastSpawned;
    private int currentStar = 0;
    private float nextSpawn = 2;

    // Start is called before the first frame update
    void Start()
    {
        stars = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            stars[i] = (GameObject)Instantiate(obstaclePrefab, starPoolPosition, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameControl.instance.gameOver && timeSinceLastSpawned >= nextSpawn && !GameControl.instance.pause)
        {
            //First generate the last obstacle/heart calculated (so we can space out obstacles by difficuly = give more time for more difficult combinations)
            timeSinceLastSpawned = 0;
            float spawnYPos = Random.Range(minYPos, maxYPos);
            float spawnXPos = Random.Range(minXPos, maxXPos);

            stars[currentStar].transform.position = new Vector2(spawnXPos, spawnYPos);
            //stars[currentStar].SetActive(true);

            currentStar++;
            if (currentStar == poolSize)
                currentStar = 0;

        }
    }
}
