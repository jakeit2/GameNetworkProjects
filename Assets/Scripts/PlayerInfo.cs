using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Serialization;

public struct PlayerInfo : INetworkSerializable
{
    public ulong networkClientID;
    public string networkPlayerName;
    public bool networkPlayerReady;

    public PlayerInfo(ulong newClientID, string newPName, bool playerReady)
    {
        networkClientID = newClientID;
        networkPlayerName = newPName;
        networkPlayerReady = playerReady;
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref networkClientID);
        serializer.Serialize(ref networkPlayerName);
        serializer.Serialize(ref networkPlayerReady);
    }
}
