using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;

public class PlayerStats : NetworkBehaviour
{
    public Slider healthbar;
    private float maxHp = 100f;
    private NetworkVariableFloat currentHP = new NetworkVariableFloat(100f);
    private float damgeVal = 20f;
    public GameObject deathScreen;
    public bool playerDied = false;
    
    // Update is called once per frame
    void Update()
    {
        healthbar.value = currentHP.Value / maxHp;
        /*if(playerDied == true)
        {
                Debug.Log("Death Screen");
                deathScreen.SetActive(true);
        }*/
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet") && IsOwner)
        {
            TakeDamageServerRpc(damgeVal);
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Medkit") && IsOwner)
        {
            HealDamageServerRpc();
        }
    }

    [ServerRpc]
    private void TakeDamageServerRpc(float damage)
    {
        currentHP.Value -= damage;
        if(currentHP.Value < 0)
        {
            Debug.Log("You died");
            Destroy(this.gameObject);
            //playerDied = true;
        }
    }

    [ServerRpc]
    public void HealDamageServerRpc()
    {
        currentHP.Value += 25f;
        if(currentHP.Value > maxHp)
        {
            currentHP.Value = maxHp;
        }
    }
}
