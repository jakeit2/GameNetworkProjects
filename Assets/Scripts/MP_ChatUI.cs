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


}
