using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public GameObject gameOverText, oneHeart, twoHearts, threeHearts, pauseText;
    public Text scoreText;
    public bool gameOver = false;
    public bool pause = false;
    public static int health = 3;
    public float scrollSpeed = -1.5f;

    private float defaultScrollSpeed;

    private int score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
        health = 3;
        defaultScrollSpeed = scrollSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (!gameOver && !pause)
        {
            if (health > 3)
                health = 3;

            switch (health)
            {
                case 3:
                    threeHearts.gameObject.SetActive(true);
                    twoHearts.gameObject.SetActive(false);
                    oneHeart.gameObject.SetActive(false);
                    break;
                case 2:
                    threeHearts.gameObject.SetActive(false);
                    twoHearts.gameObject.SetActive(true);
                    oneHeart.gameObject.SetActive(false);
                    break;
                case 1:
                    threeHearts.gameObject.SetActive(false);
                    twoHearts.gameObject.SetActive(false);
                    oneHeart.gameObject.SetActive(true);
                    break;
                case 0:
                    threeHearts.gameObject.SetActive(false);
                    twoHearts.gameObject.SetActive(false);
                    oneHeart.gameObject.SetActive(false);
                    EndOfGame();
                    break;

            }

            if (Input.GetKey(KeyCode.P))
            {
                scrollSpeed = 0;
                pause = true;
                pauseText.SetActive(true);
                //new WaitForSecondsRealtime(5);
            }
        }
        else if(gameOver)
        {
            scrollSpeed = 0;
            if (Input.GetKey("space"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            scrollSpeed = 0;
            if (Input.GetKey(KeyCode.C))
            {
                pauseText.SetActive(false);
                scrollSpeed = defaultScrollSpeed;
                pause = false;
            }else if (Input.GetKey(KeyCode.E))
            {
                SceneManager.LoadScene(0);
            }
        }

    }

    public void EndOfGame()
    {
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void Score(int amount=1)
    {
        if (gameOver)
            return;

        score+=amount;
        scoreText.text = "Score: " + score.ToString();
    }
}
