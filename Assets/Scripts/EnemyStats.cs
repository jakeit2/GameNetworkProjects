using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UnityEngine.UI;

public class EnemyStats : NetworkBehaviour
{
    public Slider healthbar;
    private float maxHp = 100f;
    private NetworkVariableFloat currentHP = new NetworkVariableFloat(100f);
    public bool enemyDied = false;
    private float damgeVal = 20f;
    
    void Update()
    {
        healthbar.value = currentHP.Value / maxHp;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            if(collision.gameObject.GetComponent<MP_Bullet>().spawnerPlayerId != NetworkManager.Singleton.LocalClientId)
            {

                //if(currentHP.Value - damgeVal < 0)
                //{
                  //  IncreaseKillCountServerRpc(collision.gameObject.GetComponent<MP_Bullet>().spawnerPlayerId);
                //}

                TakeDamageServerRpc(damgeVal);
                Destroy(collision.gameObject);
            }
        }
    }

    [ServerRpc]
    private void TakeDamageServerRpc(float damage, ServerRpcParams svrParams = default)
    {
        currentHP.Value -= damage;
        if(currentHP.Value < 0 && OwnerClientId == svrParams.Receive.SenderClientId)
        {
            Debug.Log("Enemy died");
            //playerDied = true;
        }
    }

}
