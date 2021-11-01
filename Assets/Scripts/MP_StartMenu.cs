using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;
using MLAPI.SceneManagement;

public class MP_StartMenu : NetworkBehaviour
{
    [SerializeField] private InputField playerName;
    public void HostButtonClicked()
    {
        PlayerPrefs.SetString("PName", playerName.text);
        NetworkManager.Singleton.StartHost();
        NetworkSceneManager.SwitchScene("LobbyArea");
    }

    public void ClientButtonClicked()
    {
        PlayerPrefs.SetString("PName", playerName.text);
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Started");
    } 
}
