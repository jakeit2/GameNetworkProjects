using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using MLAPI.SceneManagement;
using MLAPI.NetworkVariable.Collections;

public class Mp_BulletSpawner : NetworkBehaviour
{
    public Rigidbody bullet;
    public Transform bulletPos;
    private float bulletSpeed = 10f;
    public AudioSource audioSource;
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && IsOwner)
        {
            FireServerRpc();
            audioSource.Play();
        }
    }
    [ServerRpc]
    
    private void FireServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Fire weapon");
        Rigidbody bulletClone = Instantiate(bullet, bulletPos.position, transform.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
        bulletClone.GetComponent<MP_Bullet>().spawnerPlayerId = serverRpcParams.Receive.SenderClientId;
        bulletClone.gameObject.GetComponent<NetworkObject>().Spawn();
        Destroy(bulletClone.gameObject, 3);
    }
}
