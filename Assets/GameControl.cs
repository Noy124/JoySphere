using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO.Ports;
using UnityEngine.Networking;

//This class manages the game in general. It controls the life system, the game over and restart, and the pause menu. Any general calculations not related to a specific object should be made here
public class GameControl : MonoBehaviour
{
    [System.Serializable]
    public class HighScoreLine
    {
        public string player_a;
        public string player_b;
        public int score;

        public static HighScoreLine CreateFromJSON(string jsonString)
        {
            //Debug.Log(jsonString);
            return JsonUtility.FromJson<HighScoreLine>(jsonString);
        }
    }

    public static GameControl instance; //Note that this class is a singleton, and this is the single instance. From this you get the rest of the functions
    public GameObject gameOverText, noHearts, oneHeart, twoHearts, threeHearts, fourHearts, fiveHearts, sixHearts, sevenHearts, eightHearts, nineHearts, pauseText, errorText, player;
    public Text scoreText, timeText, rankText, toNextRankText;
    public bool gameOver = false;
    public bool pause = false;
    public static int health = 9;
    public int totalObstacles = 0, totalCaught=0;

    //This is the only thing you may change, changes the scrolling speed of scrolling objects (right now obstacles and hearts)
    public float scrollSpeed = -1.5f;

    private float defaultScrollSpeed;
    private int score = 0;
    private float originalSpeed;
    private HighScoreLine[] hsTable;
    private float runTime;
    private int currentRank, scoreToNextRank;

    // Start is called before the first frame update
    void Awake()
    {
        //Setting up singleton status
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        health = 9;
        defaultScrollSpeed = scrollSpeed;

        StartCoroutine(GetRequest("https://joysphere-high-scores.azurewebsites.net/api/GetHighScores?code=wWAwKQYv3wiP2LAFmrW7baBle5Iux66d1Dk8qYZ/xvoG8aHMFJEG7A=="));

       
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            yield return webRequest.SendWebRequest();

            while (webRequest.isNetworkError)
            {
                bool gotInput = false;
                errorText.SetActive(true);
                while (!gotInput)
                {
                    if (Input.GetKeyDown("R"))
                    {
                        gotInput = true;
                    }
                    else if (Input.GetKeyDown("E"))
                    {

                        if (Movement.spMovement.IsOpen)
                            Movement.spMovement.Close();
                        if (ColorControl.sp.IsOpen)
                            ColorControl.sp.Close();
                        SceneManager.LoadScene("Main Menu");
                    }
                }

                yield return webRequest.SendWebRequest();
            }

            DownloadHandler dh = webRequest.downloadHandler;
            string[] jsonLines = dh.text.Replace("[", "").Replace("]", "").Split(new string[] { "}," }, System.StringSplitOptions.None);

            hsTable = new HighScoreLine[jsonLines.Length];
            for (int i = 0; i < jsonLines.Length; i++)
            {
                if (i < jsonLines.Length - 1)
                {
                    hsTable[i] = HighScoreLine.CreateFromJSON(jsonLines[i] + "}");
                }
                else
                {
                    hsTable[i] = HighScoreLine.CreateFromJSON(jsonLines[i]);
                }
                //Debug.Log("Player a: " + hsTable[i].player_a + ", Player b: " + hsTable[i].player_b + ", Score: " + hsTable[i].score);
            }

        }

        currentRank = hsTable.Length;
        scoreToNextRank = hsTable[currentRank - 1].score;

    }
    // Update is called once per frame
    void Update()
    {

        //Setting up the health bar. I had a problem with the resolution so I made the hearts a single image that changes instead of 9 seperate hearts
        if (!gameOver && !pause)
        {
            if (health > 10)
                health = 10;

            switch (health)
            {
                case 10:case 9: nineHearts.gameObject.SetActive(true); break;
                case 8: nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(true); break;
                case 7:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(true); break;
                case 6:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(false);
                    sixHearts.gameObject.SetActive(true); break;
                case 5:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(false);
                    sixHearts.gameObject.SetActive(false);
                    fiveHearts.gameObject.SetActive(true); break;
                case 4:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(false);
                    sixHearts.gameObject.SetActive(false);
                    fiveHearts.gameObject.SetActive(false);
                    fourHearts.gameObject.SetActive(true); break;
                case 3:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(false);
                    sixHearts.gameObject.SetActive(false);
                    fiveHearts.gameObject.SetActive(false);
                    fourHearts.gameObject.SetActive(false);
                    threeHearts.gameObject.SetActive(true); break;
                case 2:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(false);
                    sixHearts.gameObject.SetActive(false);
                    fiveHearts.gameObject.SetActive(false);
                    fourHearts.gameObject.SetActive(false);
                    threeHearts.gameObject.SetActive(false);
                    twoHearts.gameObject.SetActive(true); break;
                case 1:
                    nineHearts.gameObject.SetActive(false);
                    eightHearts.gameObject.SetActive(false);
                    sevenHearts.gameObject.SetActive(false);
                    sixHearts.gameObject.SetActive(false);
                    fiveHearts.gameObject.SetActive(false);
                    fourHearts.gameObject.SetActive(false);
                    threeHearts.gameObject.SetActive(false);
                    twoHearts.gameObject.SetActive(false);
                    oneHeart.gameObject.SetActive(true); break;

                case 0:
                    threeHearts.gameObject.SetActive(false);
                    twoHearts.gameObject.SetActive(false);
                    oneHeart.gameObject.SetActive(false);
                    EndOfGame();
                    break;

            }

            runTime += Time.deltaTime;
            timeText.GetComponent<Text>().text = "Time: " + (int)runTime;
            if(score > scoreToNextRank && currentRank>1)
            {
                currentRank--;
                scoreToNextRank = hsTable[currentRank - 1].score;
            }

            rankText.GetComponent<Text>().text = "Rank: " + currentRank;
            toNextRankText.GetComponent<Text>().text = "Next Rank: " + scoreToNextRank;
            //Pause menu
            if (Input.GetKey(KeyCode.P))
            {
                scrollSpeed = 0;
                pause = true;
                originalSpeed = player.GetComponent<Animator>().speed;
                player.GetComponent<Animator>().speed = 0;

                pauseText.SetActive(true);
            }


        }

        //Game over menu
        else if (gameOver)
        {
            scrollSpeed = 0;
            nineHearts.gameObject.SetActive(false);
            eightHearts.gameObject.SetActive(false);
            sevenHearts.gameObject.SetActive(false);
            sixHearts.gameObject.SetActive(false);
            fiveHearts.gameObject.SetActive(false);
            fourHearts.gameObject.SetActive(false);
            threeHearts.gameObject.SetActive(false);
            twoHearts.gameObject.SetActive(false);
            oneHeart.gameObject.SetActive(false);
            noHearts.gameObject.SetActive(true);
            

            //Sending names and scores and changing the scene to highscores
            if (Input.GetKey("space"))
            {
                if(Movement.spMovement.IsOpen)
                    Movement.spMovement.Close();
                if(ColorControl.sp.IsOpen)
                    ColorControl.sp.Close();

                string playerA = GlobalControl.Instance.playerA;
                string playerB = GlobalControl.Instance.playerB;
                StartCoroutine(GetRequest("https://joysphere-high-scores.azurewebsites.net/api/InsertScore?code=tBGQNyRYHaDY1JtoG/YkNEaeCg2RT6VjxZsFkRty57l9LoOKQG1CiA==&player_a=" + playerA + "&player_b=" + playerB + "&score=" + score));

                System.Threading.Thread.Sleep(2000);
                SceneManager.LoadScene("Highscores");
            }
        }

        else
        {
            scrollSpeed = 0;
            if (Input.GetKey(KeyCode.C))
            {
                //Continues the game from pause
                pauseText.SetActive(false);
                scrollSpeed = defaultScrollSpeed;
                player.GetComponent<Animator>().speed = originalSpeed;
                pause = false;
            } else if (Input.GetKey(KeyCode.E))
            {

                if (Movement.spMovement.IsOpen)
                    Movement.spMovement.Close();
                if (ColorControl.sp.IsOpen)
                    ColorControl.sp.Close();
                //Exits to main menu
                SceneManager.LoadScene(0);
            }
        }

    }

    public void EndOfGame()
    {
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void Score(int amount = 1)
    {
        //Calculating score and updating text
        if (gameOver)
            return;

        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }

    
}
