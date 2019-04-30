using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public GameObject gameOverText, heart1, heart2, heart3;
    public Text scoreText;
    public bool gameOver = false;
    public static int health = 3;
    public float scrollSpeed = -1.5f;

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
    }
    
    // Update is called once per frame
    void Update()
    {

        if (!gameOver)
        {
            if (health > 3)
                health = 3;

            switch (health)
            {
                case 3:
                    heart1.gameObject.SetActive(true);
                    heart2.gameObject.SetActive(true);
                    heart3.gameObject.SetActive(true);
                    break;
                case 2:
                    heart1.gameObject.SetActive(false);
                    heart2.gameObject.SetActive(true);
                    heart3.gameObject.SetActive(true);
                    break;
                case 1:
                    heart1.gameObject.SetActive(false);
                    heart2.gameObject.SetActive(false);
                    heart3.gameObject.SetActive(true);
                    break;
                case 0:
                    heart1.gameObject.SetActive(false);
                    heart2.gameObject.SetActive(false);
                    heart3.gameObject.SetActive(false);
                    EndOfGame();
                    break;

            }
        }
        else
        {
            scrollSpeed = 0;
            if (Input.GetKey("space"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }else if (Input.GetKey("escape"))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }

    }

    public void EndOfGame()
    {
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void Score()
    {
        if (gameOver)
            return;

        score++;
        scoreText.text = "Score: " + score.ToString();
    }
}
