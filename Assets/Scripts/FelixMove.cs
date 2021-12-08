using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelixMove : MonoBehaviour
{

    public float speed = 6f;

    public CharacterController controller;

    private Rigidbody rigid; 
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        Vector3 characterMove = new Vector3(Horizontal, 0.0f, Vertical).normalized;

        if(characterMove.magnitude >= 0.1f)
        {
            controller.Move(characterMove * speed * Time.deltaTime);
        }
    }
}
