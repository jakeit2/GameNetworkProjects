using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;


public class Objectives : NetworkBehaviour
{
    public GameObject objectiveUI;

    void Start()
    {
        StartCoroutine(Wait());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("Wait is over");
        objectiveUI.SetActive(false);
    }
}
