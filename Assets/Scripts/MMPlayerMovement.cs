using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class MMPlayerMovement : NetworkBehaviour
{
    public float movemenetSpeed = 5f;
    public Transform camT;
    CharacterController mpCharController;
    
    // Start is called before the first frame update
    void Start()
    {
        mpCharController = GetComponent<CharacterController>();
        if(IsLocalPlayer)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        if(!IsLocalPlayer)
        {
            camT.GetComponent<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLocalPlayer)
        {
            MPMovePlayer();
        }
        
    }

    void MPMovePlayer()
    {
        Vector3 moveVect = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        mpCharController.SimpleMove(moveVect * movemenetSpeed);
    }
}
