using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickContinue : MonoBehaviour
{
    public void ContinueOnClick()
    {
        SceneManager.LoadScene("Player Names");
    }
}
