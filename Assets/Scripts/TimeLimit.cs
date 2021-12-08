using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;
using MLAPI.SceneManagement;

public class TimeLimit : NetworkBehaviour
{
    public Text timerText;
    public GameObject timeOver;
    private float startTime;
    public float timeLimit = 300;

    void Start()
    {
        StartCoroutine(reloadTimer(timeLimit));
    }

    IEnumerator reloadTimer(float reloadTimeInSeconds)
    {
        float counter = 0;

        while (counter < reloadTimeInSeconds)
        {
            counter += Time.deltaTime;
            string minutes = ((int) counter / 60).ToString();
            string seconds = (counter % 60).ToString("f2"); 
            timerText.text = minutes + "." + seconds;
            yield return null;
            
        }

        timeOver.SetActive(true);
        NetworkSceneManager.SwitchScene("LobbyArea");
        Debug.Log("Time is up");
    }
}
