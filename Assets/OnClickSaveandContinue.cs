using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickSaveandContinue : MonoBehaviour
{
    public GameObject playerAName, playerBName;

    public void SaveAndPlay()
    {
        GlobalControl.Instance.playerA = playerAName.GetComponent<InputField>().text;
        GlobalControl.Instance.playerB = playerBName.GetComponent<InputField>().text;
        SceneManager.LoadScene("Game");
    }
}
