using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;

public class MP_ChatUI : NetworkBehaviour
{
    public Text chatText = null;
    public InputField chatInput = null;
    public GameObject scorecardPanel;
    public GameObject chatUIPanel;
    public Text scoreplayerName;
    public Text scorekills;

    NetworkVariableString messages = new NetworkVariableString("Temp");

    public NetworkList<PlayerInfo> chatPlayers;
    private string playerName = "N/A";
    // Start is called before the first frame update
    void Start()
    {
       messages.OnValueChanged += updateUIClientRpc;

       foreach(PlayerInfo player in chatPlayers)
       {
           if(NetworkManager.LocalClientId == player.networkClientID)
           {
               playerName = player.networkPlayerName;
           }
       }
    }

    private void Update()
    {
        if (IsOwner)
        {
            updateUIScoreServerRpc();
            if(Input.GetKeyDown(KeyCode.U))
            {
                scorecardPanel.SetActive(true);  
            }
            else if(Input.GetKeyDown(KeyCode.I))
            {
                scorecardPanel.SetActive(false);
                chatUIPanel.SetActive(false);
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                chatUIPanel.SetActive(true);
            }
        }
        
    }

    public void handleSend()
    {
        if(!IsServer)
        {
            sendMessageServerRpc(chatInput.text);
        }
        else
        {
            messages.Value +=  playerName + ":" + chatInput.text + "\n";
        }

    }

    [ClientRpc]

    private void updateUIClientRpc(string previousValue, string newValue)
    {
        chatText.text += newValue.Substring(previousValue.Length, newValue.Length - previousValue.Length);
    }


    [ServerRpc]
    private void sendMessageServerRpc(string text, ServerRpcParams serverRpcParams = default)
    {
        foreach(PlayerInfo player in chatPlayers)
        {
            if(serverRpcParams.Receive.SenderClientId == player.networkClientID)
            {
               playerName = player.networkPlayerName;
            }
        }
        messages.Value += playerName + ":" + text + "\n"; 
    }

    [ServerRpc]

    public void updateUIScoreServerRpc(ServerRpcParams svrParam = default)
    {
        clearUIScoreClientRpc();
        GameObject[] currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject playerObj in currentPlayers)
        {
            foreach(PlayerInfo playerInfo in chatPlayers)
            {
                if(playerObj.GetComponent<NetworkObject>().OwnerClientId == playerInfo.networkClientID)
                {
                    updateUIScoreClientRpc(playerInfo.networkPlayerName, playerObj.GetComponent<PlayerStats>().kills.Value);
                }
            }
        }
    }

    [ClientRpc]

    private void clearUIScoreClientRpc()
    {
        if(IsOwner)
        {
            scoreplayerName.text = "";
            scorekills.text = "";
        }
    }

    [ClientRpc]

    private void updateUIScoreClientRpc(string networkPlayerName, int kills)
    {
        if (IsOwner)
        {
            scoreplayerName.text += networkPlayerName + "\n";
            scorekills.text += kills + "\n";
        }
    }
}
