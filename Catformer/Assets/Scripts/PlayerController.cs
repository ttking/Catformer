using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float inputDirection;
    private float verticalVelocity;

    private float speed = 5.0f;
    private float gravity = 30.0f;
    private float jumpForce = 12.0f;
    private bool secondJumpAvail;


    private Vector3 moveVector;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        inputDirection = Input.GetAxis("Horizontal") * speed;

        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (verticalVelocity > 0)
            {
                verticalVelocity = -5;
            }
        }

        if (controller.isGrounded)
        {
            verticalVelocity = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = 10;
                secondJumpAvail = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))

            {
                if (secondJumpAvail)
                {
                    verticalVelocity = 10;
                    secondJumpAvail = false;
                }
            }

            verticalVelocity -= gravity * Time.deltaTime;

        }

        moveVector = new Vector3(inputDirection, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);

    }

    private bool IsControllerGrounded()
    {
        Debug.DrawRay(controller.bounds.center, Vector3.down, Color.cyan);
        return false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //collectible
        switch (hit.gameObject.tag)
        {
            case "Coin":
                Destroy(hit.gameObject);
                break;
            default:
                break;
        }

    }

}
