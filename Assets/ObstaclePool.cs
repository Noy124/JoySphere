using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{

    public int poolSize = 5;
    public GameObject obstaclePrefab;
    //public float spawnRate= 4f;
    public float maxYPos = 1.4f;
    public float minYPos = -1.8f;
    public float difficultyRaiseRate = 1f;
    public int raiseDifficultyEverySeconds = 5;
    //public int startingSpawnRate = 5;
    public int heartChance = 10;
    public GameObject heart;
    public Camera camera;

    private GameObject[] obstacles;
    private Vector2 obstaclePoolPosition = new Vector2(-40, -40);
    private float timeSinceLastSpawned;
    private float spawnXPos = 20f;
    private int currentObstacle = 0;
    private float nextSpawn = 5;
    static private float minNextSpawn = 4f;
    static private float maxNextSpawn = 6f;
    private float startTime;
    static private int singleColorChance = 75;
    static private int doubleColorChance = 25;
    //private float lastSpawnTime=0f;
    private float lastChangeTime = 0;
    private ColorControl.color nextObstacleColor=ColorControl.color.blue;
    private bool isNextHeart = false;
    private ColorControl.color[] last2Colors;
    private int last2ColorsI=0;


    // Start is called before the first frame update
    void Start()
    {
        obstacles = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            obstacles[i] = (GameObject)Instantiate(obstaclePrefab, obstaclePoolPosition, Quaternion.identity);
        }
        startTime = Time.time;
        last2Colors = new ColorControl.color[2] {ColorControl.color.white, ColorControl.color.white };
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameControl.instance.gameOver && timeSinceLastSpawned >=nextSpawn && !GameControl.instance.pause)
        {
            timeSinceLastSpawned = 0;
            float spawnYPos = Random.Range(minYPos, maxYPos);
            if (isNextHeart)
            {
                isNextHeart = false;
                heart.transform.position = new Vector2(spawnXPos, spawnYPos);
                heart.SetActive(true);
            }
            else
            {
                obstacles[currentObstacle].transform.position = new Vector2(spawnXPos, spawnYPos);
                obstacles[currentObstacle].SetActive(true);
                SpriteRenderer sr = obstacles[currentObstacle].GetComponent<SpriteRenderer>();

                switch (nextObstacleColor)
                {
                    case ColorControl.color.black: sr.color = Color.black; break;
                    case ColorControl.color.blue: sr.color = Color.blue; break;
                    case ColorControl.color.green: sr.color = Color.green; break;
                    case ColorControl.color.purple: sr.color = Color.magenta; break;
                    case ColorControl.color.red: sr.color = Color.red; break;
                    case ColorControl.color.yellow: sr.color = Color.yellow; break;
                    case ColorControl.color.cyan: sr.color = Color.cyan; break;

                }
            }
            int nextSpawnMult=1;
            int chance = Random.Range(0, 100);
            Debug.Log(chance);
            if ( chance > heartChance || camera.WorldToScreenPoint(heart.GetComponent<Transform>().position).x > 0)
            {
                nextSpawnMult = ChooseNextColor();
            }
            else
            {
                Debug.Log("Heart!");
                isNextHeart = true;
            }
            
            currentObstacle++;
            if (currentObstacle >= poolSize)
                currentObstacle = 0;

            nextSpawn = Random.Range(minNextSpawn, maxNextSpawn)*nextSpawnMult;
        }
        scaleDifficulty();
    }

    void scaleDifficulty()
    {
        //float timeSinceStart = Time.time - startTime;
        //Debug.Log("time: "+(int)timeSinceStart);
        lastChangeTime += Time.deltaTime;
        if (lastChangeTime / raiseDifficultyEverySeconds>1)
        {
            lastChangeTime = 0;
            if (minNextSpawn > 1 && maxNextSpawn > 2)
            {
                minNextSpawn -= 0.1f * difficultyRaiseRate;
                maxNextSpawn -= 0.1f * difficultyRaiseRate;
            }

            if (singleColorChance >= 50)
            {
                singleColorChance -= (int)(1 * difficultyRaiseRate);
                doubleColorChance += (int)(1 * difficultyRaiseRate);
            }else if (singleColorChance >= 25)
            {
                singleColorChance -= (int)(1 * difficultyRaiseRate);
            }
        }

    }

    int ChooseNextColor()
    {
        int randomPick = Random.Range(0, 100);
        int nextSpawnMult;
        ColorControl.color tempColor;

        if (randomPick < singleColorChance)
        {
            tempColor = (ColorControl.color)Random.Range(1, 4);
            //Debug.Log(randomPick + " Single " + singleColorChance);
            nextSpawnMult = 1;

        }
        else if (randomPick < doubleColorChance + singleColorChance)
        {
            tempColor = (ColorControl.color)Random.Range(4, 7);
            //Debug.Log(randomPick + " Double" + singleColorChance);
            nextSpawnMult = 2;

        }
        else
        {
            tempColor = ColorControl.color.black;
            //Debug.Log(randomPick + " Black" + singleColorChance);
            nextSpawnMult = 3;
        }

        if (tempColor == last2Colors[0] && last2Colors[0] == last2Colors[1]) {
            Debug.Log("Stopped a threesome :)");
            return ChooseNextColor();
        }
        else
        {
            nextObstacleColor = tempColor;

            last2Colors[last2ColorsI % 2] = nextObstacleColor;
            last2ColorsI++;
        }
            
        return nextSpawnMult;
    }

    public static void LowerDifficulty()
    {
        minNextSpawn *= 1.5f;
        maxNextSpawn *= 1.5f;
        singleColorChance += 10;
        doubleColorChance = 100 - singleColorChance;
    }
}
