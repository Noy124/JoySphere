using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    public int poolSize = 5;
    public GameObject obstaclePrefab;
    public float spawnRate= 4f;
    public float maxYPos = 2.5f;

    private GameObject[] obstacles;
    private Vector2 obstaclePoolPosition = new Vector2(-40, -40);
    private float timeSinceLastSpawned;
    private float spawnXPos = 13f;
    private int currentObstacle = 0;
  
    // Start is called before the first frame update
    void Start()
    {
        obstacles = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            obstacles[i] = (GameObject)Instantiate(obstaclePrefab, obstaclePoolPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameControl.instance.gameOver && timeSinceLastSpawned >=spawnRate)
        {
            timeSinceLastSpawned = 0;
            float spawnYPos = Random.Range(-maxYPos, maxYPos);
            ColorControl.color color = (ColorControl.color)Random.Range(1,8);
            obstacles[currentObstacle].transform.position = new Vector2(spawnXPos, spawnYPos);
            obstacles[currentObstacle].SetActive(true);
            SpriteRenderer sr = obstacles[currentObstacle].GetComponent<SpriteRenderer>();
            switch (color)
            {
                case ColorControl.color.black:sr.color = Color.black;break;
                case ColorControl.color.blue: sr.color = Color.blue; break;
                case ColorControl.color.green: sr.color = Color.green; break;
                case ColorControl.color.purple: sr.color = Color.magenta; break;
                case ColorControl.color.red: sr.color = Color.red; break;
                case ColorControl.color.yellow: sr.color = Color.yellow; break;
                case ColorControl.color.orange: sr.color = ColorControl.orange; break;

            }
            currentObstacle++;
            if (currentObstacle >= 5)
                currentObstacle = 0;
        }
    }
}
