using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using MLAPI.SceneManagement;
using MLAPI.NetworkVariable.Collections;

public class MPLobby : NetworkBehaviour
{
    [SerializeField] private LobbyPlayerPanel[] lobbyPlayers;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Button startGameButton;

    private NetworkList<PlayerInfo> newPlayers = new NetworkList<PlayerInfo>();

    [SerializeField] private GameObject chatPrefab;
    void Start()
    {
        if(IsOwner)
        {
           UpdateConnListServerRpc(NetworkManager.LocalClientId); 
        }
        
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    public override void NetworkStart()
    {
        Debug.Log("Starting Server");

        newPlayers.OnListChanged += HandlePlayersInfoChanged;

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnectedHandle;
            
        }

    }
    [ServerRpc]

    private void UpdateConnListServerRpc(ulong clientId)
    {
        newPlayers.Add(new PlayerInfo(clientId, PlayerPrefs.GetString("PName"), false));
    }
    private void HandlePlayersInfoChanged(NetworkListEvent<PlayerInfo> changeEvent)
    {
        Debug.Log("List Updated");
        int index = 0;
        foreach (PlayerInfo connectedplayer in newPlayers)
        {
            lobbyPlayers[index].playerName.text = connectedplayer.networkPlayerName;
            lobbyPlayers[index].readyIcon.SetIsOnWithoutNotify(connectedplayer.networkPlayerReady);
            index++;
        }

        for(; index < 4; index++)
        {
           lobbyPlayers[index].playerName.text = "Player Name";
           lobbyPlayers[index].readyIcon.SetIsOnWithoutNotify(false);
           index++;
 
        }

        if(IsHost)
        {
           startGameButton.gameObject.SetActive(true);
           startGameButton.interactable = CheckEveryoneReady();
        }
    }

    public void StartGame()
    {
        if (IsServer)
        {
            NetworkSceneManager.OnSceneSwitched += SceneSwitched;
            NetworkSceneManager.SwitchScene("GameWorld");
        }
        else
        {
            Debug.Log("You are not the host");
        }
    }

    public void ReadyButtonPressed()
    {
        ReadyUpServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]

    public void ReadyUpServerRpc(ServerRpcParams serverRpcParams = default)
    {
       for (int indx = 0; indx < newPlayers.Count; indx++)
       {
           if (newPlayers[indx].networkClientID == serverRpcParams.Receive.SenderClientId)
           {
               Debug.Log("Updated with new");
               newPlayers[indx] = new PlayerInfo(newPlayers[indx].networkClientID, newPlayers[indx].networkPlayerName, !newPlayers[indx].networkPlayerReady);
           }
       } 
    }


    private void SceneSwitched()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach(PlayerInfo tmpClient in newPlayers)
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            int index = UnityEngine.Random.Range(0, spawnPoints.Length);
            GameObject currentPoint = spawnPoints[index];

            GameObject playerSpawn = Instantiate(playerPrefab, currentPoint.transform.position, Quaternion.identity);
            playerSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(tmpClient.networkClientID);
            Debug.Log("Player spawned for: " + tmpClient.networkPlayerName);

            GameObject chatUISpawn = Instantiate(chatPrefab);
            chatUISpawn.GetComponent<NetworkObject>().SpawnWithOwnership(tmpClient.networkClientID);
            chatUISpawn.GetComponent<MP_ChatUI>().chatPlayers = newPlayers;
            Debug.Log("Chat spawned for: " + tmpClient.networkPlayerName);
        }
    }

    private bool CheckEveryoneReady()
    {
        foreach(PlayerInfo player in newPlayers)
        {
           if(!player.networkPlayerReady)
           {
               return false;
           } 
        }
        return true;
    }



    private void HandleClientConnected(ulong clientId)
    {
        if(IsOwner)
        {
            UpdateConnListServerRpc(clientId);
        }
        
        Debug.Log("A Player has connected ID: " + clientId);
    }

    

    private void ClientDisconnectedHandle(ulong clientId)
    {
        for (int indx = 0; indx < newPlayers.Count; indx++)
        {
            if(clientId == newPlayers[indx].networkClientID)
            {
                newPlayers.RemoveAt(indx);
                Debug.Log("Player ID: " + clientId + " has left.");

                break;
            }
        }
    }

}
