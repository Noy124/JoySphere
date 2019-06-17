using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is probably the most complicated code. Generating the obstacle pool. It creates poolSize amount of obstacles which are recycled once the pool was all used up (if poolSize= 5, instead of creating a new sixth obstacle the first obstacle will be given a new color and spawn position and will be recycled)
//Play the game and watch from a distance for a while, you will get it
public class ObstaclePool : MonoBehaviour
{


    public int poolSize = 10;// How many objects should the game make. More obstacles = no obstacles dissapearing on screen, though might load slower
    public GameObject obstaclePrefab;

    //Unless you change the size of the screen (which is not reccomended), don't touch these. they determine the max and min y position for obstacle spawn
    public float maxYPos = 1.4f;
    public float minYPos = -1.8f;

    //These two are about the difficulty. These are the parameters to change while testing out the game
    public float difficultyRaiseRate = 1f;
    public int raiseDifficultyEverySeconds = 5;


    public static bool shouldLowerDifficulty = false;

    public GameObject heart;
    public Camera myCamera;

    public SpriteRenderer firstFish, secondFish;

    private GameObject[] obstacles;
    private Vector2 obstaclePoolPosition = new Vector2(20, 0);
    private float timeSinceLastSpawned;
    private float spawnXPos = 20f;
    private int currentObstacle = 0;
    private float nextSpawn = 2;
    private float startTime;

    //More parameters to change when testing out the difficulty. First two are the starting frequency range, last two are the starting chances for each color (black chance is 100 - singleColorChance - doubleColorChance)
    static private float minNextSpawn = 2f;
    static private float maxNextSpawn = 4f;
    static private int singleColorChance = 75;
    static private int doubleColorChance = 25;

    //private float lastSpawnTime=0f;
    private float lastChangeTime = 0;
    private Color nextObstacleColor=Color.blue;
    private bool isNextHeart = false;
    private Color[] last2Colors;
    private int last2ColorsI=0;
    private int heartChance = 10;
    private bool isFirstFish=false;
    float fishSize=1f;



    // Start is called before the first frame update
    void Start()
    {
        obstacles = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            obstacles[i] = (GameObject)Instantiate(obstaclePrefab, obstaclePoolPosition, Quaternion.identity);
            obstacles[i].SetActive(false);
        }
        startTime = Time.time;
        last2Colors = new Color[2] {Color.white, Color.white };
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameControl.instance.gameOver && timeSinceLastSpawned >=nextSpawn && !GameControl.instance.pause)
        {
            //First generate the last obstacle/heart calculated (so we can space out obstacles by difficuly = give more time for more difficult combinations)
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
                if (isFirstFish)
                {
                    obstacles[currentObstacle].GetComponent<SpriteRenderer>().sprite = firstFish.sprite;
                }
                else
                {
                    obstacles[currentObstacle].GetComponent<SpriteRenderer>().sprite = secondFish.sprite;

                }

                obstacles[currentObstacle].transform.localScale = new Vector3 (fishSize,fishSize,0);
                obstacles[currentObstacle].transform.position = new Vector2(spawnXPos, spawnYPos);
                obstacles[currentObstacle].GetComponent<Obstacle>().alreadyInjuredCat = false;
                obstacles[currentObstacle].SetActive(true);
                SpriteRenderer sr = obstacles[currentObstacle].GetComponent<SpriteRenderer>();

                sr.color = nextObstacleColor;

            }

            //Calculate the next obstacle
            int nextSpawnMult=1;
            int chance = Random.Range(0, 100);
            if ( chance > heartChance || myCamera.WorldToScreenPoint(heart.GetComponent<Transform>().position).x > 0)
            {
                nextSpawnMult = ChooseNextColor();
            }
            else
            {
                isNextHeart = true;
            }

            isFirstFish = Random.Range(0, 2) == 1;
            switch(Random.Range(1, 6))
            {
                case 1: fishSize = 1f;break;
                case 2: fishSize = 1.5f;break;
                case 3: fishSize = 2f;break;
                case 4: fishSize = 2.5f;break;
                case 5: fishSize = 3f;break;
            }

            currentObstacle++;
            if (currentObstacle >= poolSize)
                currentObstacle = 0;

            //When to spawn next obstacle/heart
            nextSpawn = Random.Range(minNextSpawn, maxNextSpawn)*nextSpawnMult;
        }

        //Scale the difficulty
        scaleDifficulty();
    }

    void scaleDifficulty()
    {
        //raising the difficulty as time passes
        lastChangeTime += Time.deltaTime;
        if (lastChangeTime / raiseDifficultyEverySeconds>1)
        {
            lastChangeTime = 0;
            
            //Raising obstacle frequency
            if (minNextSpawn > 1 && maxNextSpawn > 2)
            {
                minNextSpawn -= 0.1f * difficultyRaiseRate;
                maxNextSpawn -= 0.1f * difficultyRaiseRate;
            }

            //Raising chance for harder combinations
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
        int tempColorNum;
        Color tempColor = Color.white;

        if (randomPick < singleColorChance)
        {
            tempColorNum = Random.Range(1, 4);
            nextSpawnMult = 1;

        }
        else if (randomPick < doubleColorChance + singleColorChance)
        {
            tempColorNum = Random.Range(4, 7);
            nextSpawnMult = 2;

        }
        else
        {
            tempColorNum = 7;
            nextSpawnMult = 3;
        }

        switch (tempColorNum)
        {
            case 1: tempColor = Color.red; break;
            case 2: tempColor = Color.blue;break;
            case 3: tempColor = Color.green;break;
            case 4: tempColor = Color.magenta;break;
            case 5: tempColor = Color.cyan;break;
            case 6: tempColor = Color.yellow;break;
            case 7: tempColor = Color.black; break;

        }

        //Making sure we didn't get the same color three times in a row
        if (tempColor == last2Colors[0] && last2Colors[0] == last2Colors[1]) {
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
        if (shouldLowerDifficulty)
        {
            //If the player lost a life, lower difficuly TODO: If missed it lower frequency only, if hit with wrong color lower difficult combination chance only
            minNextSpawn *= 1.5f;
            maxNextSpawn *= 1.5f;
            singleColorChance += 10;
            doubleColorChance = 100 - singleColorChance;
        }
    }
}
