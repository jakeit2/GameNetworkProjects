using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;

public class MMPlayerMovement : NetworkBehaviour
{
    public float movemenetSpeed = 5f;
    public float rotationSpeed = 100f;
    public Transform camT;
    CharacterController mpCharController;

    
    // Start is called before the first frame update
    void Start()
    {
        mpCharController = GetComponent<CharacterController>();
        if(IsOwner)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        if(!IsOwner)
        {
            camT.GetComponent<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
            MPMovePlayer();
        }
        
    }

    void MPMovePlayer()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        mpCharController.SimpleMove(forward * movemenetSpeed * Input.GetAxis("Vertical"));
    }
}
