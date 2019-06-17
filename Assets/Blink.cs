using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blink : MonoBehaviour
{
    public Animator animator;
    public float maxYPos = 1.4f;
    public float minYPos = -1.8f;
    public float maxXPos = 9f;
    public float minXPos = -9f;

    private float timeSinceBlink;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game") && !GameControl.instance.pause))
        {
            {
                int shouldPlay = Random.Range(0, 500);
                if (shouldPlay == 1)
                {
                    animator.Play("star animation");
                    //timeSinceBlink = 0;
                    //float posX = Random.Range(minXPos, maxXPos);
                    //float posY = Random.Range(minYPos, maxYPos);

                    //this.transform.position = new Vector2(posX, posY);
                }
            }
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Game"))
            {
                int shouldPlay = Random.Range(0, 200);
                if (shouldPlay == 1)
                {
                    animator.Play("star animation");
                }
            }
        }

    }
}
