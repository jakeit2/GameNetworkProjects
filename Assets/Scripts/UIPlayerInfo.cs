using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayerInfo : MonoBehaviour
{
     [SerializeField] public Text playerName;

    internal void UpdatePlayerName(Text playerNameIn)
    {
        playerName = playerNameIn;
    }
    
}
