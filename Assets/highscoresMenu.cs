using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class highscoresMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene("Player Names");
        }
        if (Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
