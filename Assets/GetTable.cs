using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetTable : MonoBehaviour
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

    public static HighScoreLine[] hsTable;
    public int amountOfEntriesToSave = 10;
    public GameObject errorText;

    private Transform entryContainer;
    private Transform entryTemplate;


    // Start is called before the first frame update
    void Awake()
    {
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
                    else if(Input.GetKeyDown("E")){
                        SceneManager.LoadScene("Main Menu");
                    }   
                }

                yield return webRequest.SendWebRequest();
            }

            DownloadHandler dh = webRequest.downloadHandler;
            string[] jsonLines = dh.text.Replace("[","").Replace("]","").Split(new string[] { "},"}, System.StringSplitOptions.None);

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
        printTable();
        CreateTable();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void printTable()
    {
        for (int i = 0; i < hsTable.Length; i++)
        {
            Debug.Log("Player a: " + hsTable[i].player_a + ", Player b: " + hsTable[i].player_b + ", Score: " + hsTable[i].score);
        }
    }

    void CreateTable() {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
       
        
        HighScoreLine[] highScoreData = hsTable;

        float templateStartingHeight = 35f;
        float spaceBetweenEntries = 25f;

        for (int i = 0; i < amountOfEntriesToSave && i<highScoreData.Length; i++)
        {
            Debug.Log(i);
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -(templateStartingHeight + spaceBetweenEntries * i));

            Transform positionField = entryTransform.Find("position");
            Transform playerAField = entryTransform.Find("PlayerA");
            Transform playerBField = entryTransform.Find("PlayerB");
            Transform scoreField = entryTransform.Find("Score");

            positionField.GetComponent<Text>().text = (i + 1).ToString();
            playerAField.GetComponent<Text>().text = highScoreData[i].player_a;
            playerBField.GetComponent<Text>().text = highScoreData[i].player_b;
            scoreField.GetComponent<Text>().text = highScoreData[i].score.ToString();
            entryTransform.gameObject.SetActive(true);
        }
    }
}
